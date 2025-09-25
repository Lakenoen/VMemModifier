using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VMemReaderCore;
public class VMemManager
{
    private SortedDictionary<int, VMemStream> streams = new SortedDictionary<int, VMemStream>();
    private HashSet<DLLInjector.Key> dllHistoryInjection = new HashSet<DLLInjector.Key>();
    private Timer clearTimer;
    private List<int> delKeys = new List<int>();
    private object streamsLocker = new();
    protected ILogger logger = Log.Instance.Factory.CreateLogger<VMemManager>();
    public VMemStream this[int id]
    {
        get {
            lock (streamsLocker)
            {
                if (streams.ContainsKey(id))
                    return streams[id];
                if (!VMemStream.isProcessStarted(id))
                    throw new ArgumentException($"The process with these IDs is not launched, id: {id}");
                VMemStream stream = new VMemStream(id);
                streams.Add(id, stream);
                return streams[id];
            }
        }
    }
    public VMemManager()
    {
        clearTimer = new Timer(new TimerCallback(clearClosedProcess), 0, 0, 5000);
    }
    public int create(string path, IEnumerable<string>? args)
    {
        ProcessStartInfo startInfo;
        if (args != null)
            startInfo = new ProcessStartInfo(path, args);
        else
            startInfo = new ProcessStartInfo(path);

        Process? proc = Process.Start(startInfo);
        if (proc == null)
            throw new ApplicationException("The process was not launched");

        return proc.Id;
    }

    public void close(int id)
    {
        Process.GetProcessById(id).Kill();
    }

    public void inject(DLLInjector.Key key)
    {
        if (dllHistoryInjection.Contains(key))
            throw new ApplicationException($"DLL is already injected, name of DLL: {key.dllName}");
        try
        {
            DLLInjector.inject(key.id, key.dllName);
            dllHistoryInjection.Add(key);
        }
        catch (InjectionException ex)
        {
            logger.LogDebug("Injection Error: " + ex.Message);
            throw;
        }

    }

    public void clearClosedProcess(object? obj)
    {
        try
        {
            lock (streamsLocker)
            {
                foreach (var el in streams)
                {
                    if (el.Value.isProcStarted())
                        continue;
                    delKeys.Add(el.Key);
                }

                foreach (int key in delKeys)
                    streams.Remove(key);
            }
            delKeys.Clear();
        } catch (Exception ex){
            Console.WriteLine($"Error: There was a problem when closing the flow of virtual memory: {ex.Message}");
        }
    }

}

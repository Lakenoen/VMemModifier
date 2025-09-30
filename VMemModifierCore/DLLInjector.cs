using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using Microsoft.Extensions.Logging;

namespace VMemReaderCore;

public class InjectionException : Exception
{
    public DLLInjector.Status state { get; private set; } = DLLInjector.Status.SUCCESS;
    public InjectionException(string msg) :base(msg)
    {

    }

    public InjectionException(string msg, DLLInjector.Status state) : base(msg)
    {
        this.state = state;
    }

}
public class DLLInjector
{

    private static ILogger logger = Log.Instance.Factory.CreateLogger<InjectionException>();

    [DllImport("VMemModifierDLL", CallingConvention = CallingConvention.Cdecl)]
    protected static extern void inject(int id, [MarshalAs(UnmanagedType.LPWStr)]string dllPath, out short status);
    public static void inject(int id, string dllPath)
    {
        short status = 0;
        inject(id, dllPath, out status);
        if (status == 0)
            return;
        string errorMsg = stateToString((Status)status);
        logger.LogDebug($"There was a problem with injecting dll: {errorMsg}");
        throw new InjectionException(errorMsg, (Status)status);
    }

    private static string stateToString(Status state) => state switch
    {
        Status.SUCCESS => "Success",
        Status.OPEN_PROCESS_ERROR => "Process was not opened",
        Status.LOAD_MODULE_ERROR => "Target module was not be loaded",
        Status.LOAD_LIB_ERROR => "The address of the function was not received",
        Status.ALLOC_ERROR => "Memory was not allocate",
        Status.WRITE_PROCESS_ERROR => "Write into process error",
        Status.INJECT_ERROR => "Inject error",
        _ => "Undefined",
    };

    public enum Status
    {
        SUCCESS = 0,
        OPEN_PROCESS_ERROR = -1,
        LOAD_MODULE_ERROR = -2,
        LOAD_LIB_ERROR = -3,
        ALLOC_ERROR = -4,
        WRITE_PROCESS_ERROR= -5,
        INJECT_ERROR = -6,
    }

    public class Key
    {
        public int id { get; set; } = 0;
        public string dllName { get; set; } = "";

        public Key()
        {

        }

        public Key(int id, string dllName)
        {
            this.id = id;
            this.dllName = dllName;
        }

        public Key((int id, string dllName) key)
        {
            this.id = key.id;
            this.dllName = key.dllName;
        }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(id);
            builder.Append(dllName);
            using MD5 hash = MD5.Create();
            byte[] h = hash.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString()));
            return BitConverter.ToInt32(h);
        }

    }

}

using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace VMemReaderCore;

public abstract class VMemStreamNative
{
    [DllImport("VMemReaderDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern void readMemory(int id, long addr, long size, [MarshalAs(UnmanagedType.LPArray)] byte[] buffer, out ulong readed, out ulong error);

    [DllImport("VMemReaderDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern void getMemInfo(int id, long addr, ref MemInfoNative info, out ulong error);

    [DllImport("VMemReaderDLL", CallingConvention = CallingConvention.Cdecl)]
    private static extern void writeMemory(int id, long addr, [MarshalAs(UnmanagedType.LPArray)] byte[] data, long size, out ulong written, out ulong error);

    protected ILogger logger = Log.Instance.Factory.CreateLogger<VMemStreamNative>();
    protected byte[] read(int id,long addr, long size)
    {
        if (getInfo(id, addr) is null)
            throw new Exception("The address does not belong to the process");

        byte[] data = new byte[size];
        ulong readed = 0;
        ulong error = 0;
        readMemory(id, addr, size, data, out readed, out error);

        if (readed == 0)
        {
            logger.LogDebug("Error: Read method returned an empty array, code: " + Convert.ToString(error));
            return new byte[0];
        }
        return data;
    }

    protected bool write(int id,long addr, byte[] data)
    {
        if (getInfo(id, addr) is null)
            throw new Exception("The address does not belong to the process");

        ulong written = 0;
        ulong error = 0;
        writeMemory(id, addr, data, data.Length, out written, out error);

        if (written <= 0)
        {
            Console.WriteLine("Error: Write method returned an false, code: " + Convert.ToString(error));
            return false;
        }
        return true;
    }

    protected MemInfoNative? getInfo(int id, long addr)
    {
        MemInfoNative result = new MemInfoNative();
        ulong error = 0;
        getMemInfo(id, addr, ref result, out error);
        if (result.state == 0)
        {
            logger.LogDebug($"getMemoryInfo method ended with an error: {error}");
            return null;
        }
        return result;
    }

    public abstract Data read(long addr, long size);
    public abstract bool write(long addr, Data data);

    [StructLayout(LayoutKind.Sequential)]
    protected struct MemInfoNative()
    {
        public long baseAddr = 0;
        public long protect = 0;
        public long regionSize = 0;
        public ulong state = 0;
        public long minAddr = 0;
        public long maxAddr = 0;
    }

    public enum MemInfoState
    {
        COMMIT = 0x1000,
        FREE = 0x10000,
        RESERVE = 0x2000,
        ANY = 0x0,
    }

    public enum ACCESS : long
    {
        NOACCESS = 0x01,
        GUARD = 0x100,
        EXECUTE = 0x10,
        EXECUTE_READ = 0x20,
        EXECUTE_READWRITE = 0x40,
        PAGE_EXECUTE_WRITECOPY = 0x80,
        READWRITE = 0x04,
        READONLY = 0x02,
        WRITECOPY = 0x08,
        TARGETS_INVALID = 0x40000000,
        TARGETS_NO_UPDATE = 0x40000000,
        NOCACHE = 0x200,
        WRITECOMBINE = 0x400,
    }

}

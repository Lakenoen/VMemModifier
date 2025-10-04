using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMemReaderCore;
public class VMemStream : VMemStreamNative
{

    const int BLOCK_SIZE = 0x1000;

    public SearchAlgorithms.Search search { get; set; } = SearchAlgorithms.bmSearch;
    public int Id { get; private set; }
    public VMemStream(int id) : base()
    {
        this.Id = id;
    }

    public override Data read(long addr, long size)
    {
        if (!isProcStarted())
            throw new ApplicationException("The process was closed");
        if(size > BLOCK_SIZE)
            throw new ArgumentException("Size should be less then block size");
        return new Data(base.read(this.Id, addr, size));
    }
    public override bool write(long addr, Data data)
    {
        if (!isProcStarted())
            throw new ApplicationException("The process was closed");
        if (data.data.Count > BLOCK_SIZE)
            throw new ArgumentException("Size should be less then block size");
        return base.write(this.Id, addr, data.data.ToArray());
    }

    public MemInfo? getInfo(long addr)
    {
        if (!isProcStarted())
            throw new ApplicationException("The process was closed");
        MemInfo result = new MemInfo();
        MemInfoNative? info = base.getInfo(this.Id, addr);
        if (info is null)
            return null;
        MemInfoNative temp = info.Value;
        ConvertInfoFromNative( ref result, ref temp);
        return result;
    }

    private void forEach(int step, Action<Data> payload)
    {
        MemInfo? info = this.getInfo(0);
        if (info is null)
            throw new Exception("Failed to open the process, you may not have sufficient permissions");
        long minAddr = info.Value.minAddr;
        long maxAddr = info.Value.maxAddr;
        forEach(minAddr, maxAddr, step, payload);
    }

    public void forEach(long start, long end, int step, Action<Data> payload)
    {
        MemInfo? info = null;
        for (long addr = start; addr < end; addr += nextSize(info = this.getInfo(addr)))
        {
            if (info is null) continue;
            if (info.Value.state != MemInfoState.COMMIT || (info.Value.protect & ((long)ACCESS.NOACCESS | (long)ACCESS.GUARD)) != 0)
                continue;
            Data data;
            long i = addr;
            while (i < addr + info.Value.regionSize)
            {
                long chunkSize = long.Min(BLOCK_SIZE, addr + info.Value.regionSize - i);
                data = this.read(i, chunkSize);
                data.addr = i;
                payload(data);
                i += step;
            }
        }
    }

    public void forEach(Action<Data> payload)
    {
        forEach(BLOCK_SIZE, payload);
    }

    public HashSet<long> find(Data findData)
    {
        object sync = new object();
        if (findData.data.Count >= BLOCK_SIZE / 2)
            throw new ArgumentException("The desired element should be less than block size / 2");

        HashSet<long> results = new HashSet<long>();

        forEach(BLOCK_SIZE / 2, async data =>
        {
            if (data.data.Count == 0)
                return;
            await Task.Run(() =>
            {
                foreach (var item in search.Invoke(data.data, findData.data))
                {
                    lock (sync)
                    {
                        results.Add(data.addr + item);
                    }
                }
            });
        });
        return results;
    }

    public HashSet<long> find(long start, long end, Data findData)
    {
        object sync = new object();
        if (findData.data.Count >= BLOCK_SIZE / 2)
            throw new ArgumentException("The desired element should be less than block size / 2");

        HashSet<long> results = new HashSet<long>();

        forEach(start, end, BLOCK_SIZE / 2, async data =>
        {
            if (data.data.Count == 0)
                return;
            await Task.Run(() =>
            {
                foreach (var item in search.Invoke(data.data, findData.data))
                {
                    lock (sync)
                        results.Add(data.addr + item);
                }
            });
        });
        return results;
    }

    private long nextSize(MemInfo? info)
    {
        if (info == null)
            return BLOCK_SIZE;
        return (info.Value.regionSize > 0) ? info.Value.regionSize : BLOCK_SIZE;
    }

    private void ConvertInfoFromNative(ref MemInfo target ,ref MemInfoNative nativeInfo)
    {
        target.baseAddr = nativeInfo.baseAddr;
        target.protect = nativeInfo.protect;
        target.regionSize = nativeInfo.regionSize;
        target.minAddr = nativeInfo.minAddr;
        target.maxAddr = nativeInfo.maxAddr;
        target.state = (MemInfoState)nativeInfo.state;
    }

    public bool isProcStarted()
    {
        return isProcessStarted(this.Id);
    }

    public static bool isProcessStarted(int id)
    {
        try
        {
            Process.GetProcessById(id);
        }
        catch (ArgumentException)
        {
            return false;
        }
        return true;
    }

    public struct MemInfo(long baseAddr, long protect, long regionSize, MemInfoState state, long minAddr, long maxAddr)
    {
        public long baseAddr = baseAddr;
        public long protect = protect;
        public long regionSize = regionSize;
        public MemInfoState state = state;
        public long minAddr = minAddr;
        public long maxAddr = maxAddr;
    }

}

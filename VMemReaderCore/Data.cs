using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VMemReaderCore;
public class Data : IEnumerable, ICloneable
{
    public List<byte> data { get; protected set; } = new List<byte>();

    public long addr { get; set; } = 0;

    public byte this[int index]
    {
        get => data[index];
        set => data.Insert(index, value);
    }

    public string getString(Encoding encoding)
    {
        byte[] temp = data.Slice(0, data.Count).ToArray();
        return encoding.GetString(temp);
    }

    public void setString(Encoding encoding, string data)
    {
        this.data.Clear();
        this.data.InsertRange(0, encoding.GetBytes(data));
    }

    public IEnumerable<int> getIntArray()
    {
        List<int> result = new List<int>();
        int size = sizeof(int);
        for (int i = 0; i < this.data.Count; i += size)
        {
            int readSize = Math.Min(size, this.data.Count - size);
            int val = BitConverter.ToInt32(data.Slice(i, (readSize == 0) ? size : readSize).ToArray());
            result.Add(val);
        }
        return result;
    }

    public IEnumerable<long> getLongArray()
    {
        List<long> result = new List<long>();
        int size = sizeof(long);
        for (int i = 0; i < this.data.Count; i += size)
        {
            int readSize = Math.Min(size, this.data.Count - size);
            long val = BitConverter.ToInt64(data.Slice(i, (readSize == 0) ? size : readSize).ToArray());
            result.Add(val);
        }
        return result;
    }

    public IEnumerable<short> getShortArray()
    {
        List<short> result = new List<short>();
        int size = sizeof(short);
        for (int i = 0; i < this.data.Count; i += size)
        {
            int readSize = Math.Min(size, this.data.Count - size);
            short val = BitConverter.ToInt16(data.Slice(i, (readSize == 0) ? size : readSize).ToArray());
            result.Add(val);
        }
        return result;
    }

    public IEnumerable<double> getDoubleArray()
    {
        List<double> result = new List<double>();
        int size = sizeof(double);
        for (int i = 0; i < this.data.Count; i += size)
        {
            int readSize = Math.Min(size, this.data.Count - size);
            double val = BitConverter.ToDouble(data.Slice(i, (readSize == 0) ? size : readSize).ToArray());
            result.Add(val);
        }
        return result;
    }

    public override string ToString()
    {
        return this.getString(Encoding.ASCII);
    }

    public IEnumerable<float> getFloatArray()
    {
        List<float> result = new List<float>();
        int size = sizeof(float);
        for (int i = 0; i < this.data.Count; i += size)
        {
            int readSize = Math.Min(size, this.data.Count - size);
            float val = BitConverter.ToSingle(data.Slice(i, (readSize == 0) ? size : readSize).ToArray());
            result.Add(val);
        }
        return result;
    }

    public void setIntArray(IEnumerable<int> arr)
    {
        this.data.Clear();
        int i = 0;
        foreach(int val in arr)
        {
            this.data.InsertRange(0,BitConverter.GetBytes(val));
            i += sizeof(int);
        }
    }

    public void setLongArray(IEnumerable<long> arr)
    {
        this.data.Clear();
        int i = 0;
        foreach (long val in arr)
        {
            this.data.InsertRange(0, BitConverter.GetBytes(val));
            i += sizeof(long);
        }
    }

    public void setShortArray(IEnumerable<short> arr)
    {
        this.data.Clear();
        int i = 0;
        foreach (short val in arr)
        {
            this.data.InsertRange(0, BitConverter.GetBytes(val));
            i += sizeof(short);
        }
    }

    public void setDoubleArray(IEnumerable<double> arr)
    {
        this.data.Clear();
        int i = 0;
        foreach (double val in arr)
        {
            this.data.InsertRange(0, BitConverter.GetBytes(val));
            i += sizeof(double);
        }
    }

    public void setFloatArray(IEnumerable<float> arr)
    {
        this.data.Clear();
        int i = 0;
        foreach (float val in arr)
        {
            this.data.InsertRange(0, BitConverter.GetBytes(val));
            i += sizeof(float);
        }
    }

    public static Data fromIntArray(IEnumerable<int> arr)
    {
        return new Data(arr);
    }

    public static Data fromLongArray(IEnumerable<long> arr)
    {
        return new Data(arr);
    }

    public static Data fromShortArray(IEnumerable<short> arr)
    {
        return new Data(arr);
    }

    public static Data fromDoubleArray(IEnumerable<double> arr)
    {
        return new Data(arr);
    }

    public static Data fromFloatArray(IEnumerable<float> arr)
    {
        return new Data(arr);
    }

    public static Data fromStringArray(Encoding enc, string str)
    {
        return new Data(enc,str);
    }

    public Data()
    {

    }
    public Data(byte[] bytes)
    {
        this.data = bytes.ToList();
    }

    public Data(long addr, byte[] bytes)
    {
        this.addr = addr;
        this.data = bytes.ToList();
    }
    
    public Data(Encoding encoder, string data)
    {
        this.setString(encoder, data);
    }

    public Data(IEnumerable<int> arr)
    {
        this.setIntArray(arr);
    }

    public Data(IEnumerable<long> arr)
    {
        this.setLongArray(arr);
    }
    public Data(IEnumerable<short> arr)
    {
        this.setShortArray(arr);
    }

    public Data(IEnumerable<double> arr)
    {
        this.setDoubleArray(arr);
    }

    public Data(IEnumerable<float> arr)
    {
        this.setFloatArray(arr);
    }

    public List<byte> ToList()
    {
        return this.data;
    }

    public bool isEmpty() => ToList().Count == 0;

    public IEnumerator GetEnumerator() => data.GetEnumerator();

    public object Clone()
    {
        return new Data(this.addr, this.data.ToArray());
    }

}

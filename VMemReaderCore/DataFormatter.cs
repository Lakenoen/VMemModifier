using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VMemReaderCore;

public static class DataFormatter
{
    public class Format
    {
        public const short HEX = 0x1;
        public const short INT = 0x2;
        public const short LONG = 0x4;
        public const short SHORT = 0x8;
        public const short DOUBLE = 0x10;
        public const short FLOAT = 0x20;
        public const short BYTE = 0x40;
        public const short STR = 0x80;
        public const short ASCII = 0x100;
        public const short UTF8 = 0x200;
        public const short UNICODE = 0x400;
        public const short BIN = 0x800;
    }

    private const int BINFORMATSIZE = 8;

    public static string format(Data data, in short format) => format switch
    {
        Format.HEX | Format.STR => hexFormat(data),
        Format.HEX | Format.INT => decHexFormat(data, Format.HEX | Format.INT),
        Format.HEX | Format.LONG => decHexFormat(data, Format.HEX | Format.LONG),
        Format.HEX | Format.DOUBLE => decHexFormat(data, Format.HEX | Format.DOUBLE),
        Format.HEX | Format.FLOAT => decHexFormat(data, Format.HEX | Format.FLOAT),
        Format.HEX | Format.SHORT => decHexFormat(data, Format.HEX | Format.SHORT),
        Format.HEX | Format.BYTE => decHexFormat(data, Format.HEX | Format.BYTE),
        Format.STR | Format.ASCII => strFormat(data, Encoding.ASCII),
        Format.STR | Format.UTF8 => strFormat(data, Encoding.UTF8),
        Format.STR | Format.UNICODE => strFormat(data, Encoding.Unicode),
        Format.STR => strFormat(data, Encoding.ASCII),
        Format.BIN => binFormat(data),
        Format.INT => decFormat(data, Format.INT),
        Format.LONG => decFormat(data, Format.LONG),
        Format.DOUBLE => decFormat(data, Format.DOUBLE),
        Format.FLOAT => decFormat(data, Format.FLOAT),
        Format.SHORT => decFormat(data, Format.SHORT),
        Format.BYTE => decFormat(data, Format.BYTE),
        _ => data.ToString()
    };

    public static Data format(string data, in short format) => format switch
    {
        Format.HEX | Format.STR => hexFormat(data),
        Format.HEX | Format.INT => decHexFormat(data, Format.HEX | Format.INT),
        Format.HEX | Format.LONG => decHexFormat(data, Format.HEX | Format.LONG),
        Format.HEX | Format.DOUBLE => decHexFormat(data, Format.HEX | Format.DOUBLE),
        Format.HEX | Format.FLOAT => decHexFormat(data, Format.HEX | Format.FLOAT),
        Format.HEX | Format.SHORT => decHexFormat(data, Format.HEX | Format.SHORT),
        Format.HEX | Format.BYTE => decHexFormat(data, Format.HEX | Format.BYTE),
        Format.STR | Format.ASCII => strFormat(data, Encoding.ASCII),
        Format.STR | Format.UTF8 => strFormat(data, Encoding.UTF8),
        Format.STR | Format.UNICODE => strFormat(data, Encoding.Unicode),
        Format.STR => strFormat(data, Encoding.ASCII),
        Format.BIN => binFormat(data),
        Format.INT => decFormat(data, Format.INT),
        Format.LONG => decFormat(data, Format.LONG),
        Format.DOUBLE => decFormat(data, Format.DOUBLE),
        Format.FLOAT => decFormat(data, Format.FLOAT),
        Format.SHORT => decFormat(data, Format.SHORT),
        Format.BYTE => decFormat(data, Format.BYTE),
        _ => new Data()
    };

    private static string hexFormat(Data data)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in data)
        {
            sb.AppendFormat("{0:x2}", b).Append(" ");
        }
        return sb.ToString();
    }

    private static Data hexFormat(string data)
    {
        Data result = new Data();
        for (int i = 0; i < data.Length; i+= 2)
        {
            result[i / 2] = Convert.ToByte(data.Substring(i, 2), 16);
        }
        return result;
    }

    private static string decFormat(Data data, in short format)
    {
        StringBuilder sb = new StringBuilder();
        switch (format)
        {
            case Format.INT: data.getIntArray().ToList().ForEach(el => { sb.Append(el).Append(" "); }); break;
            case Format.LONG: data.getLongArray().ToList().ForEach(el => { sb.Append(el).Append(" "); }); break;
            case Format.SHORT: data.getShortArray().ToList().ForEach(el => { sb.Append(el).Append(" "); }); break;
            case Format.FLOAT: data.getFloatArray().ToList().ForEach(el => { sb.Append(el).Append(" "); }); break;
            case Format.DOUBLE: data.getDoubleArray().ToList().ForEach(el => { sb.Append(el).Append(" "); }); break;
            case Format.BYTE: data.ToList().ForEach(el => { sb.Append(el).Append(" "); }); break;
        };
        return sb.ToString();
    }

    private static Data decFormat(string data, in short format) => format switch
    {
        Format.INT => new Data(new List<int>() { Convert.ToInt32(data)}),
        Format.LONG => new Data(new List<long>() { Convert.ToInt64(data) }),
        Format.SHORT => new Data(new List<short>() { Convert.ToInt16(data) }),
        Format.FLOAT => new Data(new List<float>() { float.Parse(data) }),
        Format.DOUBLE => new Data(new List<double>() { double.Parse(data) }),
        Format.BYTE => new Data(new List<int>() { Convert.ToByte(data) }),
    };
    private static Data decHexFormat(string data, in short format) => format switch
    {
        Format.HEX | Format.INT => new Data(new List<int>() { Convert.ToInt32(data, 16) }),
        Format.HEX | Format.LONG => new Data(new List<long>() { Convert.ToInt64(data, 16) }),
        Format.HEX | Format.SHORT => new Data(new List<short>() { Convert.ToInt16(data, 16) }),
        Format.HEX | Format.FLOAT => new Data(new List<float>() { BitConverter.ToSingle(Convert.FromHexString(data)) }),
        Format.HEX | Format.DOUBLE => new Data(new List<double>() { BitConverter.ToDouble(Convert.FromHexString(data)) }),
        Format.HEX | Format.BYTE => new Data(new List<int>() { Convert.ToByte(data, 16) }),
    };
    private static string decHexFormat(Data data, in short format)
    {
        StringBuilder sb = new StringBuilder();
        switch (format)
        {
            case Format.HEX | Format.INT: data.getIntArray().ToList().ForEach(el => { sb.AppendFormat("{0:x2}", el).Append(" "); }); break;
            case Format.HEX | Format.LONG: data.getLongArray().ToList().ForEach(el => { sb.AppendFormat("{0:x2}", el).Append(" "); }); break;
            case Format.HEX | Format.SHORT: data.getShortArray().ToList().ForEach(el => { sb.AppendFormat("{0:x2}", el).Append(" "); }); break;
            case Format.HEX | Format.FLOAT: data.getFloatArray().ToList().ForEach(el => { sb.AppendFormat("{0:x2}", el).Append(" "); }); break;
            case Format.HEX | Format.DOUBLE: data.getDoubleArray().ToList().ForEach(el => { sb.AppendFormat("{0:x2}", el).Append(" "); }); break;
            case Format.HEX | Format.BYTE: data.ToList().ForEach(el => { sb.AppendFormat("{0:x2}", el).Append(" "); }); break;
        }
        ;
        return sb.ToString();
    }

    private static string binFormat(Data data)
    {
        StringBuilder result = new StringBuilder();
        foreach (byte item in data)
        {
            StringBuilder temp = new StringBuilder(Convert.ToString(item, 2));
            short tempLen = (short) temp.Length;
            if (tempLen < BINFORMATSIZE)
            {
                for (short i = 0; i < BINFORMATSIZE - tempLen; ++i)
                    temp.Insert(0, "0");
            }
            result.Append(temp);
            result.Append(' ');
        }
        result.Append("\r\n\r\n");
        return result.ToString();
    }

    private static Data binFormat(string data)
    {
        Data result = new Data();
        for (int i = 0; i < data.Length; i += 8)
            result[i / 8] = Convert.ToByte(data.Substring(i, Math.Min(8, data.Length - i)), 2);
        return result;
    }

    private static string strFormat(Data data, Encoding enc)
    {
        return data.getString(enc);
    }

    private static Data strFormat(string data, Encoding enc)
    {
        return new Data( enc.GetBytes(data) );
    }

}

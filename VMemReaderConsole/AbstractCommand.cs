using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using VMemReaderCore;

namespace VMemReaderConsole
{
    internal abstract class AbstractCommand
    {
        public abstract short allowFlags { get; init; }
        public short flags { get; set; } = 0;

        protected VMemManager manager { get; init; }
        public AbstractCommand(VMemManager manager) {
            this.manager = manager;
        }
        public abstract void run(params string[] parameters);
        private short convert(string key) => key switch
        {
            "str" => Flag.STR,
            "hex" => Flag.HEX,
            "bin" => Flag.BIN,
            "ascii" => Flag.ASCII,
            "utf8" => Flag.UTF8,
            "unicode" => Flag.UNICODE,
            "int" => Flag.INT,
            "long" => Flag.LONG,
            "float" => Flag.FLOAT,
            "double" => Flag.DOUBLE,
            "short" => Flag.SHORT,
            "byte" => Flag.BYTE,
            "reg" => Flag.REG,
            "name" => Flag.NAME,
            "flag" => Flag.FLAG,
            _=> throw new ArgumentException($"Missing flag - {key}")
        };
        public short getKeyFromString(string flag)
        {
            if (flag.StartsWith('-'))
                flag = flag.Remove(0, 1);
            short res = convert(flag.ToLower());
            if ( (res & allowFlags) == 0)
                throw new ArgumentException($"Invalid flag - {flag}");
            return res;
        }

        protected string getOutput(long addr, Data value)
        {
            string strVal = "";
            try
            {
                if (value.data.Count == 0)
                    strVal = "Empty";
                else
                    strVal = DataFormatter.format(value, this.flags);
            }
            catch (Exception e)
            {
                strVal = "??";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("Address: ").AppendFormat("{0:x2}", addr).Append($" Value: {strVal}\r\n");
            return sb.ToString();
        }
        public class Flag : DataFormatter.Format
        {
            public const short REG = 0x1000;
            public const short NAME = 0x2000;
            public const short FLAG = 0x4000;
        }

        public class CommandInfoException : ApplicationException
        {
            public CommandInfoException(string info) : base(info) { }

        }

    }

}

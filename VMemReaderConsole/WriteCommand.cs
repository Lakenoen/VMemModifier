using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace VMemReaderConsole;
internal class WriteCommand : AbstractCommand
{
    public override short allowFlags { get; init; }
        = AbstractCommand.Flag.STR | AbstractCommand.Flag.ASCII | AbstractCommand.Flag.UNICODE | AbstractCommand.Flag.UTF8
        | AbstractCommand.Flag.INT | AbstractCommand.Flag.DOUBLE | AbstractCommand.Flag.FLOAT | AbstractCommand.Flag.LONG
        | AbstractCommand.Flag.SHORT | AbstractCommand.Flag.BYTE | AbstractCommand.Flag.BIN | AbstractCommand.Flag.HEX;
    public WriteCommand(VMemManager manager) : base(manager)
    {

    }
    public override void run(params string[] parameters)
    {
        if (parameters.Length < 3)
            throw new ArgumentOutOfRangeException("There should be at least 3 parameters, enter -help to obtain detailed information");

        int id = int.Parse(parameters[0]);

        NumberStyles startFormasat = (parameters[1].Length > 2 && parameters[1].Substring(0, 2) == "0x")
            ? NumberStyles.HexNumber : NumberStyles.Number;

        if (startFormasat == NumberStyles.HexNumber)
            parameters[1] = parameters[1].Remove(0, 2);

        long addr = long.Parse(parameters[1], startFormasat);

        string dataStr = parameters[2];
        Data? data = DataFormatter.format(dataStr, base.flags);
        if (data == null)
            throw new ArgumentException("Unknown flag or prohibited combination of flags");

        if (!this.manager[id].write(addr, data))
            throw new CommandInfoException("Failed to record data");
        Console.WriteLine("Success");
    }
}

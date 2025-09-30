using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Markup;
using VMemReaderCore;

namespace VMemReaderConsole;
class SearchCommand : AbstractCommand
{
    public override short allowFlags { get; init; }
        = AbstractCommand.Flag.STR | AbstractCommand.Flag.ASCII | AbstractCommand.Flag.UNICODE | AbstractCommand.Flag.UTF8
        | AbstractCommand.Flag.INT | AbstractCommand.Flag.DOUBLE | AbstractCommand.Flag.FLOAT | AbstractCommand.Flag.LONG
        | AbstractCommand.Flag.SHORT | AbstractCommand.Flag.BYTE | AbstractCommand.Flag.BIN | AbstractCommand.Flag.HEX | AbstractCommand.Flag.REG;
    public SearchCommand(VMemManager manager) : base(manager)
    {

    }
    public override void run(params string[] parameters)
    {
        if (parameters.Length < 2)
            throw new ArgumentOutOfRangeException("There should be at least 2 parameters, enter -help to obtain detailed information");

        int id = int.Parse(parameters[0]);
        string dataStr = parameters[1];

        if ( (base.flags & Flag.STR) == 0 && dataStr.Length > 2 && dataStr.Substring(0, 2) == "0x")
            dataStr = dataStr.Remove(0, 2);

        if ((base.flags & Flag.STR) == 0 && dataStr.Length > 2 && dataStr.Substring(0, 2) == "0b")
            dataStr = dataStr.Remove(0, 2);

        Data? data = DataFormatter.format(dataStr, base.flags);
        if (data == null)
            throw new ArgumentException("Unknown flag or prohibited combination of flags");

        if ( (base.flags & AbstractCommand.Flag.REG ) != 0)
            base.manager[id].search = SearchAlgorithms.bmSearchRegEx;

        List<long> addrs = base.manager[id].find(data);

        base.manager[id].search = SearchAlgorithms.bmSearch;

        if (addrs.Count() == 0)
            throw new CommandInfoException("Nothing was found");

        for(int i = 0; i < addrs.Count; ++i)
        {
            Console.WriteLine(getOutput(addrs[i], base.manager[id].read(addrs[i], data.data.Count)));
        }

        Console.WriteLine("Success");
    }

}

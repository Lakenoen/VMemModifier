using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace VMemReaderConsole;
internal class SaveMemDumpCommand : AbstractCommand
{
    public override short allowFlags { get; init; } = 0;
    private const short FLUSH_THRESHOLD = 0x100;
    public SaveMemDumpCommand(VMemManager manager) : base(manager)
    {

    }
    public override void run(params string[] parameters)
    {
        if (parameters.Length < 2)
            throw new ArgumentOutOfRangeException("There should be at least 2 parameters, enter -help to obtain detailed information");
        base.flags |= AbstractCommand.Flag.STR | AbstractCommand.Flag.HEX;
        int id = int.Parse(parameters[0]);
        StringBuilder builder = new StringBuilder(parameters[1]);
        builder.Append(".mdump");
        FileStream file = File.Open(builder.ToString(), FileMode.CreateNew, FileAccess.Write);
        StreamWriter writer = new StreamWriter(file);
        long iter = 0;
        writer.Write("MDUMP\n");
        writer.Flush();
        manager[id].forEach( (Data data) => {
            writer.Write(base.getOutput(data.addr, data));
            if (iter % FLUSH_THRESHOLD == 0)
                writer.Flush();
            ++iter;
        });
        writer.Close();
        file.Close();
        Console.WriteLine("Success");
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace VMemReaderConsole;
internal class CloseCommand : AbstractCommand
{
    public override short allowFlags { get; init; } = 0;
    public CloseCommand(VMemManager manager) : base(manager)
    {

    }

    public override void run(params string[] parameters)
    {
        if (parameters.Length < 1)
            throw new ArgumentOutOfRangeException("There should be at least 2 parameters, enter -help to obtain detailed information");
        int id = int.Parse(parameters[0]);
        manager.close(id);
        Console.WriteLine("Success");
    }
}

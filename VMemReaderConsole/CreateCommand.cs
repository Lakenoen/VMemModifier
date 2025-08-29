using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace VMemReaderConsole;
internal class CreateCommand : AbstractCommand
{
    public override short allowFlags { get; init; } = 0;
    public CreateCommand(VMemManager manager) : base(manager)
    {

    }

    public override void run(params string[] parameters)
    {
        if (parameters.Length < 1)
            throw new ArgumentOutOfRangeException("There should be at least 2 parameters, enter -help to obtain detailed information");
        string path = parameters[0];
        manager.create(path, parameters.Take(1..parameters.Length) );
        Console.WriteLine("Success");
    }
}

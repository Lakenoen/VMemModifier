using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Pipelines;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace VMemReaderConsole;
class ConsoleInterface
{
    private static ConsoleInterface? intance = null;
    public static ConsoleInterface Instance
    {
        get
        {
            if(intance == null)
                intance = new ConsoleInterface();
            return intance;
        }
    }

    private ConsoleInterface() { }

    internal void process(string[] commandLine)
    {
        AbstractCommand command = CommandFactory.create(commandLine[0]);
        List<string> commandList = commandLine.ToList();
        commandList.RemoveAt(0);
        List<string> parameters = new List<string>();
        var arr = commandList.FindAll((string el) =>
        {
            if (el.First().Equals('-'))
                return true;

            parameters.Add(el);
            return false;
        });
        short flags = 0;
        if(arr.Count > 0)
            flags = arr.Select(str => command.getKeyFromString(str)).Aggregate((short a, short b) => {
                return (short)(a | b);
            });
        command.flags = flags;
        command.run(parameters.ToArray());
    }

}

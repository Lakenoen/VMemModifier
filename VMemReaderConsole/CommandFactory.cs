using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace VMemReaderConsole;
internal static class CommandFactory
{
    public static VMemManager manager { get; private set; } = new VMemManager();
    public static AbstractCommand create(string command) => command switch
    {
        "search" => new SearchCommand(manager),
        "read" => new ReadCommand(manager),
        "write" => new WriteCommand(manager),
        "inject" => new WriteCommand(manager),
        "close" => new CloseCommand(manager),
        "create" => new CreateCommand(manager),
        "help" => new HelpCommand(manager),
        "dump" => new SaveMemDumpCommand(manager),
        _ => throw new ArgumentException("Unknomn command")
    };
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using VMemModifierConsole;
using VMemReaderCore;

namespace VMemReaderConsole;

internal class HelpCommand : AbstractCommand
{
    public override short allowFlags { get; init; } = Flag.FLAG | Flag.NAME;

    public HelpCommand(VMemManager manager) : base(manager)
    {

    }

    public override void run(params string[] parameters)
    {

        string? param = null;
        if (parameters.Length > 0)
            param = parameters[0];

        bool isName = (base.flags & Flag.NAME) != 0;
        bool isFlag = (base.flags & Flag.FLAG) != 0;

        if (isName && isFlag)
            throw new ArgumentException("Only one of the flags - '-FLAG' or '-NAME'");

        byte[] bytes = HelpResource.HelpFile;
        JsonDocument doc = JsonDocument.Parse(bytes);
        JsonElement root = doc.RootElement;
        JsonElement commands = root.GetProperty("commands");
        commands.EnumerateArray();
        JsonElement.ArrayEnumerator commandArr = commands.EnumerateArray();
        using MemoryStream stream = new MemoryStream();
        using StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
        while (commandArr.MoveNext())
        {
            StringBuilder stringBuffer = new StringBuilder();
            string? name = commandArr.Current.GetProperty("name").GetString();

            if (isName && param != null && param != name)
                continue;

            string? description = commandArr.Current.GetProperty("description").GetString();

            if (description == null || description.Length == 0)
                description = "The description is absent";

            stringBuffer.AppendLine($"command: {name} - {description}");

            var flagArr = commandArr.Current.GetProperty("allowFlags").EnumerateArray();
            bool isExistFlag = false;
            while (flagArr.MoveNext())
            {
                string? flagName = flagArr.Current.GetProperty("flagName").GetString();
                string? flagDescription = flagArr.Current.GetProperty("description").GetString();

                if (isFlag && param != null && param != flagName)
                    continue;

                isExistFlag = true;

                if (flagDescription == null || flagDescription.Length == 0)
                    flagDescription = "The description is absent";

                stringBuffer.AppendLine($"flag: {flagName} - {flagDescription}");
            }

            if (!isExistFlag && isFlag)
                continue;

            stringBuffer.Append("\r\n");
            writer.WriteLine(stringBuffer.ToString());
            writer.Flush();
            stringBuffer.Clear();
        }
        if (stream.Length == 0)
            throw new CommandInfoException("Nothing was found");
        using StreamReader reader = new StreamReader(stream);
        stream.Seek(0, SeekOrigin.Begin);
        Console.WriteLine(reader.ReadToEnd());
    }

}

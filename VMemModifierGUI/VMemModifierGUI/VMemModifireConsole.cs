using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMemReaderCore;

namespace VMemModifierGUI;
static class VMemModifierConsole
{
    private const string PATH = "VMemModifierConsole.exe";
    private static ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = PATH,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true,
        UseShellExecute = false,
        WorkingDirectory = "."
    };
    public static string Exec(params string[] args)
    {
        startInfo.Arguments = string.Join(' ',args);
        
        StringBuilder processOutput = new StringBuilder();
        Process proc = new Process();
        proc.StartInfo = startInfo;
        proc.OutputDataReceived += (object sender, DataReceivedEventArgs e) => processOutput.Append(e.Data).Append('\n');
        proc.Start();
        proc.BeginOutputReadLine();
        proc.PriorityBoostEnabled = true;
        proc.PriorityClass = ProcessPriorityClass.High;
        proc.WaitForExit();

        return processOutput.ToString();
    }
    public static string ExecSearch(int id, string pattern, string start, string end, string flags, bool? isReg, bool? isHex)
    {
        string formattedFlag = formatterFlag(flags);

        if (isReg != null && isReg == true)
            formattedFlag += " -reg";

        if (isHex != null && isHex == true)
            formattedFlag += " -hex";

        VMemStream temp = new VMemStream(id);
        long? maxAddr = temp.getInfo(0)?.maxAddr;
        bool emptyEndAccess = false;

        if(maxAddr != null)
            emptyEndAccess = true;

        if ( (start.Length == 0 && end.Length == 0) || (start.Length != 0 && end.Length == 0 && !emptyEndAccess))
            return Exec("search", id.ToString(), pattern, formattedFlag);
        else if (start.Length == 0)
            return Exec("search", id.ToString(), pattern, "0", end, formattedFlag);
        else if (end.Length == 0 && emptyEndAccess)
            return Exec("search", id.ToString(), pattern, start, maxAddr.ToString()!, formattedFlag);
        else
            return Exec("search", id.ToString(), pattern, start, end, formattedFlag);
    }

    public static string ExecRead(int id, string address, string size, string flags, bool? isHex)
    {
        string formattedFlag = formatterFlag(flags);

        if (isHex != null && isHex == true)
            formattedFlag += " -hex";

        return Exec("read", id.ToString(), address, size, formattedFlag);
    }

    public static string ExecWrite(int id, string address, string strValue, string flags, bool? isHex)
    {
        string formattedFlag = formatterFlag(flags);
        if (isHex != null && isHex == true)
            formattedFlag += " -hex";
        return Exec("write",id.ToString(), address, strValue, formattedFlag);
    }
    private static string formatterFlag(string flags) => flags switch
    {
        "Utf-8 string" => "-str -utf8",
        "Ascii string" => "-str -ascii",
        "Unicode string" => "-str -unicode",
        "int" => "-int",
        "short" => "-short",
        "long" => "-long",
        "byte" => "-byte",
        "double" => "-double",
        "float" => "-float",
        "bin" => "-bin",
        _ => flags
    };

}

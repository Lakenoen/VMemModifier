using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMemReaderGUI;
static class VMemModifierConsole
{
    private const string PATH = "VMemModifierConsole.exe";
    public static string Exec(params string[] args)
    {
        StringBuilder builder = new StringBuilder();
        foreach (string arg in args) {
            builder.Append(arg).Append(" ");
        }
        ProcessStartInfo startInfo = new ProcessStartInfo(PATH);
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.Arguments = builder.ToString();
        Process proc = new Process();
        proc.StartInfo = startInfo;
        proc.Start();
        proc.WaitForExit();
        return proc.StandardOutput.ReadToEnd();
    }

    public static string ExecSearch(int id, string pattern, string start, string end, string flags, bool? isReg, bool? isHex)
    {
        string formattedFlag = "";
        switch (flags)
        {
            case "Utf-8 string" : formattedFlag = "-str -utf8"; break;
            case "Ascii string": formattedFlag = "-str -ascii"; break;
            case "Unicode string": formattedFlag = "-str -unicode"; break;
            case "int": formattedFlag = "-int"; break;
            case "short": formattedFlag = "-short"; break;
            case "long": formattedFlag = "-long"; break;
            case "byte": formattedFlag = "-byte"; break;
            case "double": formattedFlag = "-double"; break;
            case "float": formattedFlag = "-float"; break;
            case "bin": formattedFlag = "-bin"; break;
        }

        if (isReg != null && isReg == true)
            formattedFlag += " -reg";

        if (isHex != null && isHex == true)
            formattedFlag += " -hex";

        if (start.Length == 0 && end.Length == 0)
            return Exec("search", id.ToString(), pattern, formattedFlag);
        else
            return Exec("search", id.ToString(), pattern, start, end, formattedFlag);
    }

}

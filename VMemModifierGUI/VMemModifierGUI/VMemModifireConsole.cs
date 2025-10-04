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

    public static string ExecSearch(int id, string pattern, string flags)
    {
        string formattedFlag = "";
        switch (flags)
        {
            case "Utf-8 string" : formattedFlag = "-str -utf8"; break;
            case "Ascii string": formattedFlag = "-str -ascii"; break;
            case "Unicode string": formattedFlag = "-str -unicode"; break;
            case "Int": formattedFlag = "-int"; break;
        }
        return Exec("search", id.ToString(), pattern, formattedFlag);
    }

}

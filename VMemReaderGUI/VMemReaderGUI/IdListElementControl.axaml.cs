using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace VMemReaderGUI;

public partial class IdListElementControl : UserControl
{

    public Process? Process { get; protected set; }
    public IdListElementControl()
    {
        InitializeComponent();
    }

    public IdListElementControl(Process proc) : this()
    {
        this.Process = proc;
        this.Content = proc.ProcessName;
    }

    public new int GetHashCode()
    {
        return (Process is null) ? 0 : Process.GetHashCode();
    }
}
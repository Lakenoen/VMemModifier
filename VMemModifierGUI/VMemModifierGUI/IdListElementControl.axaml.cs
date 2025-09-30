using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;

namespace VMemReaderGUI;

public partial class IdListElementControl : UserControl
{

    public Process? Process { get; protected set; }
    public IdListElementControl()
    {
        InitializeComponent();
        WindowManager<IdListElementControl>.Instance.Value = this;
    }

    public IdListElementControl(Process proc) : this()
    {
        this.Process = proc;
        var textBoxes = InternalButton.GetControl<Grid>("ButtonGrid").Children;
        (textBoxes[0] as TextBlock)!.Text = proc.ProcessName;
        (textBoxes[1] as TextBlock)!.Text = proc.Id.ToString();
    }

    public void OnClick(object sender, RoutedEventArgs args)
    {
        if (Process != null)
            OnCallProcess?.Invoke(Process);
    }

    public new int GetHashCode()
    {
        return (Process is null) ? 0 : Process.GetHashCode();
    }

    public delegate void CallProcess(Process proc);
    public event CallProcess? OnCallProcess;
}
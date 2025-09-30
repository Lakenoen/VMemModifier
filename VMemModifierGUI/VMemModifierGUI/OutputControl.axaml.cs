using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VMemReaderGUI;

public partial class OutputControl : UserControl
{

    public ObservableCollection<TabItem> tabs { get; init; }
    private Dictionary<int, TabItem> map { get; init; } = new Dictionary<int, TabItem>();
    public Process? CurrentProcess { get; private set; } = null;
    public OutputControl()
    {
        InitializeComponent();
        tabs = new ObservableCollection<TabItem>();
        MainTab.ItemsSource = tabs;
        WindowManager<OutputControl>.Instance.Value = this;
    }

    public void TakeProcess(Process proc)
    {
        if (map.ContainsKey(proc.Id))
        {
            MainTab.SelectedItem = map[proc.Id];
            return;
        }
        var item = new TabItem
        {
            Header = proc.ProcessName + " (" + proc.Id + ")",
            Content = new TextBox()
        };
        map[proc.Id] = item;
        tabs.Add(item);
        CurrentProcess = proc;
    }

}
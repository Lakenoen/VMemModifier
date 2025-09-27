using System;
using System.Threading;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Frozen;
using Avalonia.Threading;

namespace VMemReaderGUI;

public partial class IdListControl : UserControl
{
    private readonly System.Timers.Timer checkProcessTimer = new System.Timers.Timer(5000);
    public IdListControl()
    {
        InitializeComponent();
        checkProcessTimer.Elapsed += TimerEvent;
        checkProcessTimer.AutoReset = true;
        checkProcessTimer.Start();
        AddProcesses();
    }

    public void AddIdElement(IdListElementControl elem)
    {
        Scroll.GetControl<StackPanel>("StackPanelContainer").Children.Add(elem);
    }

    public void RemoveIdElement(IdListElementControl elem)
    {
        Scroll.GetControl<StackPanel>("StackPanelContainer").Children.Remove(elem);
    }

    private void TimerEvent(object? source, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.Post(() => AddProcesses());
    }

    private List<Control> getIdElements()
    {
        return Scroll.GetControl<StackPanel>("StackPanelContainer").Children.ToList();
    }

    private void AddProcesses()
    {
        Process[] procArray = Process.GetProcesses();
        foreach( IdListElementControl elem in getIdElements())
        {
            if (!procArray.Contains(elem.Process))
                RemoveIdElement(elem);
        }
        foreach(Process proc in procArray )
        {
            if (getIdElements().Contains(new IdListElementControl(proc)))
                continue;
            AddIdElement(new IdListElementControl(proc));
        }
    }

}
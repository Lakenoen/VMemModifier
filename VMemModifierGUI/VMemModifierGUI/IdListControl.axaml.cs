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
using Avalonia.Input;
using VMemReaderCore;
using System.Text;

namespace VMemModifierGUI;

public partial class IdListControl : UserControl
{
    private readonly System.Timers.Timer checkProcessTimer = new System.Timers.Timer(1000 * 60);
    public OutputControl? outputControl { get; set; }
    public IdListControl()
    {
        InitializeComponent();

        checkProcessTimer.Elapsed += TimerEvent;
        checkProcessTimer.AutoReset = true;
        checkProcessTimer.Start();

        WindowManager<IdListControl>.Instance.Value = this;
    }

    private void FindKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter && e.Key != Key.Escape)
            return;

        var textBox = (TextBox)sender;
        string pattern = textBox.Text!.ToLower();

        if (e.Key == Key.Escape)
        {
            pattern = string.Empty;
            textBox.Text = string.Empty;
        }

        var elems = Scroll.GetControl<StackPanel>("StackPanelContainer").Children.ToArray();
        Array.ForEach(elems, (Control ctls) =>
        {
            if (ctls is not IdListElementControl el)
                return;

            if (el.Process is null)
            {
                el.IsVisible = false;
                return;
            } 

            if (pattern.Length == 0)
            {
                el.IsVisible = true;
                return;
            }

            string text = el.Process.ProcessName + el.Process.Id.ToString().ToLower();
            if (SearchAlgorithms.bmSearch(Encoding.UTF8.GetBytes(text).ToList(), Encoding.UTF8.GetBytes(pattern).ToList()).Count() > 0)
                el.IsVisible = true;
            else
                el.IsVisible = false;
        });

        e.Handled = true;
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
        Dispatcher.UIThread.Post(() => UpdateProcesses());
    }

    private List<Control> getIdElements()
    {
        return Scroll.GetControl<StackPanel>("StackPanelContainer").Children.ToList();
    }

    public void UpdateProcesses()
    {
        Process[] procArray = Process.GetProcesses();
        foreach( Control ctrl in getIdElements())
        {
            IdListElementControl? elem = ctrl as IdListElementControl;

            if (elem == null)
                continue;

            if (!procArray.Contains(elem.Process))
                RemoveIdElement(elem);
        }
        foreach(Process proc in procArray )
        {
            if (getIdElements().Contains(new IdListElementControl(proc)))
                continue;

            var newElem = new IdListElementControl(proc);
            if(outputControl != null)
                newElem.OnCallProcess += outputControl.TakeProcess;

            AddIdElement(newElem);
        }
    }

}
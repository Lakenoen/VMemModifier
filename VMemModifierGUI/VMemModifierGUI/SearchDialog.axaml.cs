using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VMemModifierGUI;
using static VMemReaderGUI.IdListElementControl;

namespace VMemReaderGUI;

public partial class SearchDialog : Window
{
    public SearchDialog()
    {
        InitializeComponent();
        WindowManager<SearchDialog>.Instance.Value = this;
    }

    public void OnSearch(object sender, RoutedEventArgs args)
    {
        int? procId = WindowManager<OutputControl>.Instance.Value?.CurrentProcess?.Id;
        if (procId == null)
        {
            new ErrorDialog("Process ID is not defined").ShowDialog(this);
            return;
        }

        TabItem? outputItem = WindowManager<OutputControl>.Instance.Value?.MainTab.SelectedItem as TabItem;
        if (outputItem == null)
        {
            new ErrorDialog("Not chosen the target process").ShowDialog(this);
            return;
        }

        ComboBoxItem? selectedItem = DataVariantCombo.SelectedItem as ComboBoxItem;
        string? flags = selectedItem?.Content as string;

        if (string.IsNullOrEmpty(PatternTextBox.Text) || flags is null)
        {
            new ErrorDialog("Not chosen flag or pattern").ShowDialog(this);
            return;
        }

        string response = VMemModifierConsole.ExecSearch(procId.Value, PatternTextBox.Text, flags);

        outputItem.Content = response;
        Close();
    }

    public void OnClose(object sender, RoutedEventArgs args)
    {
        Close();
    }

}
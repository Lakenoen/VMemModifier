using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using VMemModifierGUI;
using static VMemReaderGUI.IdListElementControl;

namespace VMemReaderGUI;

public partial class SearchDialog : Window
{
    public SearchDialog()
    {
        InitializeComponent();
        WindowManager<SearchDialog>.Instance.Value = this;
        DataVariantCombo.SelectionChanged += DataVariantCombo_SelectionChanged;
        checkHex();
    }

    private void DataVariantCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        checkHex();
    }

    private void checkHex()
    {
        CheckBox? isHexCheckBox = this.GetLogicalDescendants().OfType<CheckBox>().ElementAt(1);
        ComboBoxItem? selectedItem = DataVariantCombo.SelectedItem as ComboBoxItem;
        string? flags = selectedItem?.Content as string;
        if (flags.Contains("string") || flags.Contains("bin"))
            isHexCheckBox.IsEnabled = false;
        else
            isHexCheckBox.IsEnabled = true;
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

        (outputItem.Content as TextBox)!.Text = "Processing...";
        CheckBox? isRegcheckBox = this.GetLogicalDescendants().OfType<CheckBox>().First();
        CheckBox? isHexCheckBox = this.GetLogicalDescendants().OfType<CheckBox>().ElementAt(1);

        int id = procId.Value;
        string text = PatternTextBox.Text;
        string start = (StartTextBox?.Text is null) ? string.Empty : StartTextBox.Text;
        string end = (EndTextBox?.Text is null) ? string.Empty : EndTextBox.Text;
        bool? isReg = isRegcheckBox?.IsChecked;
        bool? isHex = isHexCheckBox?.IsChecked;

        Task.Run(() =>
        {
            string response = VMemModifierConsole.ExecSearch(id, text, start, end, flags, isReg, isHex);
            Dispatcher.UIThread.Post(() =>
            {
                (outputItem.Content as TextBox)!.Text = response;
            }, DispatcherPriority.Background);
        });

        Close();
    }
    public void OnClose(object sender, RoutedEventArgs args)
    {
        Close();
    }

}
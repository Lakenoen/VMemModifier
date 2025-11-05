using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using VMemModifierGUI;
using static System.Net.Mime.MediaTypeNames;

namespace VMemModifierGUI;

public partial class ReadDialog : Window, ICommandDialog
{
    public ReadDialog()
    {
        InitializeComponent();
        WindowManager<ReadDialog>.Instance.Value = this;
        DataVariantCombo.SelectionChanged += DataVariantCombo_SelectionChanged;
        checkHex();
    }

    private void DataVariantCombo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        checkHex();
    }

    private void checkHex()
    {
        CheckBox? isHexCheckBox = this.GetLogicalDescendants().OfType<CheckBox>().ElementAt(0);
        ComboBoxItem? selectedItem = DataVariantCombo.SelectedItem as ComboBoxItem;
        string? flags = selectedItem?.Content as string;
        if (flags.Contains("string") || flags.Contains("bin"))
            isHexCheckBox.IsEnabled = false;
        else
            isHexCheckBox.IsEnabled = true;
    }
    public void OnClick(object sender, RoutedEventArgs args)
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

        if (string.IsNullOrEmpty(AddressTextBox.Text) || flags is null)
        {
            new ErrorDialog("Not chosen address or size").ShowDialog(this);
            return;
        }

        (outputItem.Content as TextBox)!.Text = "Processing...";
        CheckBox? isHexCheckBox = this.GetLogicalDescendants().OfType<CheckBox>().First();

        int id = procId.Value;
        string address = (AddressTextBox?.Text is null) ? string.Empty : AddressTextBox.Text;
        string size = (SizeTextBox?.Text is null) ? string.Empty : SizeTextBox.Text;
        bool? isHex = isHexCheckBox?.IsChecked;

        Task.Run(() =>
        {
            string response = VMemModifierConsole.ExecRead(id, address, size, flags, isHex);
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
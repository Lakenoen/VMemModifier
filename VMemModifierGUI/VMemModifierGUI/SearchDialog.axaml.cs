using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
        //TODO
        int? procId = WindowManager<OutputControl>.Instance.Value?.CurrentProcess?.Id;
        if (procId == null)
            return;
        ComboBoxItem? selectedItem = DataVariantCombo.SelectedItem as ComboBoxItem;
        string response = VMemModifierConsole.ExecSearch(procId.Value, PatternTextBox.Text, selectedItem.Content as string);
        (WindowManager<OutputControl>.Instance.Value?.MainTab.SelectedItem as TabItem).Content = response;
    }
}
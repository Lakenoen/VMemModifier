using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tmds.DBus.Protocol;
using VMemModifierGUI;

namespace VMemReaderGUI;

public partial class ToolBar : UserControl
{
    public ToolBar()
    {
        InitializeComponent();
        WindowManager<ToolBar>.Instance.Value = this;
    }

    public void ClickHandler(object sender, RoutedEventArgs args)
    {
        if (WindowManager<SearchDialog>.Instance.Value is not null)
            WindowManager<SearchDialog>.Instance.Value.Close();
        if (WindowManager<OutputControl>.Instance.Value?.CurrentProcess is null)
        {
            new ErrorDialog("The target process is not chosen, please choose the target process").ShowDialog(WindowManager<MainWindow>.Instance.Value!);
            return;
        }
        SearchDialog dialog = new SearchDialog();
        dialog.ShowDialog(WindowManager<MainWindow>.Instance.Value!);
    }

}
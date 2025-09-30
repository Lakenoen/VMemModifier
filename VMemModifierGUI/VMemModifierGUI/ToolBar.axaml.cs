using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Tmds.DBus.Protocol;

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
        SearchDialog dialog = new SearchDialog();
        dialog.Show();
    }

}
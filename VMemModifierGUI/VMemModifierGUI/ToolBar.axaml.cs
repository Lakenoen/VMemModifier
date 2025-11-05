using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Tmds.DBus.Protocol;
using VMemModifierGUI;

namespace VMemModifierGUI;

public partial class ToolBar : UserControl
{
    public ToolBar()
    {
        InitializeComponent();
        WindowManager<ToolBar>.Instance.Value = this;
    }

    public void ClickHandler(object sender, RoutedEventArgs args)
    {
        Button button = sender as Button;

        if (button!.Content!.Equals("Close"))
        {
            Process? target = null;
            if (CloseIdBox.Text is not null)
            {
                try
                {
                    target = Process.GetProcessById(int.Parse(CloseIdBox.Text));
                }
                catch (Exception)
                {
                    target?.Kill();
                }
            }
            return;
        }
        
        if (button.Content.Equals("Create"))
        {
            var topLevel = TopLevel.GetTopLevel(this);
            var file = topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false
            }).Result.First();
            string path = file.Path.AbsolutePath;
            Process proc = Process.Start(path);
            return;
        }

        if (WindowManager<OutputControl>.Instance.Value?.CurrentProcess is null)
        {
            new ErrorDialog("The target process is not chosen, please choose the target process").ShowDialog(WindowManager<MainWindow>.Instance.Value!);
            return;
        }

        if (button.Content.Equals("Search"))
        {
            if (WindowManager<SearchDialog>.Instance.Value is not null)
                WindowManager<SearchDialog>.Instance.Value.Close();
            SearchDialog dialog = new SearchDialog();
            dialog.ShowDialog(WindowManager<MainWindow>.Instance.Value!);
        }
        else if (button.Content.Equals("Read"))
        {
            if (WindowManager<ReadDialog>.Instance.Value is not null)
                WindowManager<ReadDialog>.Instance.Value.Close();
            ReadDialog dialog = new ReadDialog();
            dialog.ShowDialog(WindowManager<MainWindow>.Instance.Value!);
        }
        else if (button.Content.Equals("Write"))
        {
            if (WindowManager<WriteDialog>.Instance.Value is not null)
                WindowManager<WriteDialog>.Instance.Value.Close();
            WriteDialog dialog = new WriteDialog();
            dialog.ShowDialog(WindowManager<MainWindow>.Instance.Value!);
        }
        else if (button.Content.Equals("Inject"))
        {
            TabItem? outputItem = WindowManager<OutputControl>.Instance.Value?.MainTab.SelectedItem as TabItem;
            if (outputItem == null)
            {
                new ErrorDialog("Not chosen the target process").ShowDialog(WindowManager<MainWindow>.Instance.Value!);
                return;
            }
            var topLevel = TopLevel.GetTopLevel(this);
            var file = topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Text File",
                AllowMultiple = false
            }).Result.First();
            string pathToDll = file.Path.AbsolutePath;
            (outputItem.Content as TextBox)!.Text = VMemModifierConsole.Exec(
                    "inject",
                    WindowManager<OutputControl>.Instance.Value?.CurrentProcess.ToString()!,
                    $"\"{pathToDll}\""
                    );
        }
        
    }

}
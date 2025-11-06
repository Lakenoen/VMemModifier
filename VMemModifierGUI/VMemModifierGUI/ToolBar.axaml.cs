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

    private static FilePickerFileType DLL_TYPE = new FilePickerFileType("Dll")
    {
        Patterns = new[] { "*.dll" },
        AppleUniformTypeIdentifiers = new[] { "com.microsoft.windows-dynamic-link-library" },
        MimeTypes = new[] { "application/x-ms-ne-executable" }
    };
    private static FilePickerFileType EXE_TYPE = new FilePickerFileType("Exe")
    {
        Patterns = new[] { "*.exe" },
        AppleUniformTypeIdentifiers = new[] { "com.microsoft.windows-executable" },
        MimeTypes = new[] { "application/x-dosexec" }
    };
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
            var files = topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select executable file",
                AllowMultiple = false,
                FileTypeFilter = new[] { EXE_TYPE }
            }).Result;
            if (files.Count == 0)
                return;
            string path = files.First().Path.AbsolutePath;
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
            if(TopLevel.GetTopLevel(this) is Window topLevel){
                var files = topLevel!.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    Title = "Select Dll",
                    AllowMultiple = false,
                    FileTypeFilter = new[] { DLL_TYPE }
                }).Result;

                if (files.Count == 0)
                    return;

                string pathToDll = files.First().Path.AbsolutePath;
                (outputItem.Content as TextBox)!.Text = VMemModifierConsole.Exec(
                        "inject",
                        WindowManager<OutputControl>.Instance.Value?.CurrentProcess.Id.ToString()!,
                        $"\"{pathToDll}\""
                        );
            }
        }
        
    }

}
using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace VMemModifierGUI;

public partial class ErrorDialog : Window
{
    public ErrorDialog(string text)
    {
        InitializeComponent();
        TextBlock.Text = text;
    }

    private void OnOkClick(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
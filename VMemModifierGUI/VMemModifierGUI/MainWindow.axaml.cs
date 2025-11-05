using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.Collections.Generic;
using Avalonia.VisualTree;
using Microsoft.VisualBasic;

namespace VMemModifierGUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowManager<MainWindow>.Instance.Value = this;
            init();
        }

        private void init()
        {
            IdControl.outputControl = OutputControl;
            IdControl.UpdateProcesses();
        }
        private void Exit(object? sender, RoutedEventArgs e)
        {
            var a  =this.Width;
            this.Close();
        }

        private void MenuHandler(object? sender, RoutedEventArgs e)
        {
            ToolBar? toolBar = WindowManager<ToolBar>.Instance.Value;
            if (toolBar == null  || sender is null)
                throw new ApplicationException("ToolBar or menu item is null, something went wrong");

            MenuItem item = (MenuItem)sender;
            List<Button> buttons = toolBar.GetLogicalDescendants().OfType<Button>().ToList();
            var button = item.Header switch
            {
                "_Search" => buttons.Find(b => b.Content == "Search"),
                "_Read" => buttons.Find(b => b.Content == "Read"),
                "_Inject" => buttons.Find(b => b.Content == "Inject"),
                "_Create" => buttons.Find(b => b.Content == "Create"),
                "_Close" => buttons.Find(b => b.Content == "Close"),
                "_Write" => buttons.Find(b => b.Content == "Write"),
                _ => null
            };

            if (button == null)
                throw new ApplicationException("ToolBar or menu item is null, something went wrong");

            toolBar.ClickHandler(button, e);
        }

    }
}
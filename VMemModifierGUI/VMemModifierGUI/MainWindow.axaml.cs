using Avalonia.Controls;

namespace VMemReaderGUI
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
    }
}
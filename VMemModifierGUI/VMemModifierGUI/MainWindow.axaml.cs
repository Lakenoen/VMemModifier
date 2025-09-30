using Avalonia.Controls;

namespace VMemReaderGUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            IdControl.outputControl = OutputControl;
        }
    }
}
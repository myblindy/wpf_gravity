using gravity.ViewModels;
using System.Windows;

namespace gravity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(Width, Height);
        }
    }
}

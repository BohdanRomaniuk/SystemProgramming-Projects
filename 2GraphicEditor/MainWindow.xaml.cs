using GraphicEditor.ViewModels;
using System.Windows;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}

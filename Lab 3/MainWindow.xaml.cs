using Lab_3.Model;
using Lab_3.ViewModel;
using System.Windows;

namespace Lab_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowViewModel();
            await viewModel.InitializeAsync(); 
            DataContext = viewModel;
            menuView.MainWindow = this;
        }
    }
}
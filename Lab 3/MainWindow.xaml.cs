using Lab_3.Command;
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

            viewModel.ShowDialogRequested += OnShowDialogRequested;
            viewModel.ChangeWindowRequested += OnChangeWindowRequested;
            viewModel.RequestExit += () => Application.Current.Shutdown();
        }

        private void OnShowDialogRequested(string className)
        {
            if (string.IsNullOrEmpty(className))
            {
                return;
            }

            Type windowType = Type.GetType("Lab_3.Dialogs." + className);

            if (windowType != null)
            {
                Window newWindow = (Window)Activator.CreateInstance(windowType);
                newWindow.Show();
            }
        }

        private void OnChangeWindowRequested()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }
    }
}
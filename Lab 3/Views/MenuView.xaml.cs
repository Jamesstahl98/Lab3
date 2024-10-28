using Lab_3.Command;
using Lab_3.Dialogs;
using Lab_3.ViewModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Lab_3.Views
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MainWindow? MainWindow { get; set; }

        public MenuView()
        {
            InitializeComponent();
        }

        private void GoToPlayerView(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.configuratorView.Visibility = System.Windows.Visibility.Collapsed;
            MainWindow.playerView.Visibility = System.Windows.Visibility.Visible;
        }

        private void GoToConfiguratorView(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.configuratorView.Visibility = System.Windows.Visibility.Visible;
            MainWindow.playerView.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void OpenPackOptions(object sender, RoutedEventArgs e)
        {
            var packOptionsDialog = new PackOptionsDialog();
            packOptionsDialog.ShowDialog();
        }
    }
}

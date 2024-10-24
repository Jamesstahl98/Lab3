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

        private void OpenCreateNewPackDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateNewPackDialog dialog = new CreateNewPackDialog();
            bool? dialogResult = dialog.ShowDialog();

            if (dialogResult == true)
            {
                (MainWindow.DataContext as MainWindowViewModel).Packs.Add(dialog.DataContext as QuestionPackViewModel);
            }
        }

        private void OpenPackOptionsDialog(object sender, System.Windows.RoutedEventArgs e)
        {
            PackOptionsDialog dialog = new PackOptionsDialog();
            bool? dialogResult = dialog.ShowDialog();

            if(dialogResult == true)
            {
                
            }
        }
    }
}

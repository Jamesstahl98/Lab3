using Lab_3.Dialogs;
using System.Windows;
using System.Windows.Controls;

namespace Lab_3.Views
{
    /// <summary>
    /// Interaction logic for ConfiguratorView.xaml
    /// </summary>
    public partial class ConfiguratorView : UserControl
    {
        public ConfiguratorView()
        {
            InitializeComponent();
        }

        private void OpenPackOptions(object sender, RoutedEventArgs e)
        {
            var packOptionsDialog = new PackOptionsDialog();
            packOptionsDialog.ShowDialog();
        }
    }
}

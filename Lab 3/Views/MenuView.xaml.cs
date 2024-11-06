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
    }
}

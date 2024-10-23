using Lab_3.Model;
using Lab_3.ViewModel;
using System.Windows;

namespace Lab_3.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            var pack = new QuestionPackViewModel(new QuestionPack("Question Pack"));
        }
    }
}
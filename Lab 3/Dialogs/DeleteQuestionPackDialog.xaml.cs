using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab_3.Dialogs
{
    /// <summary>
    /// Interaction logic for DeleteQuestionPackDialog.xaml
    /// </summary>
    public partial class DeleteQuestionPackDialog : Window
    {
        public DeleteQuestionPackDialog()
        {
            InitializeComponent();
            DataContext = (App.Current.MainWindow as MainWindow).DataContext;
        }

        public void CloseDialog(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

﻿using Lab_3.ViewModel;
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
    /// Interaction logic for ImportQuestionsTopack.xaml
    /// </summary>
    public partial class ImportQuestionsToPack : Window
    {
        public ImportQuestionsToPack()
        {
            InitializeComponent();
            DataContext = new QuestionImporterViewModel();
        }
    }
}
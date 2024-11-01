using Lab_3.Command;
using Lab_3.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows;

namespace Lab_3.ViewModel
{
    internal class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack model;
        private MainWindowViewModel? mainWindowViewModel;

        public string Name
        { 
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
        public DelegateCommand AddQuestionPackCommand { get; }
        public QuestionPackViewModel()
        {
            model = new QuestionPack();
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindowViewModel = (MainWindowViewModel)(App.Current.MainWindow as MainWindow).DataContext;
            });

            AddQuestionPackCommand = new DelegateCommand(AddQuestionPack);
        }
        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            Questions = new ObservableCollection<Question>(model.Questions);
            mainWindowViewModel = (MainWindowViewModel)(App.Current.MainWindow as MainWindow).DataContext;

            AddQuestionPackCommand = new DelegateCommand(AddQuestionPack);
        }

        private void AddQuestionPack(object obj)
        {
            mainWindowViewModel.Packs.Add(this);
            mainWindowViewModel.ActivePack = this;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

using Lab_3.Command;
using Lab_3.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Lab_3.ViewModel
{
    internal class QuestionPackViewModel : ViewModelBase
    {
        private readonly QuestionPack model;
        private readonly MainWindowViewModel? mainWindowViewModel;

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
        public ObservableCollection<Question> Questions { get; }
        public DelegateCommand AddQuestionPackCommand { get; }

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

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

        public string Name
        { 
            get => model.Name;
            set
            {
                model.Name = value;
                RaisePropertyChanged();
                Debug.WriteLine(Name);
            }
        }
        public Difficulty Difficulty
        {
            get => model.Difficulty;
            set
            {
                model.Difficulty = value;
                RaisePropertyChanged();
                Debug.WriteLine(Difficulty);
            }
        }
        public int TimeLimitInSeconds
        {
            get => model.TimeLimitInSeconds;
            set
            {
                model.TimeLimitInSeconds = value;
                RaisePropertyChanged();
                Debug.WriteLine(TimeLimitInSeconds);
            }
        }
        public ObservableCollection<Question> Questions { get; }
        public DelegateCommand AddQuestionPackCommand { get; }

        public QuestionPackViewModel(QuestionPack model)
        {
            this.model = model;
            Questions = new ObservableCollection<Question>(model.Questions);

            AddQuestionPackCommand = new DelegateCommand(AddQuestionPack);
        }

        private void AddQuestionPack(object obj)
        {
            
        }
    }
}

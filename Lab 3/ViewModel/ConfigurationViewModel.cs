using Lab_3.Command;
using Lab_3.Dialogs;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private Question? _activeQuestion;

        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public DelegateCommand AddQuestionPackCommand { get; }
        public DelegateCommand ChangeActivePackCommand { get; }
        public QuestionPackViewModel? ActivePack { get => mainWindowViewModel.ActivePack; }
        public Question? ActiveQuestion {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion);
            AddQuestionPackCommand = new DelegateCommand(AddQuestionPack);
            ChangeActivePackCommand = new DelegateCommand(ChangeActivePack);
        }

        private void ChangeActivePack(object obj)
        {
            throw new NotImplementedException();
        }

        private void AddQuestionPack(object obj)
        {
            //Figure out MVVM way to do this
            var newPack = new CreateNewPackDialog();
            bool? result = newPack.ShowDialog();
            if(result == true)
            {
                mainWindowViewModel.Packs.Add(newPack.DataContext as QuestionPackViewModel);
                mainWindowViewModel.ActivePack = mainWindowViewModel.Packs[mainWindowViewModel.Packs.Count-1];
            }
        }

        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
        }

        private void AddQuestion(object obj)
        {
            //Should this somehow be moved up to QuestionPack class
            ActivePack.Questions.Add(new Model.Question());
        }
    }
}

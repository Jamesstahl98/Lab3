using Lab_3.Command;
using Lab_3.Dialogs;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public QuestionPackViewModel? ActivePack{ get => mainWindowViewModel.ActivePack; }
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

            this.mainWindowViewModel.PropertyChanged += MainWindowViewModel_PropertyChanged;
        }

        private void MainWindowViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainWindowViewModel.ActivePack))
            {
                RaisePropertyChanged(nameof(ActivePack));
            }
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
            var newQuestion = new Model.Question();
            ActivePack.Questions.Add(newQuestion);
            ActiveQuestion = newQuestion;
        }
    }
}

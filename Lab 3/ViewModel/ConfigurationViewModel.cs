﻿using Lab_3.Command;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Lab_3.ViewModel
{
    internal class ConfigurationViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private Question? _activeQuestion;

        public DelegateCommand AddQuestionCommand { get; }
        public DelegateCommand RemoveQuestionCommand { get; }
        public QuestionPackViewModel? ActivePack{ get => mainWindowViewModel.ActivePack; }
        public Question? ActiveQuestion {
            get => _activeQuestion;
            set
            {
                _activeQuestion = value;
                RaisePropertyChanged();
                RemoveQuestionCommand.RaiseCanExecuteChanged();
                Task.Run(async () => await mainWindowViewModel.JsonQuestionPackHandler.SaveQuestionPacksToJsonAsync(mainWindowViewModel.Packs));
            }
        }
        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            AddQuestionCommand = new DelegateCommand(AddQuestion);
            RemoveQuestionCommand = new DelegateCommand(RemoveQuestion, canRemove => ActiveQuestion != null);
        }

        private void RemoveQuestion(object obj)
        {
            ActivePack.Questions.Remove(ActiveQuestion);
            mainWindowViewModel.StartQuizCommand.RaiseCanExecuteChanged();
        }

        private void AddQuestion(object obj)
        {
            if(ActivePack == null) { return; }
            var newQuestion = new Question("New Question");
            ActivePack.Questions.Add(newQuestion);
            ActiveQuestion = newQuestion;
            mainWindowViewModel.StartQuizCommand.RaiseCanExecuteChanged();
        }
    }
}

﻿using Lab_3.Command;
using Lab_3.Dialogs;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private QuestionPackViewModel? _activePack;
        private bool _configurationViewModelActive;
        private bool _playerViewModelActive;

        public QuestionPackViewModel? ActivePack
		{
            get => _activePack;
			set
            { 
                _activePack = value;
                RaisePropertyChanged();
                RemoveActivePackCommand.RaiseCanExecuteChanged();
                GoToPlayerViewCommand.RaiseCanExecuteChanged();
            }
		}
        public bool ConfigurationViewModelActive
        { 
            get => _configurationViewModelActive;
            set
            {
                _configurationViewModelActive = value;
                RaisePropertyChanged();
            }
        }
        public bool PlayerViewModelActive
        {
            get => _playerViewModelActive;
            set
            {
                _playerViewModelActive = value;
                ConfigurationViewModelActive = !_playerViewModelActive;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<QuestionPackViewModel> Packs{ get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public DelegateCommand ChangeActivePackCommand { get; }
        public DelegateCommand RemoveActivePackCommand { get; }
        public DelegateCommand GoToPlayerViewCommand { get; }
        public DelegateCommand GoToConfigurationViewCommand { get; }

        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            PlayerViewModelActive = false;

            ChangeActivePackCommand = new DelegateCommand(ChangeActivePack);
            RemoveActivePackCommand = new DelegateCommand(RemoveActivePack, canRemove => Packs.Count > 1);
            GoToPlayerViewCommand = new DelegateCommand(GoToPlayerView, canRemove => ActivePack.Questions.Count > 0);
            GoToConfigurationViewCommand = new DelegateCommand(GoToConfigurationView);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Question Pack"));
            Packs = new ObservableCollection<QuestionPackViewModel>();
            Packs.Add(ActivePack);
        }

        private void GoToConfigurationView(object obj)
        {
            PlayerViewModelActive = false;
        }

        private void GoToPlayerView(object obj)
        {
            PlayerViewModel.StartQuiz(ActivePack);
            PlayerViewModelActive = true;
        }

        private void RemoveActivePack(object obj)
        {
            Packs.Remove(ActivePack);
            ActivePack = Packs.FirstOrDefault();
        }

        private void ChangeActivePack(object obj)
        {
            ActivePack = (QuestionPackViewModel)obj;
        }
    }
}

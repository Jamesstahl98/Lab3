﻿using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private QuestionPackViewModel? _activePack;
		public QuestionPackViewModel? ActivePack
		{
            get => _activePack;
			set
            { 
                _activePack = value;
                RaisePropertyChanged();
            }
		}
        public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; } 

        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Question Pack"));
            Packs = new ObservableCollection<QuestionPackViewModel>();
            Packs.Add(ActivePack);
        }
    }
}

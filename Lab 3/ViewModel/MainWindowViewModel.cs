using Lab_3.Command;
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
		public QuestionPackViewModel? ActivePack
		{
            get => _activePack;
			set
            { 
                _activePack = value;
                RaisePropertyChanged();
                RemoveActivePackCommand.RaiseCanExecuteChanged();
            }
		}
        public ObservableCollection<QuestionPackViewModel> Packs{ get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; }
        public DelegateCommand ChangeActivePackCommand { get; }
        public DelegateCommand RemoveActivePackCommand { get; }
        public DelegateCommand GoToPlayerViewCommand { get; }

        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            ConfigurationViewModel = new ConfigurationViewModel(this);

            ChangeActivePackCommand = new DelegateCommand(ChangeActivePack);
            RemoveActivePackCommand = new DelegateCommand(RemoveActivePack, canRemove => Packs.Count > 1);
            GoToPlayerViewCommand = new DelegateCommand(GoToPlayerView);

            ActivePack = new QuestionPackViewModel(new QuestionPack("Question Pack"));
            Packs = new ObservableCollection<QuestionPackViewModel>();
            Packs.Add(ActivePack);
        }

        private void GoToPlayerView(object obj)
        {
            PlayerViewModel.StartQuiz(ActivePack);
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

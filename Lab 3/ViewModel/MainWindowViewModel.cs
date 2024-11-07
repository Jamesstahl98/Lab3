using Lab_3.Command;
using Lab_3.Dialogs;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lab_3.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private QuestionPackViewModel? _activePack;
        private bool _configurationViewModelActive;
        private bool _playerViewModelActive;
        private bool _canRemoveQuestionPack;

        public bool CanRemoveQuestionPack
        {
            get => _canRemoveQuestionPack;
            set
            {
                _canRemoveQuestionPack = value;
                RaisePropertyChanged();
            }
        }

        public QuestionPackViewModel? ActivePack
		{
            get => _activePack;
			set
            { 
                _activePack = value;
                RaisePropertyChanged();
                ConfigurationViewModel?.RaisePropertyChanged();
                UpdateCommandStates();

                if (Packs.Count > 1 )
                {
                    CanRemoveQuestionPack = true;
                }
                else
                {
                    CanRemoveQuestionPack = false;
                }
                Task.Run(async () => await JsonQuestionPackHandler.SaveQuestionPacksToJsonAsync(Packs));
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
        public JsonQuestionPackHandler JsonQuestionPackHandler { get; }
        public ObservableCollection<QuestionPackViewModel> Packs{ get; set; }
        public PlayerViewModel PlayerViewModel { get; }
        public ConfigurationViewModel ConfigurationViewModel { get; private set; }
        public DelegateCommand ChangeActivePackCommand { get; private set; }
        public DelegateCommand RemoveActivePackCommand { get; private set; }
        public DelegateCommand StartQuizCommand { get; private set; }
        public DelegateCommand StartConfigurationCommand { get; private set; }
        public DelegateCommand CreateNewDialogCommand { get; private set; }
        public DelegateCommand ChangeWindowStateCommand { get; private set; }
        public DelegateCommand ExitProgramRequestCommand { get; private set; }

        public event Action ChangeWindowRequested;
        public event Action RequestExit;
        public DelegateCommand ShowDialogCommand { get; private set; }
        public event Action<string> ShowDialogRequested;

        public MainWindowViewModel()
        {
            PlayerViewModel = new PlayerViewModel(this);
            JsonQuestionPackHandler = new JsonQuestionPackHandler(this);

            InitializeCommands();
            PlayerViewModelActive = false;
        }

        private void InitializeCommands()
        {
            ChangeWindowStateCommand = new DelegateCommand(ChangeWindowState);
            ExitProgramRequestCommand = new DelegateCommand(ExitProgramRequest);
            ShowDialogCommand = new DelegateCommand(CreateNewDialog);
            ConfigurationViewModel = new ConfigurationViewModel(this);
            ChangeActivePackCommand = new DelegateCommand(ChangeActivePack);
            RemoveActivePackCommand = new DelegateCommand(RemoveActivePack, canRemove => Packs.Count > 1);
            StartQuizCommand = new DelegateCommand(StartQuiz, canPlay => ActivePack.Questions.Count > 0);
            StartConfigurationCommand = new DelegateCommand(StartConfiguration);
            CreateNewDialogCommand = new DelegateCommand(CreateNewDialog);
        }

        private void ExitProgramRequest(object obj) => RequestExit?.Invoke();
        private void ChangeWindowState(object obj) => ChangeWindowRequested?.Invoke();

        private void UpdateCommandStates()
        {
            RemoveActivePackCommand.RaiseCanExecuteChanged();
            StartQuizCommand.RaiseCanExecuteChanged();
        }

        public async Task InitializeAsync()
        {

            Packs = new ObservableCollection<QuestionPackViewModel>();
            var packs = await JsonQuestionPackHandler.LoadQuestionPacksAsync();
            
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Packs.Clear();
                foreach (var pack in packs)
                {
                    Packs.Add(pack);
                }
            
                if (Packs.Count < 1)
                {
                    Packs.Add(new QuestionPackViewModel(new QuestionPack()));
                }
            
                ActivePack = Packs.FirstOrDefault();
            });
        }

        private void StartConfiguration(object obj)
        {
            PlayerViewModelActive = false;
        }

        private void StartQuiz(object obj)
        {
            PlayerViewModel.StartQuiz(ActivePack);
            PlayerViewModelActive = true;
        }

        private void RemoveActivePack(object obj)
        {
            Packs.Remove(ActivePack);
            ActivePack = Packs.FirstOrDefault();
            Task.Run(async () => await JsonQuestionPackHandler.SaveQuestionPacksToJsonAsync(Packs));
        }

        private void ChangeActivePack(object obj)
        {
            ActivePack = (QuestionPackViewModel)obj;
        }

        private void CreateNewDialog(object obj)
        {
            string className = obj as string;
            ShowDialogRequested?.Invoke(className);
        }
    }
}

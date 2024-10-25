using Lab_3.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Lab_3.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private int _timeLeft;

        public int TimeLeft
        {
            get => _timeLeft;
            private set
            {
                _timeLeft = value;
                if(_timeLeft <= 0)
                {
                    TimeIsUp();
                }
                RaisePropertyChanged();
            }
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public void StartQuiz(QuestionPackViewModel questionPackViewModel)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }

        private void TimeIsUp()
        {

        }
    }
}

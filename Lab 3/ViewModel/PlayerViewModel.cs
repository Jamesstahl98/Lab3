using Lab_3.Command;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Lab_3.ViewModel
{
    internal class PlayerViewModel : ViewModelBase
    {
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private int questionCount = 0;
        private int _timeLeft;
        private string[] _answers;

        public DelegateCommand PlayerGuessCommand { get; private set; }
        public QuestionPackViewModel ActivePack { get; set; }
        public Question ActiveQuestion { get; private set; }
        public string[] Answers
        {
            get => _answers;
            private set
            {
                _answers = value;
                RaisePropertyChanged();
            }
        }

        public int TimeLeft
        {
            get => _timeLeft;
            private set
            {
                _timeLeft = value;
                RaisePropertyChanged();
                if (_timeLeft <= 0)
                {
                    StopTimer();
                    RevealCorrectAnswer(null);
                }
            }
        }

        public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            PlayerGuessCommand = new DelegateCommand(PlayerGuess);
        }

        public void StartQuiz(QuestionPackViewModel questionPackViewModel)
        {
            ActivePack = questionPackViewModel;
            ActiveQuestion = ActivePack.Questions[0];
            StartQuestion(ActiveQuestion);
        }

        private void StartQuestion(Question question)
        {
            Answers = new string[4];
            Answers[0] = question.CorrectAnswer;
            for (int i = 0; i < question.IncorrectAnswers.Length; i++)
            {
                Answers[i+1] = question.IncorrectAnswers[i];
            }
            Answers = RandomizeArray(Answers);

            TimeLeft = ActivePack.TimeLimitInSeconds;
            timer.Start();
        }

        private string[] RandomizeArray(string[] array)
        {
            int count = array.Length;
            while (count > 1)
            {
                int i = Random.Shared.Next(count--);
                (array[i], array[count]) = (array[count], array[i]);
            }
            return array;
        }

        private void PlayerGuess(object obj)
        {
            StopTimer();
            Debug.WriteLine(obj.GetType());
            RevealCorrectAnswer(obj.ToString());
        }

        private void RevealCorrectAnswer(string? playerGuess)
        {
            if(playerGuess == null)
            {
                Debug.WriteLine("Wrong");
            }
            else if (ActiveQuestion.CorrectAnswer == playerGuess)
            {
                Debug.WriteLine("Correct");
            }
            else
            {
                Debug.WriteLine("Wrong");
            }

            questionCount++;
            if (ActivePack.Questions.Count <= questionCount)
            {
                StopQuiz();
                return;
            }
            ActiveQuestion = ActivePack.Questions[questionCount];
            StartQuestion(ActiveQuestion);
        }

        private void StopQuiz()
        {

        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }

        private void StopTimer()
        {
            timer.Stop();
        }
    }
}

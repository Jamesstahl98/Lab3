using Lab_3.Command;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int _timeLeft;
        private string[] _answers;
        private Question _activeQuestion;
        private int _questionCount;
        private bool _quizOngoing;
        private int _correctGuesses;

        public ObservableCollection<bool?> RevealedAnswers { get; private set; }
        public DelegateCommand PlayerGuessCommand { get; private set; }
        public bool QuizOngoing 
        {
            get => _quizOngoing;
            private set
            {
                _quizOngoing = value;
                RaisePropertyChanged();
            }
        }
        public Question ActiveQuestion
        {
            get => _activeQuestion;
            private set
            { 
                _activeQuestion = value;
                RaisePropertyChanged();
            }
        }
        public int QuestionCount
        {
            get => _questionCount;
            private set
            {
                _questionCount = value;
                RaisePropertyChanged();
            }
        }
        public int CorrectGuesses
        {
            get => _correctGuesses;
            private set
            {
                _correctGuesses = value;
                RaisePropertyChanged();
            }
        }
        public QuestionPackViewModel ActivePack { get; set; }
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
            RevealedAnswers = new ObservableCollection<bool?>();
            for (int i = 0; i < 4; i++)
            {
                RevealedAnswers.Add(null);
            }

            this.mainWindowViewModel = mainWindowViewModel;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            PlayerGuessCommand = new DelegateCommand(PlayerGuess);
        }

        public void StartQuiz(QuestionPackViewModel questionPackViewModel)
        {
            QuizOngoing = true;
            ActivePack = questionPackViewModel;
            ActiveQuestion = ActivePack.Questions[0];
            QuestionCount = 0;
            CorrectGuesses = 0;

            StartQuestion(ActiveQuestion);
        }

        private void StartQuestion(Question question)
        {
            QuestionCount++;
            for (int i = 0; i < RevealedAnswers.Count; i++)
            {
                RevealedAnswers[i] = null;
            }

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

            RevealCorrectAnswer(obj.ToString());
        }

        private async void RevealCorrectAnswer(string? playerGuess)
        {
            if(playerGuess == ActiveQuestion.CorrectAnswer)
            {
                CorrectGuesses++;
            }
            for (int i = 0; i < Answers.Length; i++)
            {
                if (Answers[i] == ActiveQuestion.CorrectAnswer)
                {
                    RevealedAnswers[i] = true;
                }
                if(playerGuess == Answers[i] && playerGuess != ActiveQuestion.CorrectAnswer)
                {
                    RevealedAnswers[i] = false;
                }
            }

            await WaitMilliseconds(2000);

            if (ActivePack.Questions.Count <= _questionCount)
            {
                StopQuiz();
                return;
            }
            ActiveQuestion = ActivePack.Questions[_questionCount];
            StartQuestion(ActiveQuestion);
        }

        private void StopQuiz()
        {
            QuizOngoing = false;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }

        private void StopTimer()
        {
            timer.Stop();
        }

        private static async Task WaitMilliseconds(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
    }
}
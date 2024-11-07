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
        private const int revealDelayMS = 2000;
        private const int answersOptionCount = 4;

        private Question[] questions;
        private readonly MainWindowViewModel? mainWindowViewModel;
        private DispatcherTimer timer;
        private int _timeLeft;
        private string[] _answers;
        private Question _activeQuestion;
        private int _questionCount;
        private bool _quizOngoing;
        private int _correctGuesses;
        private int _questionPackQuestionCount;

        public ObservableCollection<bool?> RevealedAnswers { get; private set; }
        public DelegateCommand PlayerGuessCommand { get; private set; }
        public DelegateCommand RestartQuizCommand { get; }

        public int QuestionPackQuestionCount
        {
            get => _questionPackQuestionCount;
            set
            {
                _questionPackQuestionCount = value;
                RaisePropertyChanged();
            }
        }
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
            for (int i = 0; i < answersOptionCount; i++)
            {
                RevealedAnswers.Add(null);
            }

            InitializeTimer();

            this.mainWindowViewModel = mainWindowViewModel;

            PlayerGuessCommand = new DelegateCommand(PlayerGuess);
            RestartQuizCommand = new DelegateCommand(RestartQuiz);
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += OnTimerTick;
        }

        private void RestartQuiz(object obj)
        {
            StartQuiz(ActivePack);
        }

        public void StartQuiz(QuestionPackViewModel questionPackViewModel)
        {
            ActivePack = questionPackViewModel;
            QuizOngoing = true;
            QuestionPackQuestionCount = ActivePack.Questions.Count;
            questions = RandomizeArray(ActivePack.Questions.ToArray());
            ActiveQuestion = questions[0];
            QuestionCount = 0;
            CorrectGuesses = 0;

            StartQuestion(ActiveQuestion);
        }

        private void StartQuestion(Question question)
        {
            QuestionCount++;
            RevealedAnswers = GetResetRevealedAnswers(RevealedAnswers);

            Answers = GetPreparedQuestions(question);
            Answers = RandomizeArray(Answers);

            TimeLeft = ActivePack.TimeLimitInSeconds;
            timer.Start();
        }

        private ObservableCollection<bool?> GetResetRevealedAnswers(ObservableCollection<bool?> revealedAnswers)
        {
            for (int i = 0; i < revealedAnswers.Count; i++)
            {
                revealedAnswers[i] = null;
            }
            return revealedAnswers;
        }

        private string[] GetPreparedQuestions(Question question)
        {
            string[] answers = new string[answersOptionCount];
            answers[0] = question.CorrectAnswer;

            for (int i = 0; i < question.IncorrectAnswers.Length; i++)
            {
                answers[i + 1] = question.IncorrectAnswers[i];
            }
            return answers;
        }

        private T[] RandomizeArray<T>(T[] array)
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

            await Task.Delay(revealDelayMS);

            if (QuestionPackQuestionCount <= _questionCount)
            {
                EndQuiz();
                return;
            }
            ActiveQuestion = questions[_questionCount];
            StartQuestion(ActiveQuestion);
        }

        private void EndQuiz()
        {
            QuizOngoing = false;
        }

        private void OnTimerTick(object? sender, EventArgs e)
        {
            TimeLeft--;
        }

        private void StopTimer()
        {
            timer.Stop();
        }
    }
}
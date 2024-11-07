using Lab_3.Command;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Lab_3.ViewModel
{
    internal class QuestionImporterViewModel : ViewModelBase
    {
        private static readonly Category LoadingCategory = new Category { Name = "Loading..." };
        private string _errorMessage;
        private bool _isLoading;
        private MainWindowViewModel mainWindowViewModel;
        private Category _selectedCategory;
        private static readonly HttpClient client = new HttpClient();

        public ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                RaisePropertyChanged();
            }
        }
        public Difficulty Difficulty { get; set; } = Difficulty.Medium;
        public static int NumberOfQuestions { get; set; } = 1;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand FetchAndValidateCommand { get; }
        public event Action RequestClose;

        public QuestionImporterViewModel()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindowViewModel = (MainWindowViewModel)(App.Current.MainWindow as MainWindow).DataContext;
            });

            FetchAndValidateCommand = new DelegateCommand(async _ => await FetchAndValidateAsync());

            InitializeCategoriesAsync().ConfigureAwait(false);
        }

        private async Task InitializeCategoriesAsync()
        {
            IsLoading = true;
            Categories.Clear();
            Categories.Add(LoadingCategory);
            SelectedCategory = LoadingCategory;

            try
            {
                var categories = await FetchCategoriesAsync("https://opentdb.com/api_category.php") ?? new List<Category>();

                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
                SelectedCategory = Categories.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error initializing categories: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<List<Category>> FetchCategoriesAsync(string uri)
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var categoryResponse = JsonSerializer.Deserialize<CategoryResponse>(jsonResponse);
                return categoryResponse?.TriviaCategories ?? new List<Category>();
            }
            else
            {
                ErrorMessage = $"Failed to retrieve categories: {response.StatusCode}";
                return new List<Category>();
            }
        }

        private static async Task<Category> GetCategoryAsync(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Category>(jsonResponse);
            }
            return null;
        }

        private static async Task<List<Question>> FetchQuestionsAsync(Category selectedCategory, int numberOfQuestions, Difficulty difficulty, Action<string> setErrorMessage)
        {
            string url = $"https://opentdb.com/api.php?amount={numberOfQuestions}&category={selectedCategory.Id}&difficulty={Uri.EscapeDataString(difficulty.ToString().ToLower())}&type=multiple";
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var questionResponse = JsonSerializer.Deserialize<QuestionResponse>(jsonResponse);

                if (questionResponse?.Results != null)
                {
                    questionResponse = await GetHtmlDecodedQuestionResponseAsync(questionResponse);
                }
                return questionResponse?.Results ?? new List<Question>();
            }
            else
            {
                setErrorMessage?.Invoke($"Failed to retrieve questions for category {selectedCategory.Name}: {response.StatusCode}");
                return null;
            }
        }

        private async Task<bool> LoadQuestionsForSelectedCategoryAsync()
        {
            if (SelectedCategory == null)
            {
                return false;
            }

            var questions = await FetchQuestionsAsync(SelectedCategory, 
                NumberOfQuestions, 
                Difficulty,
                message => ErrorMessage = message);

            if(questions == null)
            {
                return false;
            }

            if (questions.Count < NumberOfQuestions)
            {
                ErrorMessage = $"Not enough questions in the selected category. Please adjust your selection.";
                return false;
            }

            foreach (var question in questions)
            {
                mainWindowViewModel.ActivePack.Questions.Add(question);
            }

            Task.Run(async () => await mainWindowViewModel.JsonQuestionPackHandler.SaveQuestionPacksToJsonAsync(mainWindowViewModel.Packs));

            ErrorMessage = string.Empty;
            return true;
        }

        private async Task FetchAndValidateAsync()
        {
            bool isValid = await LoadQuestionsForSelectedCategoryAsync();

            if (isValid)
            {
                OnRequestClose();
            }
        }

        private static async Task<QuestionResponse> GetHtmlDecodedQuestionResponseAsync(QuestionResponse questionResponse)
        {
            foreach (var question in questionResponse.Results)
            {
                question.Query = WebUtility.HtmlDecode(question.Query);
                question.CorrectAnswer = WebUtility.HtmlDecode(question.CorrectAnswer);

                for (int i = 0; i < question.IncorrectAnswers.Length; i++)
                {
                    question.IncorrectAnswers[i] = WebUtility.HtmlDecode(question.IncorrectAnswers[i]);
                }
            }
            return questionResponse;
        }

        private void OnRequestClose()
        {
            RequestClose?.Invoke();
        }
    }
}

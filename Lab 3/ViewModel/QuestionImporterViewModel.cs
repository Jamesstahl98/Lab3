using Lab_3.Command;
using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                FetchQuestionsCommand.RaiseCanExecuteChanged();
            }
        }

        public Difficulty Difficulty { get; set; } = Difficulty.Medium;

        public static int NumberOfQuestions { get; set; } = 1;
        public DelegateCommand FetchQuestionsCommand { get; }

        public QuestionImporterViewModel()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                mainWindowViewModel = (MainWindowViewModel)(App.Current.MainWindow as MainWindow).DataContext;
            });

            InitializeCategoriesAsync().ConfigureAwait(false);

            FetchQuestionsCommand = new DelegateCommand(
                                    async _ => await LoadQuestionsForSelectedCategoryAsync(),
                                    _ => SelectedCategory != null
                                    );
        }

        private async Task InitializeCategoriesAsync()
        {
            try
            {
                var categories = await FetchCategoriesAsync("https://opentdb.com/api_category.php") ?? new List<Category>();

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing categories: {ex.Message}");
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
                Debug.WriteLine($"Failed to retrieve categories: {response.StatusCode}");
                return new List<Category>();
            }
        }

        private static async Task<Category> GetCategoryAsync(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(jsonResponse);
                return JsonSerializer.Deserialize<Category>(jsonResponse);
            }
            return null;
        }

        private static async Task<List<Question>> FetchQuestionsAsync(Category selectedCategory, int numberOfQuestions, Difficulty difficulty)
        {
            string url = $"https://opentdb.com/api.php?amount={numberOfQuestions}&category={selectedCategory.Id}&difficulty={Uri.EscapeDataString(difficulty.ToString().ToLower())}&type=multiple";
            HttpResponseMessage response = await client.GetAsync(url);
            string jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var questionResponse = JsonSerializer.Deserialize<QuestionResponse>(jsonResponse);
                return questionResponse?.Results ?? new List<Question>();
            }
            else
            {
                Debug.WriteLine($"Failed to retrieve questions for category {selectedCategory.Id}: {response.StatusCode}");
                return new List<Question>();
            }
        }
        private async Task LoadQuestionsForSelectedCategoryAsync()
        {
            if (SelectedCategory != null)
            {
                var questions = await FetchQuestionsAsync(SelectedCategory, NumberOfQuestions, Difficulty);
                foreach (var question in questions)
                {
                    mainWindowViewModel.ActivePack.Questions.Add(question);
                }
            }
        }
    }
}

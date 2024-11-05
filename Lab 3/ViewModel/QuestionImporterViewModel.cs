using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab_3.ViewModel
{
    internal class QuestionImporterViewModel : ViewModelBase
    {
        private static readonly HttpClient client = new HttpClient();
        public ObservableCollection<Category> Categories { get; private set; } = new ObservableCollection<Category>();

        public QuestionImporterViewModel()
        {
            _ = InitializeCategoriesAsync();
        }

        private async Task InitializeCategoriesAsync()
        {
            try
            {
                var categories = await FetchCategoriesAsync("https://opentdb.com/api_category.php") ?? new List<Category>();
                for (int i = 0; i < categories.Count; i++)
                {
                    var detailedCategory = await GetCategoryAsync($"https://opentdb.com/api_category.php?id={categories[i].Id}");
                    Categories.Add(detailedCategory ?? categories[i]);
                    RaisePropertyChanged(Categories[i].Name);
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
                return categoryResponse.TriviaCategories;
            }
            else
            {
                Debug.WriteLine($"Failed to retrieve categories: {response.StatusCode}");
            }
            return new List<Category>();
        }

        private static async Task<Category> GetCategoryAsync(string path)
        {
            Category category = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                category = await response.Content.ReadAsAsync<Category>();
            }
            return category;
        }
    }
}

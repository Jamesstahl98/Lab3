using Lab_3.ViewModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lab_3
{
    class JsonQuestionPackHandler
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly string saveLocation;

        public JsonQuestionPackHandler(MainWindowViewModel mainWindowViewModel)
        {
            saveLocation = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}/QuestionPacks.json";
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public async Task SaveQuestionPacksToJsonAsync(ObservableCollection<QuestionPackViewModel> questionPacks)
        {
            try
            {
                string json = JsonSerializer.Serialize(questionPacks);
                await File.WriteAllTextAsync(saveLocation, json);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"I/O error saving to file: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error serializing question packs: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<ObservableCollection<QuestionPackViewModel>> LoadQuestionPacksAsync()
        {
            if(!File.Exists(saveLocation))
            {
                using (File.Create(saveLocation)) { }
            }
            try
            {
                await using FileStream fileStream = File.OpenRead(saveLocation);
                if (fileStream.Length == 0)
                {
                    return new ObservableCollection<QuestionPackViewModel>();
                }
                var questionPacks = await JsonSerializer.DeserializeAsync<ObservableCollection<QuestionPackViewModel>>(fileStream);
                return questionPacks;
            }

            catch (JsonException ex)
            {
                Debug.WriteLine($"Invalid JSON: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Debug.WriteLine("Access denied to the file: " + ex.Message);
            }
            catch (IOException ex)
            {
                Debug.WriteLine("I/O error while reading the file: " + ex.Message);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
            }
            return new ObservableCollection<QuestionPackViewModel>();
        }
    }
}

using Lab_3.ViewModel;
using System.Collections.ObjectModel;
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
            string json = JsonSerializer.Serialize(questionPacks);
            await File.WriteAllTextAsync(saveLocation, json);
        }

        public async Task<ObservableCollection<QuestionPackViewModel>> LoadQuestionPacksAsync()
        {
            if(!File.Exists(saveLocation))
            {
                using (File.Create(saveLocation)) { }
            }
            await using FileStream fileStream = File.OpenRead(saveLocation);
            if(fileStream.Length == 0)
            {
                return new ObservableCollection<QuestionPackViewModel>();
            }
            var questionPacks = await JsonSerializer.DeserializeAsync<ObservableCollection<QuestionPackViewModel>>(fileStream);
            return questionPacks;
        }
    }
}

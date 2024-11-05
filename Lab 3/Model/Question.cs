using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace Lab_3.Model
{
    internal class Question
    {
        [JsonPropertyName("question")]
        public string Query { get; set; }

        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public string[] IncorrectAnswers { get; set; }

        public Question(string query = "New Question", string correctAnswer = "", string incorrectAnswerOne = "", string incorrectAnswerTwo = "", string incorrectAnswerThree = "")
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswerOne, incorrectAnswerTwo, incorrectAnswerThree };
        }

        public Question()
        {

        }

        public override string ToString()
        {
            return Query;
        }
    }
}

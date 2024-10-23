using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.Model
{
    internal class Question
    {
        public string Query { get; set; }
        public string CorrectAnswer { get; set; }
        public string[] IncorrectAnswers { get; set; }

        public Question(string query, string correctAnswer, string incorrectAnswerOne, string incorrectAnswerTwo, string incorrectAnswerThree)
        {
            Query = query;
            CorrectAnswer = correctAnswer;
            IncorrectAnswers = new string[3] { incorrectAnswerOne, incorrectAnswerTwo, incorrectAnswerThree };
        }
    }
}

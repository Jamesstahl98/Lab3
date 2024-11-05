using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab_3.Model
{
    internal class QuestionResponse
    {
        [JsonPropertyName("results")]
        public List<Question> Results { get; set; }
    }
}

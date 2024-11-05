using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab_3.Model
{
    class CategoryResponse
    {
        [JsonPropertyName("trivia_categories")]
        public List<Category> TriviaCategories { get; set; }
    }
}

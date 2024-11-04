using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_3.Model
{
    internal class Category
    {
        public Dictionary<Enum, ObservableCollection<Question>> Questions { get; set; }


    }
}

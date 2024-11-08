using Lab_3.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Diagnostics;

namespace Lab_3.Converters
{
    public class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (int)value;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (Difficulty)value;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return value;
            }
        }
    }
    }
}

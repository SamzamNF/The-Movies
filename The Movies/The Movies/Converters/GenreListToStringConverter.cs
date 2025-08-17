using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using The_Movies.Model;

namespace The_Movies.Converters
{
    public class GenreListToStringConverter : IValueConverter
    {
        // Konverterer en liste af genrer til en string adskilt med bindestreger - Som så forbindes i xaml under commandparameter ved "Genrer" i datagrid
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Konverterer en samling af Genre-objekter til en string adskilt med bindestreger, hvis den ikke er tom - ellers returneres en tom string
            if (value is IEnumerable<Genre> genres)
            {
                return string.Join(" - ", genres);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
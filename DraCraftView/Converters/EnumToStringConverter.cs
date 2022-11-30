using System;
using System.Globalization;
using System.Windows.Data;

namespace DraCraft.View
{
    public class EnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string? EnumString;
            try
            {
                EnumString = Enum.GetName(value.GetType(), value);
                return EnumString ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Enum.Parse(targetType, value.ToString() ?? "");
            }
            catch
            {

                return Enum.ToObject(targetType, 0);
            }
        }
    }
}

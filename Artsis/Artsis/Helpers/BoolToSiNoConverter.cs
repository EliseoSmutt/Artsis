using System.Globalization;
namespace Artsis.Helpers
{
    public class BoolToSiNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? "SI" : "NO";
            return "NO";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString().ToUpper() == "SI";
        }
    }

}

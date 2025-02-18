using System.Globalization;

namespace SpinningTrainer.Resources.Converters
{
    public class MaterialIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPasswordHidden)
            {
                // Cambiar el ícono según el estado
                return isPasswordHidden ? "{mi:Material Icon=VisibilityOff}" : "{mi:Material Icon=Visibility}";
            }
            return "{mi:Material Icon=VisibilityOff}";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

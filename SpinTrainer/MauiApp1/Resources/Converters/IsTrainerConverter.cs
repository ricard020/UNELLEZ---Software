using System.Globalization;

namespace SpinningTrainer.Resources.Converters
{
    internal class IsTrainerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            short valueShort = short.Parse(value.ToString());
            if (valueShort is short tipoUsuario)
            {
                return tipoUsuario == 2; // Trainer is 2
            }

            return false; // Not a trainer
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Not required for this converter
            throw new NotImplementedException();
        }
    }
}

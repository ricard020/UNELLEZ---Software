using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinningTrainer.Resources.Converters
{
    internal class NotIsSuperUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int tipoUsuario)
            {
                return tipoUsuario != 0; // Trainer is 2
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

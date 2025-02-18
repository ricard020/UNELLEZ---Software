using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpinningTrainer.Resources.Converters
{
    internal class UserTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is short userType)
            {
                switch (userType)
                {
                    case 0:
                        return "Super Usuario";
                    case 1:
                        return "Administrador";
                    case 2:
                        return "Entrenador";
                    default:
                        return "Tipo Desconocido"; // Handle unknown values
                }
            }

            return value; // Return the original value if not an int
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This method is not required for one-way conversion
            throw new NotImplementedException();
        }
    }
}

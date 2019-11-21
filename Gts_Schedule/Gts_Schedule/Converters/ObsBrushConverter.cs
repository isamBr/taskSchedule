using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace Gts_Schedule.Converters
{
    class ObsBrushConverter : IValueConverter
    {
        public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            string currentObsColor = value.ToString();
          
            return Color.FromHex(currentObsColor);

        }

        public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

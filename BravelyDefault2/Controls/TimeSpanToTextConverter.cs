using System;
using System.Globalization;
using System.Windows.Data;

namespace BravelyDefault2.Controls {
    class TimeSpanToTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null) {
                return string.Empty;
            }

            return new DateTime(((TimeSpan)value).Ticks).ToString("HH:mm:ss");
            ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return new TimeSpan();
        }
    }
}
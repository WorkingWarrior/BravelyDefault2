using System;
using System.Globalization;
using System.Windows.Data;

namespace BravelyDefault2.Controls {
    class DateTimeToTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null) {
                return string.Empty;
            }

            return ((DateTime)value).ToString("HH:mm:ss dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return new DateTime();
        }
    }
}

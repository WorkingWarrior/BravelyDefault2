using System;
using System.Globalization;
using System.Windows.Data;

namespace BravelyDefault2.Controls {
    public class JobToTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value == null) {
                return null;
            }

            return (value as Job).SaveDataID;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Job.FromID(value as string);
        }
    }
}
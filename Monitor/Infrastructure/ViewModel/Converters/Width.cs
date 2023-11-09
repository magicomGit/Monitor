using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Monitor.Infrastructure.ViewModel.Converters
{
    public class Width : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            var volume = (decimal)value;
            if (volume == 0 || Services.Info.MaxQuantity == 0) { return 0; }

            var volumePercent = volume / (Services.Info.MaxQuantity / 100);

            var width = volumePercent / (100 / 100);
            return width;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

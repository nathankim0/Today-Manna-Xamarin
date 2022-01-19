using System;
using System.Globalization;
using Xamarin.Forms;

namespace TodaysManna
{
    public class AllSelectIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? FontIcons.CheckboxMultipleMarkedOutline : FontIcons.CheckboxMultipleBlankOutline;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}

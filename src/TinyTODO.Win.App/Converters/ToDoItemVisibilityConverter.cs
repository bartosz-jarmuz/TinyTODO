using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TinyTODO.App.Windows.Converters
{
    public sealed class ToDoItemVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                bool showCompleted = (bool)values[0];
                bool isCompleted = (bool)values[1];
                return (isCompleted && !showCompleted) ? Visibility.Collapsed : Visibility.Visible;
            }
            catch (Exception ex)
            {
#if DEBUG
                return Visibility.Visible;
#endif
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

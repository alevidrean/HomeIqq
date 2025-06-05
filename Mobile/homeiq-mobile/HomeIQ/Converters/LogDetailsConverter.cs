using System;
using System.Globalization;
using HomeIQ.Services; // Or wherever your EventLog class is
using Microsoft.Maui.Controls;

namespace HomeIQ.Converters
{
    public class LogDetailsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is EventLog log)
            {
                return $"{log.EventType} | {log.Details} | User: {log.UserId} | {log.Timestamp:yyyy-MM-dd HH:mm:ss}";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
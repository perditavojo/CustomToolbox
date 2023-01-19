using System.Globalization;
using System.Windows.Data;

namespace CustomToolbox.Common.Converters;

/// <summary>
/// 供 DataGrid 使用的 TimeSpanConverter
/// </summary>
class TimeSpanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan)
        {
            return value;
        }
        else if (value is double result1)
        {
            return TimeSpan.FromSeconds(result1);
        }
        else if (value is string ||
            value is int ||
            value is long ||
            value is float ||
            value is decimal)
        {
            if (double.TryParse(value.ToString(), out double result2))
            {
                return TimeSpan.FromSeconds(result2);
            }
        }

        return TimeSpan.FromSeconds(0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan)
        {
            return value;
        }
        else if (value is double result1)
        {
            return TimeSpan.FromSeconds(result1);
        }
        else if (value is string ||
            value is int ||
            value is long ||
            value is float ||
            value is decimal)
        {
            if (double.TryParse(value.ToString(), out double result2))
            {
                return TimeSpan.FromSeconds(result2);
            }
        }

        return TimeSpan.FromSeconds(0);
    }
}
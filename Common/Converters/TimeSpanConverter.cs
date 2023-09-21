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
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // TODO: 2023-09-21 待調整。
        bool canParse = TimeSpan.TryParse(value.ToString(), out TimeSpan result);

        if (!canParse)
        {
            value = TimeSpan.Zero;
        }

        if (result.Days > 0)
        {
            double seconds = result.Days;

            value = TimeSpan.FromSeconds(seconds);
        }

        return value;
    }
}
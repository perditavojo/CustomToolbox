using System.Globalization;
using System.Windows.Data;

namespace CustomToolbox.Common.Converters;

/// <summary>
/// 供 DataGrid 使用的 TimeSpanToSecondsConverter
/// </summary>
class TimeSpanToSecondsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TimeSpan timeSpan)
        {
            if (timeSpan.Days == 0)
            {
                return timeSpan.TotalSeconds;
            }

            double seconds = double.TryParse(
                timeSpan.Days.ToString(),
                out double parsedDouble) ?
                parsedDouble :
                -1;

            if (seconds == -1)
            {
                return timeSpan.TotalSeconds;
            }

            TimeSpan tsNew = TimeSpan.FromSeconds(seconds);

            return tsNew.TotalSeconds;
        }

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}
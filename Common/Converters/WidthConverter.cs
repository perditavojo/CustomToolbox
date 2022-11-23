using System.Globalization;
using System.Windows.Data;

namespace CustomToolbox.Common.Converters;

/// <summary>
/// 寬度轉換器
/// </summary>
class WidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double output = 0.0d;

        string sValue = value?.ToString() ?? string.Empty;
        string sParam = parameter?.ToString() ?? string.Empty;

        if (double.TryParse(sValue, out double oValue))
        {
            if (!string.IsNullOrEmpty(sParam) &&
                double.TryParse(sParam, out double oParam))
            {
                oValue = oValue += oParam;
            }

            output = oValue;
        }

        return output;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        double output = 0.0d;

        string sValue = value?.ToString() ?? string.Empty;
        string sParam = parameter?.ToString() ?? string.Empty;

        if (double.TryParse(sValue, out double oValue))
        {
            if (!string.IsNullOrEmpty(sParam) &&
                double.TryParse(sParam, out double oParam))
            {
                oValue = oValue += oParam;
            }

            output = oValue;
        }

        return output;
    }
}
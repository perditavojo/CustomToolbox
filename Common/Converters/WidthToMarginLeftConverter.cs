using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CustomToolbox.Common.Converters;

/// <summary>
/// 寬度至 Margin 的 Left 轉換器
/// </summary>
class WidthToMarginLeftConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is uint ||
            value is int ||
            value is double ||
            value is float ||
            value is long ||
            value is decimal)
        {
            string sValue = value?.ToString() ?? string.Empty,
                sParam = parameter?.ToString() ?? string.Empty;

            if (int.TryParse(sValue, out int oValue))
            {
                if (!string.IsNullOrEmpty(sParam) &&
                    int.TryParse(sParam, out int oParam))
                {
                    oValue = oValue += oParam;
                }

                return new Thickness(oValue, 5, 5, 5);
            }
        }

        return new Thickness();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
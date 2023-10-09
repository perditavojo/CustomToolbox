using System.Globalization;
using System.Windows;
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
        // 理論上 value 一定會是 TimeSpan。

        // 先將 value 轉換成字串。
        string strValue = value.ToString() ?? string.Empty;

        // 嘗試解析字串。
        bool canParse = TimeSpan.TryParse(strValue, out TimeSpan result);

        // 當不能解析字串時，則直接返回 DependencyProperty.UnsetValue。
        if (!canParse)
        {
            return DependencyProperty.UnsetValue;
        }

        // 判斷 TimeSpan 的 Days 是否大於 0，
        // 在本應用程式中 TimeSpan 的 Days 應該都要等於 0。
        if (result.Days > 0)
        {
            // 將 Days 當作秒數。
            double seconds = result.Days;

            // 重新產生 TimeSpan 並指派回 value。
            value = TimeSpan.FromSeconds(seconds);
        }

        return value;
    }
}
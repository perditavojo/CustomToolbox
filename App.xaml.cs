using Application = System.Windows.Application;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Utils;
using MessageBox = System.Windows.MessageBox;
using System.Windows;

namespace CustomToolbox;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// 應用程式預設的 LangData
    /// </summary>
    private static readonly LangData? _DefaultLangData = AppLangUtil.GetAppDefaultLangData();

    /// <summary>
    /// 應用程式語系資料
    /// </summary>
    private static readonly AppLangData _AppLangData = AppLangUtil.GetAppLangData();

    /// <summary>
    /// 預設的應用程式名稱
    /// </summary>
    private static readonly string DefaultTitle = "CustomToolbox";

    protected override void OnStartup(StartupEventArgs e)
    {
        try
        {
            base.OnStartup(e);

            CustomInit();
        }
        catch (Exception ex)
        {
            // 使用 MessageBox 輸出錯誤訊息。
            MessageBox.Show(ex.ToString(),
                caption: DefaultTitle,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// 取得語系列表
    /// </summary>
    /// <returns>List&lt;LangData&gt;</returns>
    public static List<LangData>? GetLangData()
    {
        return _AppLangData.LangDatas;
    }

    /// <summary>
    /// 應用程式預設的 LangData
    /// </summary>
    /// <returns>LangData</returns>
    public static LangData? GetDefaultLangData()
    {
        return _DefaultLangData;
    }

    /// <summary>
    /// 自定義初始化
    /// </summary>
    private static void CustomInit()
    {
        string errMsg = string.Empty;

        errMsg += AppThemeUtil.SetAppTheme();

        if (!string.IsNullOrEmpty(errMsg))
        {
            errMsg += Environment.NewLine;
        }

        errMsg += _AppLangData.ErrMsg;

        if (!string.IsNullOrEmpty(errMsg))
        {
            errMsg += Environment.NewLine;
        }

        errMsg += AppLangUtil.SetAppLang();

        if (!string.IsNullOrEmpty(errMsg))
        {
            // 使用 MessageBox 輸出錯誤訊息。
            MessageBox.Show(errMsg,
                DefaultTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
using Application = System.Windows.Application;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Utils;
using MessageBox = System.Windows.MessageBox;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollection = Microsoft.Extensions.DependencyInjection.ServiceCollection;
using System.Windows;
using System.Windows.Threading;

namespace CustomToolbox;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// IServiceProvider
    /// </summary>
    private IServiceProvider? ServiceProvider { get; set; }

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

    public App()
    {
        // 參考來源：https://executecommands.com/dependency-injection-in-wpf-net-core-csharp/
        ServiceCollection serviceCollection = new();

        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
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
    /// 設定服務
    /// </summary>
    /// <param name="services">ServiceCollection</param>
    private static void ConfigureServices(ServiceCollection services)
    {
        services.AddHttpClient().AddSingleton<WMain>();
    }

    /// <summary>
    /// 啟動
    /// </summary>
    /// <param name="sender">StartupEventArgs</param>
    /// <param name="e">object</param>
    private void OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            InitUnhandledExceptionEventHandler();

            CustomInit();

            WMain? wMain = ServiceProvider?.GetService<WMain>();

            wMain?.Show();
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
    /// 初始化 UnhandledException 的 EventHandler
    /// <para>參考：https://blog.csdn.net/Iron_Ye/article/details/82913025 </para>
    /// </summary>
    private static void InitUnhandledExceptionEventHandler()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;

        currentDomain.UnhandledException += (object sender, UnhandledExceptionEventArgs args) =>
        {
            Exception ex = (Exception)args.ExceptionObject;

            ShowErrorMsg(ex.ToString());
        };

        Current.DispatcherUnhandledException += (object sender, DispatcherUnhandledExceptionEventArgs e) =>
        {
            ShowErrorMsg(e.Exception.ToString());

            e.Handled = true;
        };

        TaskScheduler.UnobservedTaskException += (object? sender, UnobservedTaskExceptionEventArgs e) =>
        {
            ShowErrorMsg(e.Exception.ToString());

            e.SetObserved();
        };
    }

    /// <summary>
    /// 自定義初始化
    /// </summary>
    private static void CustomInit()
    {
        string errMsg = string.Empty;

        // 更新設定值。
        try
        {
            // 來源：https://stackoverflow.com/a/23924277
            if (CustomToolbox.Properties.Settings.Default.UpdateSettings)
            {
                CustomToolbox.Properties.Settings.Default.Upgrade();
                CustomToolbox.Properties.Settings.Default.UpdateSettings = false;
                CustomToolbox.Properties.Settings.Default.Save();
            }
        }
        catch (Exception ex)
        {
            errMsg += ex.ToString();
        }

        if (!string.IsNullOrEmpty(errMsg))
        {
            errMsg += Environment.NewLine;
        }

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

        ShowErrorMsg(errMsg);
    }

    /// <summary>
    /// 顯示錯誤訊息
    /// </summary>
    /// <param name="message">字串，錯誤訊息</param>
    private static void ShowErrorMsg(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            // 使用 MessageBox 輸出錯誤訊息。
            MessageBox.Show(message,
                DefaultTitle,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
using Application = System.Windows.Application;
using CustomToolbox.Common.Models;
using CustomToolbox.Common.Sets;
using FontFamily = System.Windows.Media.FontFamily;
using System.Globalization;
using System.IO;
using System.Windows;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// 應用程式語系工具
/// </summary>
internal class AppLangUtil
{
    /// <summary>
    /// 取得應用程式預設的 LangData
    /// </summary>
    /// <returns>LangData</returns>
    public static LangData? GetAppDefaultLangData()
    {
        ResourceDictionary? resDict = Application.Current.Resources
            .MergedDictionaries
            .FirstOrDefault(n => !string.IsNullOrEmpty(GetLangCode(n)));

        return resDict != null ? CreateLangData(resDict) : null;
    }

    /// <summary>
    /// 取得應用程式語系資料
    /// </summary>
    /// <returns>AppLangData</returns>
    public static AppLangData GetAppLangData()
    {
        string message = string.Empty;

        List<LangData> outputList = new();

        try
        {
            LangData? defaultLangData = App.GetDefaultLangData();

            if (defaultLangData != null)
            {
                outputList.Add(defaultLangData);
            }

            string[] patterns = { "*.xaml" };

            IEnumerable<string> files = CustomFunction.EnumerateFiles(
                VariableSet.LangsFolderPath,
                patterns);

            foreach (string file in files)
            {
                ResourceDictionary rdNew = new()
                {
                    Source = new Uri(file, UriKind.RelativeOrAbsolute)
                };

                LangData? newLangData = CreateLangData(rdNew);

                if (newLangData != null)
                {
                    outputList.Add(newLangData);
                }
            }
        }
        catch (Exception ex)
        {
            message = ex.ToString();
        }

        return new AppLangData()
        {
            LangDatas = outputList,
            ErrMsg = message
        };
    }

    /// <summary>
    /// 設定應用程式使用的語系
    /// </summary>
    /// <param name="langCode">字串，語系代碼</param>
    /// <returns>字串</returns>
    public static string SetAppLang(string langCode = "")
    {
        string message = string.Empty;

        try
        {
            // 當設定值 AppLangCode 的值為 null 或空白時。
            // 將系統使用的 CultureInfo 的 Name 儲存至 AppLangCode。
            if (string.IsNullOrEmpty(Properties.Settings.Default.AppLangCode))
            {
                CultureInfo cultureInfo = CultureInfo.CurrentCulture;

                Properties.Settings.Default.AppLangCode = cultureInfo.Name;
                Properties.Settings.Default.Save();
            }

            // 當傳入的 langCode 為空白時，則使用 AppLangCode 儲存的值。
            if (string.IsNullOrEmpty(langCode))
            {
                langCode = Properties.Settings.Default.AppLangCode;
            }

            if (Properties.Settings.Default.AppLangCode != langCode)
            {
                Properties.Settings.Default.AppLangCode = langCode;
                Properties.Settings.Default.Save();
            }

            LangData? targetLangData = App.GetLangData()
                ?.FirstOrDefault(n => n.LangCode == langCode);

            // 當 targetLangData 為 null 時，則使用應用程式預設的 LangData。
            targetLangData ??= App.GetDefaultLangData();

            if (targetLangData != null &&
                targetLangData.ResDict != null)
            {
                // 暫存應用程式在 Application.Current.Resources.MergedDictionaries
                // 內的 ResourceDictionary。
                List<ResourceDictionary> listResDict = new();

                // 只排除語系的 ResourceDictionary。
                listResDict.AddRange(Application.Current.Resources
                    .MergedDictionaries
                    .Where(n => string.IsNullOrEmpty(GetLangCode(n))));

                // 加入 targetLangData 的 ResDict。
                listResDict.Add(targetLangData.ResDict);

                // 清除原本的 Application.Current.Resources.MergedDictionaries。
                // ※因為只刪除一個後再加入會造成找不到資源檔的問題。
                Application.Current.Resources
                    .MergedDictionaries
                    .Clear();

                // 重新將 ResourceDictionary 加回 Application.Current.Resources.MergedDictionaries。
                foreach (ResourceDictionary resDict in listResDict)
                {
                    Application.Current.Resources
                        .MergedDictionaries
                        .Add(resDict);
                }
            }
        }
        catch (Exception ex)
        {
            message = ex.ToString();
        }

        return message;
    }

    /// <summary>
    /// 取得目前語系的 ResourceDictionary
    /// </summary>
    /// <returns>ResourceDictionary</returns>
    public static ResourceDictionary? GetCurrentLangResDict()
    {
        return Application.Current.Resources
            .MergedDictionaries
            .FirstOrDefault(n => !string.IsNullOrEmpty(GetLangCode(n)));
    }

    /// <summary>
    /// 取得語系名稱
    /// </summary>
    /// <param name="resourceDictionary">ResourceDictionary</param>
    /// <returns>字串</returns>
    public static string GetLangName(ResourceDictionary? resourceDictionary)
    {
        return resourceDictionary?["LangName"]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 取得語系代碼
    /// </summary>
    /// <param name="resourceDictionary">ResourceDictionary</param>
    /// <returns>字串</returns>
    public static string GetLangCode(ResourceDictionary? resourceDictionary)
    {
        return resourceDictionary?["LangCode"]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 取得語預設的文字編碼
    /// </summary>
    /// <param name="resourceDictionary">ResourceDictionary</param>
    /// <returns>字串</returns>
    public static string GetDefaultEncoding(ResourceDictionary? resourceDictionary)
    {
        return resourceDictionary?["DefaultEncoding"]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 取得語預設的字型
    /// </summary>
    /// <param name="resourceDictionary">ResourceDictionary</param>
    /// <returns>字串</returns>
    public static string GetDefaultFont(ResourceDictionary? resourceDictionary)
    {
        return resourceDictionary?["DefaultFont"]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 取得語預設的日誌紀錄用字型
    /// </summary>
    /// <param name="resourceDictionary">ResourceDictionary</param>
    /// <returns>字串</returns>
    public static string GetDefaultLogFont(ResourceDictionary? resourceDictionary)
    {
        return resourceDictionary?["DefaultLogFont"]?.ToString() ?? string.Empty;
    }

    /// <summary>
    /// 取得日誌紀錄用的 FontFamily
    /// </summary>
    /// <returns>FontFamily</returns>
    public static FontFamily GetLogFontFamily()
    {
        return new FontFamily(GetDefaultLogFont(GetCurrentLangResDict()));
    }

    /// <summary>
    /// 建立 LangData
    /// </summary>
    /// <param name="resourceDictionary">ResourceDictionary</param>
    /// <returns>LangData</returns>
    private static LangData? CreateLangData(ResourceDictionary resourceDictionary)
    {
        string langName = GetLangName(resourceDictionary),
            langCode = GetLangCode(resourceDictionary),
            fileName = Path.GetFileName(resourceDictionary.Source.ToString());

        if (!string.IsNullOrEmpty(langName))
        {
            return new LangData()
            {
                LangName = langName,
                LangCode = langCode,
                ResDict = resourceDictionary,
                LangFileName = fileName
            };
        }

        return null;
    }
}
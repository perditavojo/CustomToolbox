using CustomToolbox.Common.Models.UpdateNotifier;
using CustomToolbox.Common.Sets;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace CustomToolbox.Common;

/// <summary>
/// 更新通知器
/// </summary>
internal class UpdateNotifier
{
    /// <summary>
    /// 檢查版本
    /// </summary>
    /// <returns>Task&lt;CheckResult&gt;</returns>
    public static async Task<CheckResult> CheckVersion(HttpClient httpClient)
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            Version? localVersion = assemblyName.Version;

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(UrlSet.AppVersionJsonUrl);

            httpResponseMessage.EnsureSuccessStatusCode();

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                return new CheckResult()
                {
                    IsException = true,
                    MessageText = MsgSet.GetFmtStr(
                        MsgSet.MsgUpdateNotifierHttpErrorCode,
                        httpResponseMessage.StatusCode.ToString())
                };
            }

            string textContent = await httpResponseMessage.Content.ReadAsStringAsync();

            List<AppData>? dataSet = JsonSerializer.Deserialize<List<AppData>>(textContent);

            if (dataSet == null)
            {
                return new CheckResult()
                {
                    IsException = true,
                    MessageText = MsgSet.MsgUpdateNotifierCantParsedAppVersionJsonFile
                };
            }

            AppData? appData = dataSet.FirstOrDefault(n => n.App == assemblyName.Name);

            if (appData == null)
            {
                return new CheckResult()
                {
                    IsException = true,
                    MessageText = MsgSet.GetFmtStr(
                        MsgSet.MsgUpdateNotifierCantFindAppData,
                        assemblyName.Name ?? string.Empty)
                };
            };

            if (appData.AppVersion == null)
            {
                return new CheckResult()
                {
                    IsException = true,
                    MessageText = MsgSet.GetFmtStr(
                        MsgSet.MsgUpdateNotifierCantParsedAppVersionData,
                        assemblyName.Name ?? string.Empty)
                };
            }

            Version? netVersion = Version.Parse(appData.AppVersion);

            CheckResult checkResult = new();

            int compareResult = netVersion.CompareTo(localVersion);

            if (compareResult > 0)
            {
                checkResult.HasNewVersion = true;
                checkResult.NetVersionIsOdler = false;
                checkResult.IsException = false;
                checkResult.MessageText = MsgSet.GetFmtStr(
                    MsgSet.MsgUpdateNotifierHasNewVersion,
                    appData.AppVersion);
                checkResult.VersionText = appData.AppVersion;
                checkResult.Checksum = appData.Checksum;
                checkResult.DownloadUrl = appData.DownloadUrl;
            }
            else if (compareResult == 0)
            {
                checkResult.HasNewVersion = false;
                checkResult.NetVersionIsOdler = false;
                checkResult.IsException = false;
                checkResult.MessageText = MsgSet.GetFmtStr(
                    MsgSet.MsgUpdateNotifierAlreadyLatestVersion,
                    localVersion?.ToString() ?? string.Empty);
                checkResult.VersionText = appData.AppVersion;
                checkResult.Checksum = appData.Checksum;
                checkResult.DownloadUrl = appData.DownloadUrl;
            }
            else
            {
                checkResult.HasNewVersion = false;
                checkResult.NetVersionIsOdler = true;
                checkResult.IsException = false;
                checkResult.MessageText = MsgSet.GetFmtStr(
                    MsgSet.MsgUpdateNotifierDowngradeNotice,
                    netVersion?.ToString() ?? string.Empty,
                    localVersion?.ToString() ?? string.Empty);
                checkResult.VersionText = appData.AppVersion;
                checkResult.Checksum = appData.Checksum;
                checkResult.DownloadUrl = appData.DownloadUrl;
            }

            return checkResult;
        }
        catch (Exception ex)
        {
            return new CheckResult()
            {
                IsException = true,
                MessageText = ex.Message
            };
        }
    }
}
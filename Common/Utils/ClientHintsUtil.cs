using System.Net;
using System.Net.Http;

namespace CustomToolbox.Common.Utils;

/// <summary>
/// Client Hints 工具
/// </summary>
internal class ClientHintsUtil
{
    /// <summary>
    /// Client Hints
    /// </summary>
    private static readonly Dictionary<string, string> KeyValues = new()
    {
        { "Sec-CH-UA", Properties.Settings.Default.SecChUa },
        { "Sec-CH-UA-Mobile", Properties.Settings.Default.SecChUaMobile },
        { "Sec-CH-UA-Platform", Properties.Settings.Default.SecChUaPlatform },
        { "Sec-Fetch-Dest", Properties.Settings.Default.SecFetchDest },
        { "Sec-Fetch-Mode", Properties.Settings.Default.SecFetchMode },
        { "Sec-Fetch-Site", Properties.Settings.Default.SecFetchSite },
        { "Sec-Fetch-User", Properties.Settings.Default.SecFetchUser }
    };

    /// <summary>
    /// 設定 Client Hints 標頭資訊
    /// </summary>
    /// <param name="webHeaderCollection">HttpClient</param>
    public static void SetClientHints(WebHeaderCollection webHeaderCollection)
    {
        foreach (KeyValuePair<string, string> item in KeyValues)
        {
            webHeaderCollection.Add(item.Key, item.Value);
        }
    }

    /// <summary>
    /// 設定 Client Hints 標頭資訊
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    public static void SetClientHints(HttpClient? httpClient)
    {
        foreach (KeyValuePair<string, string> item in KeyValues)
        {
            httpClient?.DefaultRequestHeaders.Add(item.Key, item.Value);
        }
    }
}
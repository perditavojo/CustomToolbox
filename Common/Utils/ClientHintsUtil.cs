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
        // TODO: 2023-02-24 待測試 Client Hints。
        { "Sec-CH-UA", "\"Chromium\";v=\"110\", \"Not A(Brand\";v=\"24\", \"Google Chrome\";v=\"110\"" },
        { "Sec-CH-UA-Mobile", "?0" },
        { "Sec-CH-UA-Platform", "Windows" },
        { "Sec-Fetch-Dest", "document" },
        { "Sec-Fetch-Mode", "navigate" },
        { "Sec-Fetch-Site", "none" },
        { "Sec-Fetch-User", "?1" }
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
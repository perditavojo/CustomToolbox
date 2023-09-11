using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Text;

namespace CustomToolbox.BilibiliApi.Functions;

/// <summary>
/// 驗證函式
/// <para>來源：https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/wbi.md</para>
/// <para>原授權：CC BY-NC 4.0</para>
/// <para>CC BY-NC 4.0：https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/LICENSE</para>
/// </summary>
public class AuthFunction
{
    /// <summary>
    /// 混合鍵值編碼表
    /// </summary>
    private static readonly int[] MixinKeyEncodeTable =
    {
        46, 47, 18, 2, 53, 8, 23, 32, 15, 50, 10, 31, 58, 3, 45, 35, 27, 43, 5, 49, 33, 9, 42, 19, 29, 28, 14, 39,
        12, 38, 41, 13, 37, 48, 7, 16, 24, 55, 40, 61, 26, 17, 0, 1, 60, 51, 30, 4, 22, 25, 54, 21, 56, 59, 6, 63,
        57, 62, 11, 36, 20, 34, 44, 52
    };

    /// <summary>
    /// 取得混合鍵值
    /// <para>對 imgKey 和 subKey 進行字元順序打亂編碼。</para>
    /// </summary>
    /// <param name="value">字串，輸入值</param>
    /// <returns>字串</returns>
    private static string GetMixinKey(string value)
    {
        return MixinKeyEncodeTable.Aggregate(string.Empty, (stringValue, index) => stringValue + value[index])[..32];
    }

    /// <summary>
    /// 編碼 Wbi
    /// </summary>
    /// <param name="parameters">Dictionary&lt;string, string&gt;，查詢字串參數</param>
    /// <param name="imgKey">字串，imgKey</param>
    /// <param name="subKey">字串，subKey</param>
    /// <returns>Task&lt;&lt;string, string&gt;&gt;</returns>
    private async static Task<Dictionary<string, string>> EncodeWbi(
        Dictionary<string, string> parameters,
        string imgKey,
        string subKey)
    {
        string mixinKey = GetMixinKey(imgKey + subKey);

        // 加入 wts 的內容。
        parameters["wts"] = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        // 依照鍵值重新排序參數。
        parameters = parameters.OrderBy(n => n.Key).ToDictionary(n => n.Key, n => n.Value);

        // 過濾 value 中的 "!'()*" 字元。
        parameters = parameters.ToDictionary(
            kvp => kvp.Key,
            kvp => new string(kvp.Value.Where(chr => !"!'()*".Contains(chr)).ToArray())
        );

        // 序列化查詢字串參數。
        string queryStringValue = await new FormUrlEncodedContent(parameters).ReadAsStringAsync();

        // 計算 w_rid 的雜湊值。
        byte[] bytesHash = MD5.HashData(Encoding.UTF8.GetBytes(queryStringValue + mixinKey));

        // 移除 "-" 及將字串小寫化。
        string wRid = BitConverter.ToString(bytesHash).Replace("-", string.Empty).ToLower();

        // 加入 w_rid 的內容。
        parameters["w_rid"] = wRid;

        return parameters;
    }

    /// <summary>
    /// 取得 Wbi 的關鍵鍵值
    /// <para>獲取最新的 img_key 和 sub_key。</para>
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <returns>Task&lt;(string, string)&gt;</returns>
    private static async Task<(string, string)> GetWbiKeys(HttpClient httpClient)
    {
        HttpRequestMessage httpRequestMessage = new()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.bilibili.com/x/web-interface/nav"),
        };

        using HttpResponseMessage? httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        string content = await httpResponseMessage.Content.ReadAsStringAsync();

        JsonNode? jnContent = JsonNode.Parse(content),
            jnImgUrl = jnContent?["data"]?["wbi_img"]?["img_url"],
            jnSubUrl = jnContent?["data"]?["wbi_img"]?["sub_url"];

        string imgUrl = jnImgUrl?.ToString() ?? string.Empty;
        string subUrl = jnSubUrl?.ToString() ?? string.Empty;

        if (!string.IsNullOrEmpty(imgUrl))
        {
            imgUrl = imgUrl.Split("/")[^1].Split(".")[0];
        }

        if (!string.IsNullOrEmpty(subUrl))
        {
            subUrl = subUrl.Split("/")[^1].Split(".")[0];
        }

        return (imgUrl, subUrl);
    }

    /// <summary>
    /// 取得帶有授權字串的查詢字串
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <param name="parameters">Dictionary&lt;string, string&gt;，查詢字串參數</param>
    /// <returns>字串</returns>
    public static async Task<string> GetAuthQueryString(
        HttpClient httpClient,
        Dictionary<string, string> parameters)
    {
        (string imgKey, string subKey) = await GetWbiKeys(httpClient);

        Dictionary<string, string> finalParameters = await EncodeWbi(
            parameters,
            imgKey: imgKey,
            subKey: subKey
        );

        return await new FormUrlEncodedContent(finalParameters).ReadAsStringAsync();
    }
}
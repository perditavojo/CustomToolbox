using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Text;

namespace CustomToolbox.BilibiliApi.Funcs;

/// <summary>
/// 驗證函式
/// <para>來源：https://github.com/SocialSisterYi/bilibili-API-collect/blob/master/docs/misc/sign/wbi.md</para>
/// </summary>
public class AuthFunction
{
    /// <summary>
    /// 混合鍵值加密表
    /// </summary>
    private static readonly int[] MixinKeyEncTab =
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
        return MixinKeyEncTab.Aggregate(string.Empty, (stringValue, index) => stringValue + value[index])[..32];
    }

    /// <summary>
    /// 加密 Wbi
    /// </summary>
    /// <param name="parameters">Dictionary&lt;string, string&gt;</param>
    /// <param name="imgKey">字串，imgKey</param>
    /// <param name="subKey">字串，subKey</param>
    /// <returns>Dictionary&lt;string, string&gt;</returns>
    private static Dictionary<string, string> EncWbi(
        Dictionary<string, string> parameters,
        string imgKey,
        string subKey)
    {
        string mixinKey = GetMixinKey(imgKey + subKey);
        string currTime = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();

        // 增加 wts 的內容。
        parameters["wts"] = currTime;

        // 依照 key 重排參數。
        parameters = parameters.OrderBy(n => n.Key).ToDictionary(n => n.Key, n => n.Value);

        // 過濾 value 中的 "!'()*" 字元。
        parameters = parameters.ToDictionary(
            kvp => kvp.Key,
            kvp => new string(kvp.Value.Where(chr => !"!'()*".Contains(chr)).ToArray())
        );

        // 序列化參數。
        string query = new FormUrlEncodedContent(parameters).ReadAsStringAsync().Result;

        // 計算 w_rid。
        byte[] hashBytes = MD5.HashData(Encoding.UTF8.GetBytes(query + mixinKey));

        string wbiSign = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();

        parameters["w_rid"] = wbiSign;

        return parameters;
    }

    /// <summary>
    /// 獲取最新的 img_key 和 sub_key
    /// </summary>
    /// <param name="httpClient">HttpClient</param>
    /// <returns>Task&lt;(string, string)&gt;</returns>
    private static async Task<(string, string)> GetWbiKeys(HttpClient httpClient)
    {
        HttpResponseMessage responseMessage = await httpClient.SendAsync(new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.bilibili.com/x/web-interface/nav"),
        });

        JsonNode response = JsonNode.Parse(await responseMessage.Content.ReadAsStringAsync())!;

        string imgUrl = (string)response["data"]!["wbi_img"]!["img_url"]!;

        imgUrl = imgUrl.Split("/")[^1].Split(".")[0];

        string subUrl = (string)response["data"]!["wbi_img"]!["sub_url"]!;

        subUrl = subUrl.Split("/")[^1].Split(".")[0];

        return (imgUrl, subUrl);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClient"></param>
    /// <returns></returns>
    public static async Task<string> GetWbi(HttpClient httpClient)
    {
        var (imgKey, subKey) = await GetWbiKeys(httpClient);

        Dictionary<string, string> signedParams = EncWbi(
            parameters: new Dictionary<string, string>
            {
                { "foo", "114" },
                { "bar", "514" },
                { "baz", "1919810" }
            },
            imgKey: imgKey,
            subKey: subKey
        );

        string query = await new FormUrlEncodedContent(signedParams).ReadAsStringAsync();

        return query;
    }
}
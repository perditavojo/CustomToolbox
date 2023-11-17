namespace CustomToolbox.BilibiliApi.Sets;

/// <summary>
/// 網址組
/// </summary>
public class UrlSet
{
    /// <summary>
    /// Bilibili 查詢使用者投稿視頻明細的 API 網址
    /// <para>參考來源：https://socialsisteryi.github.io/bilibili-API-collect/docs/user/space.html#%E6%9F%A5%E8%AF%A2%E7%94%A8%E6%88%B7%E6%8A%95%E7%A8%BF%E8%A7%86%E9%A2%91%E6%98%8E%E7%BB%86</para>
    /// </summary>
    public static readonly string BilibiliSpaceApiUrl = "https://api.bilibili.com/x/space/wbi/arc/search";

    /// <summary>
    /// Bilibili 查詢使用者投稿視頻明細的 API 網址（舊）
    /// <para>參考來源：https://socialsisteryi.github.io/bilibili-API-collect/docs/user/space.html#%E6%9F%A5%E8%AF%A2%E7%94%A8%E6%88%B7%E6%8A%95%E7%A8%BF%E8%A7%86%E9%A2%91%E6%98%8E%E7%BB%86</para>
    /// </summary>
    public static readonly string OldBilibiliSpaceApiUrl = "https://api.bilibili.com/x/space/arc/search";

    /// <summary>
    /// Bilibili 使用者空間的網址
    /// </summary>
    public static readonly string BilibiliSpaceUrl = "https://space.bilibili.com/";
}
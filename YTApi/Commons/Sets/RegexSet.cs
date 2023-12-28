using System.Text.RegularExpressions;

namespace YTApi.Commons.Sets;

/// <summary>
/// 正規表達式組
/// </summary>
public static partial class RegexSet
{
    /// <summary>
    /// v
    /// </summary>
    public static readonly Regex VideoID = RegexVideoID();

    /// <summary>
    /// INNERTUBE_API_KEY
    /// </summary>
    public static readonly Regex InnertubeApiKey = RegexInnertubeApiKey();

    /// <summary>
    /// continuation
    /// </summary>
    public static readonly Regex Continuation = RegexContinuation();

    /// <summary>
    /// visitorData
    /// </summary>
    public static readonly Regex VisitorData = RegexVisitorData();

    /// <summary>
    /// clientName
    /// </summary>
    public static readonly Regex ClientName = RegexClientName();

    /// <summary>
    /// clientVersion
    /// </summary>
    public static readonly Regex ClientVersion = RegexClientVersion();

    /// <summary>
    /// ID_TOKEN
    /// </summary>
    public static readonly Regex IDToken = RegexIDToken();

    /// <summary>
    /// SESSION_INDEX
    /// </summary>
    public static readonly Regex SessionIndex = RegexSessionIndex();

    /// <summary>
    /// INNERTUBE_CONTEXT_CLIENT_NAME
    /// </summary>
    public static readonly Regex InnertubeContextClientName = RegexInnertubeContextClientName();

    /// <summary>
    /// INNERTUBE_CONTEXT_CLIENT_VERSION
    /// </summary>
    public static readonly Regex InnertubeContextClientVersion = RegexInnertubeContextClientVersion();

    /// <summary>
    /// INNERTUBE_CLIENT_VERSION
    /// </summary>
    public static readonly Regex InnertubeClientVersion = RegexInnertubeClientVersion();

    /// <summary>
    /// DATASYNC_ID
    /// </summary>
    public static readonly Regex DatasyncID = RegexDatasyncID();

    /// <summary>
    /// DELEGATED_SESSION_ID
    /// </summary>
    public static readonly Regex DelegatedSessionID = RegexDelegatedSessionID();

    [GeneratedRegex("v=(.+)")]
    private static partial Regex RegexVideoID();

    [GeneratedRegex("INNERTUBE_API_KEY\":\"(.+?)\",")]
    private static partial Regex RegexInnertubeApiKey();

    [GeneratedRegex("continuation\":\"(.+?)\",")]
    private static partial Regex RegexContinuation();

    [GeneratedRegex("visitorData\":\"(.+?)\",")]
    private static partial Regex RegexVisitorData();

    [GeneratedRegex("clientName\":\"(.+?)\",")]
    private static partial Regex RegexClientName();

    [GeneratedRegex("clientVersion\":\"(.+?)\",")]
    private static partial Regex RegexClientVersion();

    [GeneratedRegex("ID_TOKEN\"(.+?)\",")]
    private static partial Regex RegexIDToken();

    [GeneratedRegex("SESSION_INDEX\":\"(.*?)\"")]
    private static partial Regex RegexSessionIndex();

    [GeneratedRegex("INNERTUBE_CONTEXT_CLIENT_NAME\":(.*?),")]
    private static partial Regex RegexInnertubeContextClientName();

    [GeneratedRegex("INNERTUBE_CONTEXT_CLIENT_VERSION\":\"(.*?)\"")]
    private static partial Regex RegexInnertubeContextClientVersion();

    [GeneratedRegex("INNERTUBE_CLIENT_VERSION\":\"(.*?)\"")]
    private static partial Regex RegexInnertubeClientVersion();

    [GeneratedRegex("DATASYNC_ID\":\"(.*?)\"")]
    private static partial Regex RegexDatasyncID();

    [GeneratedRegex("DELEGATED_SESSION_ID\":\"(.*?)\"")]
    private static partial Regex RegexDelegatedSessionID();
}
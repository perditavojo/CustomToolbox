namespace CustomToolbox.Common.Models;

/// <summary>
/// 短片清單資料
/// </summary>
internal class ClipListData
{
    /// <summary>
    /// 名稱
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 路徑
    /// </summary>
    public string Path { get; set; }

    public ClipListData(string name, string path)
    {
        Name = name;
        Path = path;
    }

    /// <summary>
    /// 轉換成字串
    /// </summary>
    /// <returns>字串</returns>
    public override string ToString()
    {
        return Name;
    }
}
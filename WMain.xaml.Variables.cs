using CustomToolbox.Models;
using Mpv.NET.Player;

namespace CustomToolbox;

public partial class WMain 
{
    public MpvPlayer? MPlayer = null;

    private readonly List<ClipData> ClipDatas = new();

    private WPopupPlayer? PopupPlayer = null;
}
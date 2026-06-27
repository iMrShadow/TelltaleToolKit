using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Fonts;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaSerializer(typeof(MetaClassSerializer<T3OverlayTextParams>))]
public class T3OverlayTextParams
{
    [MetaMember("mhFont")]
    public Handle<Font> Font { get; set; } = new();

    [MetaMember("mhDlg")]
    public Handle<Dlg> Dlg { get; set; } = new();

    [MetaMember("mDlgNodeName")]
    public Symbol DlgNodeName { get; set; } = Symbol.Empty;

    [MetaMember("mText")]
    public string Text { get; set; } = string.Empty;

    [MetaMember("mInitialPosition")]
    public Vector2 InitialPosition { get; set; }
}

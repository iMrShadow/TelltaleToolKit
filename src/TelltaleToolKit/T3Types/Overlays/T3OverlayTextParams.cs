using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Fonts;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Overlays;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OverlayTextParams>))]
public class T3OverlayTextParams
{
    [MetaMember("mhFont")]
    public Handle<Font> Font { get; set; } = new();

    [MetaMember("mhDlg")]
    public Handle<Dlg> Dlg { get; set; } = new();

    [MetaMember("mDlgNodeName")]
    public Symbol DlgNodeName { get; set; }

    [MetaMember("mText")]
    public string Text { get; set; } = string.Empty;

    [MetaMember("mInitialPosition")]
    public Vector2 InitialPosition { get; set; }
}
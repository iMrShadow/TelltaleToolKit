using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;

namespace TelltaleToolKit.T3Types.Miscellaneous.PreloadPackage;

[MetaSerializer(typeof(MetaClassSerializer<StartNodeOffset>))]
public class StartNodeOffset
{
    [MetaMember("mStartNodeChain")]
    public DlgObjId StartNodeChain { get; set; } = new();

    [MetaMember("fStartTimeSeconds")]
    public float StartTimeSeconds { get; set; }

    [MetaMember("mfMinDurationToPreload")]
    public float MinDurationToPreload { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgConditionTime>))]
public class DlgConditionTime : IDlgCondition
{
    [MetaSerializer(typeof(EnumSerializer<DurationClass>))]
    public enum DurationClass
    {
        Timed = 1,
        Indefinitely = 2
    }

    [MetaMember("mDurationClass")]
    public DurationClass DurationType { get; set; }

    [MetaMember("mSeconds")]
    public float Seconds { get; set; }

    [MetaMember("Baseclass_DlgCondition")]
    public DlgCondition DlgCondition { get; set; }
}

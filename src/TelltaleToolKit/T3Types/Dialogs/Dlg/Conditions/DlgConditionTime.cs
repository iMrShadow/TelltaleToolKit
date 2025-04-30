using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgConditionTime>))]
public class DlgConditionTime : IDlgCondition
{
    [MetaClassSerializerGlobal(typeof(EnumSerializer<DurationClass>))]
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
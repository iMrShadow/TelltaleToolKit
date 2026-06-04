using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<SoundEventNameBase>))]
public class SoundEventNameBase
{
    public enum NameType
    {
        Default = 0,
        Snapshot = 1,
        Dialog = 2
    }

    [MetaMember("mEventGuid")]
    public Symbol EventGuid { get; set; }


    [MetaMember("mEventDisplayName")]
    public Symbol EventDisplayName { get; set; }
}

// Small hack
// In C++ this is a compile-time type from an enum.
// In C# that is not possible - so we create the fixed classes.
[MetaSerializer(typeof(MetaClassSerializer<SoundEventName0>))]
public class SoundEventName0
{
    [MetaMember("Baseclass_SoundEventNameBase")]
    public SoundEventNameBase BaseclassSoundEventNameBase { get; set; } = new();

    public static SoundEventNameBase.NameType Type => SoundEventNameBase.NameType.Default;
}

[MetaSerializer(typeof(MetaClassSerializer<SoundEventName1>))]
public class SoundEventName1
{
    [MetaMember("Baseclass_SoundEventNameBase")]
    public SoundEventNameBase BaseclassSoundEventNameBase { get; set; } = new();

    public static SoundEventNameBase.NameType Type => SoundEventNameBase.NameType.Snapshot;
}

[MetaSerializer(typeof(MetaClassSerializer<SoundEventName2>))]
public class SoundEventName2
{
    [MetaMember("Baseclass_SoundEventNameBase")]
    public SoundEventNameBase BaseclassSoundEventNameBase { get; set; } = new();

    public static SoundEventNameBase.NameType Type => SoundEventNameBase.NameType.Dialog;
}

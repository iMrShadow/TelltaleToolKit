using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<SoundEventNameBase>))]
public class SoundEventNameBase
{
    [MetaMember("mEventGuid")]
    public Symbol EventGuid { get; set; }


    [MetaMember("mEventDisplayName")]
    public Symbol EventDisplayName { get; set; }

    public enum NameType
    {
        Default = 0,
        Snapshot = 1,
        Dialog = 2
    }
}

[MetaClassSerializerGlobal(typeof(Serializer))]

public class SoundEventName
{
    [MetaMember("Baseclass_SoundEventNameBase")]
    public SoundEventNameBase BaseclassSoundEventNameBase { get; set; } = new();

    public SoundEventNameBase.NameType Type { get; set; }

    public SoundEventName(SoundEventNameBase.NameType type)
    {
        Type = type;
    }

    public SoundEventName()
    {
        Type = SoundEventNameBase.NameType.Default;
    }

    public class Serializer : MetaClassSerializer<SoundEventName>
    {
        private static readonly DefaultClassSerializer<SoundEventName> DefaultSerializer = new();

        public override void PreSerialize(ref SoundEventName obj, MetaStream stream, MetaClassType? type = null)
        {
            ArgumentNullException.ThrowIfNull(type, "SoundEventName requires a metaclass type!");

            obj = type.Symbol.SymbolName switch
            {
                "SoundEventName<0>" => new SoundEventName(SoundEventNameBase.NameType.Default),
                "SoundEventName<1>" => new SoundEventName(SoundEventNameBase.NameType.Snapshot),
                "SoundEventName<2>" => new SoundEventName(SoundEventNameBase.NameType.Dialog),
                _ => obj
            };
        }

        public override void Serialize(ref SoundEventName obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);
        }
    }
}
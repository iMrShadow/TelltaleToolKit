using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterColorType>))]
public struct EnumEmitterColorType
{
    [MetaMember("mVal")]
    public EmitterColorType Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<EmitterColorType>))]
public enum EmitterColorType
{
    // TODO:
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterColorType>))]
public struct EnumEmitterColorType
{
    [MetaMember("mVal")]
    public EmitterColorType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterColorType>))]
public enum EmitterColorType
{
    // TODO:
}
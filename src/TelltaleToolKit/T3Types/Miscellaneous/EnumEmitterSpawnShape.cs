using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterSpawnShape>))]
public struct EnumEmitterSpawnShape
{
    [MetaMember("mVal")]
    public EmitterSpawnShape Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterSpawnShape>))]
public enum EmitterSpawnShape
{
    // TODO:
}

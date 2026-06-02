using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterSpawnShape>))]
public struct EnumEmitterSpawnShape
{
    [MetaMember("mVal")]
    public EmitterSpawnShape Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<EmitterSpawnShape>))]
public enum EmitterSpawnShape
{
    // TODO:
}

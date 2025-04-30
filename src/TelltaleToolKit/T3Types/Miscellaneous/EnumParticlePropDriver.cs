using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumParticlePropDriver>))]
public struct EnumParticlePropDriver
{
    [MetaMember("mVal")]
    public ParticlePropDriver Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<ParticlePropDriver>))]
public enum ParticlePropDriver
{
    // TODO:
}
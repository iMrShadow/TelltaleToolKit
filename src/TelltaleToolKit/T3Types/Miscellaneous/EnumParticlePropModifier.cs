using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumParticlePropModifier>))]
public struct EnumParticlePropModifier
{
    [MetaMember("mVal")]
    public ParticlePropModifier Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<ParticlePropModifier>))]
public enum ParticlePropModifier
{
    // TODO:
}
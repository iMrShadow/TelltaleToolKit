using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterParticleCountType>))]
public struct EnumEmitterParticleCountType
{
    [MetaMember("mVal")]
    public EmitterParticleCountType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterParticleCountType>))]
public enum EmitterParticleCountType
{
    // TODO:
}

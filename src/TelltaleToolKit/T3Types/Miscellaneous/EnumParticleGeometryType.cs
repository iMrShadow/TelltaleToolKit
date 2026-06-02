using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumParticleGeometryType>))]
public struct EnumParticleGeometryType
{
    [MetaMember("mVal")]
    public ParticleGeometryType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<ParticleGeometryType>))]
public enum ParticleGeometryType
{
    //       eParticleGeometry_
    Sprite = 1,
    Quad = 2,
    Streak = 3,
    Strip = 4,
    StripFacing = 5,

    None = 6
    // TODO: Verify if non is really number 6. I have a feeling it's 0.
}

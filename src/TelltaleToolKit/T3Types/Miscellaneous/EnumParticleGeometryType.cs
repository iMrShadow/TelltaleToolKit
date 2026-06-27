using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumParticleGeometryType>))]
public struct EnumParticleGeometryType
{
    [MetaMember("mVal")]
    public ParticleGeometryType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<ParticleGeometryType>))]
    public enum ParticleGeometryType
    {
        Sprite = 1,
        Quad = 2,
        Streak = 3,
        Strip = 4,
        StripFacing = 5,
        None = 6
    }
}

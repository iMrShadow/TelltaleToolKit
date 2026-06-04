using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumParticleAffectorType>))]
public struct EnumParticleAffectorType
{
    [MetaMember("mVal")]
    public ParticleAffectorType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<ParticleAffectorType>))]
    public enum ParticleAffectorType
    {
        Force = 1,
        Attractor = 2,
        KillPlane = 3,
        KillBox = 4,
        CollisionPlane = 5,
        CollisionSphere = 6,
        CollisionBox = 7,
        CollisionCylinder = 8
    }
}

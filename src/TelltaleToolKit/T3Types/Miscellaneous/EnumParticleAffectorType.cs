using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumParticleAffectorType>))]
public struct EnumParticleAffectorType
{
    [MetaMember("mVal")]
    public ParticleAffectorType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<ParticleAffectorType>))]
public enum ParticleAffectorType
{
    //   eParticleAffectorType_
    Force = 1,
    Attractor = 2,
    KillPlane = 3,
    KillBox = 4,
    CollisionPlane = 5,
    CollisionSphere = 6,
    CollisionBox = 7,
    CollisionCylinder = 8,
}

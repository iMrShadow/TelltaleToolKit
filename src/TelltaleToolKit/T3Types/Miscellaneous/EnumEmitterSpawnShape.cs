using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterSpawnShape>))]
public struct EnumEmitterSpawnShape
{
    [MetaMember("mVal")]
    public EmitterSpawnShape Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EmitterSpawnShape>))]
    public enum EmitterSpawnShape
    {
        Box = 0x1,
        Sphere = 0x2,
        Cylinder = 0x3,
        ToTarget = 0x4,
        Particle = 0x5,
        ParticleInterpolate = 0x6,
        Bones = 0x7,
        BoneBoxes = 0x8
    }
}

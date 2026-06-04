using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumParticlePropDriver>))]
public struct EnumParticlePropDriver
{
    [MetaMember("mVal")]
    public ParticlePropDriver Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<ParticlePropDriver>))]
    public enum ParticlePropDriver
    {
        EmitterSpeed = 0x1,
        DistanceToTarget = 0x2,
        BurstTime = 0x3,
        CameraDot = 0x4,
        KeyControl01 = 0x5,
        KeyControl02 = 0x6,
        KeyControl03 = 0x7,
        KeyControl04 = 0x8,
        DistanceToCamera = 0x9
    }
}

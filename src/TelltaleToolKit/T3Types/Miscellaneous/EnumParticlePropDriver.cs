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
    ePartPropDriver_EmitterSpeed = 0x1,
    ePartPropDriver_DistanceToTarget = 0x2,
    ePartPropDriver_BurstTime = 0x3,
    ePartPropDriver_CameraDot = 0x4,
    ePartPropDriver_KeyControl01 = 0x5,
    ePartPropDriver_KeyControl02 = 0x6,
    ePartPropDriver_KeyControl03 = 0x7,
    ePartPropDriver_KeyControl04 = 0x8,
    ePartPropDriver_DistanceToCamera = 0x9,
}
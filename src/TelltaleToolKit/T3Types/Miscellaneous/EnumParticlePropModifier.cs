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
    ePartPropModifier_Constraint_Length = 0x0,
    ePartPropModifier_Effect_Scale = 0x1,
    ePartPropModifier_Geometry_Turbulence = 0x2,
    ePartPropModifier_Global_Alpha = 0x3,
    ePartPropModifier_Global_Acceleration = 0x4,
    ePartPropModifier_Max_Particles = 0x5,
    ePartPropModifier_PP_Scale = 0x6,
    ePartPropModifier_PP_Lifespan = 0x7,
    ePartPropModifier_PP_Rotation = 0x8,
    ePartPropModifier_PP_RotationSpeed = 0x9,
    ePartPropModifier_PP_Speed = 0xA,
    ePartPropModifier_PP_Intensity = 0xB,
    ePartPropModifier_Time_Scale = 0xC,
    ePartPropModifier_Sprite_Animation_Rate = 0xD,
    ePartPropModifier_Sprite_Animation_Cycles = 0xE,
    ePartPropModifier_Spawn_Angle = 0xF,
    ePartPropModifier_Spawn_Volume_Sweep = 0x10,
    ePartPropModifier_Spawn_Volume_Sweep_Offset = 0x11,
    ePartPropModifier_Target_Render_Lerp = 0x12,
    ePartPropModifier_Velocity_Turbulence_Force = 0x13,
    ePartPropModifier_Velocity_Turbulence_Speed = 0x14,
    ePartPropModifier_Velocity_Timescale_Modifier = 0x15,
    ePartPropModifier_KeyControl01 = 0x16,
    ePartPropModifier_KeyControl02 = 0x17,
    ePartPropModifier_KeyControl03 = 0x18,
    ePartPropModifier_KeyControl04 = 0x19,
    ePartPropModifier_PP_Alpha = 0x1A,
    ePartPropModifier_Geometry_Scale = 0x1B,
    ePartPropModifier_Enable = 0x1C,
    ePartPropModifier_Count = 0x1D,
}
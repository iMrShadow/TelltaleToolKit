using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumParticlePropModifier>))]
public struct EnumParticlePropModifier
{
    [MetaMember("mVal")]
    public ParticlePropModifier Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<ParticlePropModifier>))]
    public enum ParticlePropModifier
    {
        // TODO: Dynamic Flags
        Constraint_Length = 0x0,
        Effect_Scale = 0x1,
        Geometry_Turbulence = 0x2,
        Global_Alpha = 0x3,
        Global_Acceleration = 0x4,
        Max_Particles = 0x5,
        PP_Scale = 0x6,
        PP_Lifespan = 0x7,
        PP_Rotation = 0x8,
        PP_RotationSpeed = 0x9,
        PP_Speed = 0xA,
        PP_Intensity = 0xB,
        Time_Scale = 0xC,
        Sprite_Animation_Rate = 0xD,
        Sprite_Animation_Cycles = 0xE,
        Spawn_Angle = 0xF,
        Spawn_Volume_Sweep = 0x10,
        Spawn_Volume_Sweep_Offset = 0x11,
        Target_Render_Lerp = 0x12,
        Velocity_Turbulence_Force = 0x13,
        Velocity_Turbulence_Speed = 0x14,
        Velocity_Timescale_Modifier = 0x15,
        KeyControl01 = 0x16,
        KeyControl02 = 0x17,
        KeyControl03 = 0x18,
        KeyControl04 = 0x19,
        PP_Alpha = 0x1A,
        Geometry_Scale = 0x1B,
        Enable = 0x1C,
        Count = 0x1D
    }
}

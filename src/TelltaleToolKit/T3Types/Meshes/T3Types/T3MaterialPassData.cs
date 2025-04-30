using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialPassData>))]
public class T3MaterialPassData
{
    [MetaMember("mPassType")]
    public T3MaterialPassType PassType { get; set; }

    [MetaMember("mBlendMode")]
    public T3BlendMode BlendMode { get; set; }

    [MetaMember("mMaterialCrc")]
    public ulong MaterialCrc { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialPassType>))]
public enum T3MaterialPassType
{
    // 
    FirstMesh = 0x0,
    Main = 0x0,
    PreZ = 0x1,
    GBuffer = 0x2,
    GBuffer_Lines = 0x3,
    GBuffer_Outline = 0x4,
    Glow = 0x5,
    ShadowMap = 0x6,
    Outline = 0x7,
    LinearDepth = 0x8,
    LinesCS_Generate = 0x9,
    LinesCS_Rasterize = 0xA,
    LinesCS_ForceLinearDepth = 0xB,
    LastMesh = 0xB,
    FirstParticle = 0xC,
    ParticleMain = 0xC,
    ParticleGlow = 0xD,
    ParticleLinearDepth = 0xE,
    LastParticle = 0xE,
    FirstDecal = 0xF,
    DecalGBuffer = 0xF,
    DecalEmissive = 0x10,
    DecalGlow = 0x11,
    LastDecal = 0x11,
    Post = 0x12,
    MayaShader = 0x13,
    LightBake = 0x14,
    Count = 0x15,
    MeshCount = 0xC,
    ParticleCount = 0x3,
    DecalCount = 0x3,
}
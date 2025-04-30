using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshCPUSkinningData>))]
public class T3MeshCPUSkinningData
{
    [MetaMember("mPositionFormat")]
    public GFXPlatformFormat PositionFormat { get; set; }

    [MetaMember("mNormalFormat")]
    public GFXPlatformFormat NormalFormat { get; set; }

    [MetaMember("mWeightFormat")]
    public GFXPlatformFormat WeightFormat { get; set; }

    [MetaMember("mVertexStreams")]
    public BitSetBase VertexStreams { get; set; }

    [MetaMember("mNormalCount")]
    public int NormalCount { get; set; }

    [MetaMember("mWeightOffset")]
    public int WeightOffset { get; set; }

    [MetaMember("mVertexSize")]
    public int VertexSize { get; set; }

    [MetaMember("mWeightSize")]
    public int WeightSize { get; set; }

    [MetaMember("mData")]
    public BinaryBuffer Data { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<GFXPlatformFormat>))]
public enum GFXPlatformFormat
{
    //  eGFXPlatformFormat_
    None = 0x0,
    F32 = 0x1,
    F32x2 = 0x2,
    F32x3 = 0x3,
    F32x4 = 0x4,
    F16x2 = 0x5,
    F16x4 = 0x6,
    S32 = 0x7,
    U32 = 0x8,
    S32x2 = 0x9,
    U32x2 = 0xA,
    S32x3 = 0xB,
    U32x3 = 0xC,
    S32x4 = 0xD,
    U32x4 = 0xE,
    S16 = 0xF,
    U16 = 0x10,
    S16x2 = 0x11,
    U16x2 = 0x12,
    S16x4 = 0x13,
    U16x4 = 0x14,
    SN16 = 0x15,
    UN16 = 0x16,
    SN16x2 = 0x17,
    UN16x2 = 0x18,
    SN16x4 = 0x19,
    UN16x4 = 0x1A,
    S8 = 0x1B,
    U8 = 0x1C,
    S8x2 = 0x1D,
    U8x2 = 0x1E,
    S8x4 = 0x1F,
    U8x4 = 0x20,
    SN8 = 0x21,
    UN8 = 0x22,
    SN8x2 = 0x23,
    UN8x2 = 0x24,
    SN8x4 = 0x25,
    UN8x4 = 0x26,
    SN10_SN11_SN11 = 0x27,
    SN10x3_SN2 = 0x28,
    UN10x3_UN2 = 0x29,
    D3DCOLOR = 0x2A,
}
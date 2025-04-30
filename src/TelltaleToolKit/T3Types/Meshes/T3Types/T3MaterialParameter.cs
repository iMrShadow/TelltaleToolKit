using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialParameter>))]
public class T3MaterialParameter
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mPropertyType")]
    public T3MaterialPropertyType PropertyType { get; set; }

    [MetaMember("mValueType")]
    public T3MaterialValueType ValueType { get; set; }

    [MetaMember("mFlags")]
    public uint Flags { get; set; }

    [MetaMember("mScalarOffset")]
    public int ScalarOffset { get; set; }
    
    [MetaMember("mScalarOffset[0]")]
    public int ScalarOffset0 { get; set; }
    
    [MetaMember("mScalarOffset[1]")]
    public int ScalarOffset1{ get; set; }
    
    [MetaMember("mScalarOffset[2]")]
    public int ScalarOffset2{ get; set; }
    
    [MetaMember("mScalarOffset[3]")]
    public int ScalarOffset3 { get; set; }

    [MetaMember("mPreShaderScalarOffset")]
    public int PreShaderScalarOffset { get; set; }

    [MetaMember("mNestedMaterialIndex")]
    public int NestedMaterialIndex { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialValueType>))]
public enum T3MaterialValueType
{
    //  eMaterialValue_
    None = -1,
    Float = 0x0,
    Float2 = 0x1,
    Float3 = 0x2,
    Float4 = 0x3,
    Channels = 0x4,
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialPropertyType>))]
public enum T3MaterialPropertyType
{
    // eMaterialProperty_
    None = -1,
    GlossExponent = 0x0,
    GlowIntensity = 0x1,
    SpecularPower = 0x2,
    OutlineColor = 0x3,
    OutlineInvertColor = 0x4,
    OutlineZRange = 0x5,
    ToonGradientCutoff = 0x6,
    LineGenerateCreases = 0x7,
    LineGenerateBoundaries = 0x8,
    LineCreaseAngle = 0x9,
    LineSmoothJaggedCreaseAngle = 0xA,
    LineGenerateSmooth = 0xB,
    LineGenerateJagged = 0xC,
    LineMinWidth = 0xD,
    LineMaxWidth = 0xE,
    LineWidthFromLighting = 0xF,
    LineLightingType = 0x10,
    LineLightingId = 0x11,
    LinePatternRepeat = 0x12,
    LitLineBias = 0x13,
    LitLineScale = 0x14,
    ConformNormal = 0x15,
    NPRLineFalloff = 0x16,
    NPRLineAlphaFalloff = 0x17,
    DrawHiddenLines = 0x18,
    AlphaMeshCullsLines = 0x19,
    UseArtistNormal = 0x1A,
    HorizonFade = 0x1B,
    HairTerms = 0x1C,
    ClothOffsetScale = 0x1D,
    LightWrap = 0x1E,
}
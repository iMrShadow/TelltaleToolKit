using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialCompiledData>))]
public class T3MaterialCompiledData
{
    [MetaMember("mParameters")]
    public List<T3MaterialParameter> Parameters { get; set; }

    [MetaMember("mTextures")]
    public List<T3MaterialTexture> Textures { get; set; }

    [MetaMember("mTransforms")]
    public List<T3MaterialTransform2D> Transforms { get; set; }

    [MetaMember("mNestedMaterials")]
    public List<T3MaterialNestedMaterial> NestedMaterials { get; set; }

    [MetaMember("mPreShaders")]
    public List<T3MaterialPreShader> PreShaders { get; set; }

    [MetaMember("mStaticParameters")]
    public List<T3MaterialStaticParameter> StaticParameters { get; set; }

    [MetaMember("mTextureParams")]
    public List<T3MaterialTextureParam> TextureParams { get; set; }

    [MetaMember("mPasses")]
    public List<T3MaterialPassData> Passes { get; set; }

    [MetaMember("mMaterialQuality")]
    public T3MaterialQualityType MaterialQuality { get; set; }

    [MetaMember("mMaterialBlendModes")]
    public BitSetBase MaterialBlendModes { get; set; }

    [MetaMember("mMaterialPasses")]
    public BitSetBase MaterialPasses { get; set; }

    [MetaMember("mMaterialChannels")]
    public BitSetBase MaterialChannels { get; set; }

    // [MetaMember("mMaterialChannels2")]
    // public BitSetBase MaterialChannels2 { get; set; } = new(32);

    // [MetaMember("mMaterialChannels")]
    // public BitSet<T3MaterialChannelType> MaterialChannels { get; set; } = new(46);
    //
    // [MetaMember("mMaterialChannels2")]
    // public BitSet<T3MaterialChannelType> MaterialChannels2 { get; set; } = new(32);

    // HOLD ON Let me cook
    [MetaMember("mShaderInputs")]
    public BitSetBase ShaderInputs { get; set; }

    [MetaMember("mShaderInputs2")]
    public BitSetBase ShaderInputs2 { get; set; }

    [MetaMember("mShaderInputs3")]
    public BitSetBase ShaderInputs3 { get; set; }

    [MetaMember("mSceneTextures")]
    public BitSetBase SceneTextures { get; set; }

    [MetaMember("mOptionalPropertyTypes")]
    public BitSetBase OptionalPropertyTypes { get; set; }

    [MetaMember("mPreShaderBuffer")]
    public BinaryBuffer PreShaderBuffer { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mPropertyParameterIndex")]
    public int[] PropertyParameterIndex { get; set; } = new int[31];

    [MetaMember("mTexturePropertyParameterIndex")]
    public int[] TexturePropertyParameterIndex { get; set; } = new int[1];

    [MetaMember("mParameterBufferScalarSize[0]")]
    public uint ParameterBufferScalarSize0 { get; set; }

    [MetaMember("mParameterBufferScalarSize[1]")]
    public uint ParameterBufferScalarSize1 { get; set; }

    [MetaMember("mPreShaderParameterBufferScalarSize")]
    public uint PreShaderParameterBufferScalarSize { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialChannelType>))]
public enum T3MaterialChannelType
{
    // eMaterialChannel_
    None = -1,
    SurfaceNormal = 0x0,
    DiffuseColor = 0x1,
    SpecularColor = 0x2,
    EmissiveColor = 0x3,
    VertexNormal = 0x4,
    InvertShadow = 0x5,
    Alpha_Legacy = 0x6,
    Gloss = 0x7,
    AmbientOcclusion = 0x8,
    Glow = 0x9,
    OutlineSize = 0xA,
    VertexOffset = 0xB,
    VertexColor0 = 0xC,
    VertexColor1 = 0xD,
    TexCoord0 = 0xE,
    TexCoord1 = 0xF,
    TexCoord2 = 0x10,
    TexCoord3 = 0x11,
    NPR_Hatching = 0x18,
    LineColor = 0x19,
    LineVisibility = 0x1A,
    LineGenerationStyle = 0x1B,
    LineWidth = 0x1C,
    Reserved0 = 0x1C,
    Reserved1 = 0x1D,
    Reserved2 = 0x1E,
    Reserved3 = 0x1F,
    Reserved4 = 0x20,
    DetailColor = 0x21,
    DetailAlpha = 0x22,
    DiffuseAlbedoColor = 0x23,
    FinalColor = 0x24,
    LineAlpha = 0x25,
    Opacity = 0x26,
    OpacityMask = 0x27,
    SpecularAlbedoColor = 0x28,
    Anisotropy = 0x29,
    SmoothSurfaceNormal = 0x2A,
    AnisotropyMask = 0x2B,
    AnisotropyTangent = 0x2C,
    SecondarySpecularAlbedoColor = 0x2D,
    Count = 0x2E,
    Custom = 0xFFFF,
    VertexColorCount = 0x2,
    TexCoordCount = 0x4,
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3MaterialQualityType>))]
public enum T3MaterialQualityType
{
}
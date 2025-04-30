using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Textures.T3Types;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.T3Types.Textures;

[MetaClassSerializerGlobal(typeof(Serializer))]
public partial class T3Texture
{
    // Most members here are before Poker Night 2
    /// <summary>
    /// The name of the texture.
    /// </summary>
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The import name of the texture. (The original file used for this texture.)
    /// </summary>
    [MetaMember("mImportName")]
    public string ImportName { get; set; } = string.Empty;

    [MetaMember("mbHasTextureData")]
    public bool HasTextureData { get; set; } = false;

    [MetaMember("mbIsMipMapped")]
    public bool IsMipMapped { get; set; } = false;

    [MetaMember("mbIsWrapU")]
    public bool IsWrapU { get; set; } = false;

    [MetaMember("mbIsWrapV")]
    public bool IsWrapV { get; set; } = false;

    [MetaMember("mbIsFiltered")]
    public bool IsFiltered { get; set; } = false;

    [MetaMember("mbEmbedMipMaps")]
    public bool EmbedMipMaps { get; set; } = false;

    /// <summary>
    /// The number of mip map levels in the texture.
    /// </summary>
    [MetaMember("mNumMipLevels")]
    public uint NumMipLevels { get; set; } = 0;

    [MetaMember("mD3DFormat")]
    public uint D3DFormat { get; set; } = 0;

    /// <summary>
    /// The pixel width of the texture.
    /// </summary>
    [MetaMember("mWidth")]
    public uint Width { get; set; } = 0;

    /// <summary>
    /// The pixel height of the texture.
    /// </summary>
    [MetaMember("mHeight")]
    public uint Height { get; set; } = 0;

    [MetaMember("mType")]
    public int Type { get; set; } = 0;

    [MetaMember("mTextureDataFormats")]
    public Flags TextureDataFormats { get; set; } = new Flags();

    [MetaMember("mTplTexutreDataSize")]
    public int TplTextureDataSize { get; set; } = 0;

    [MetaMember("mAlphaMode")]
    public int AlphaMode { get; set; } = 0;

    [MetaMember("mbAlphaHDR")]
    public bool AlphaHdr { get; set; } = false;

    [MetaMember("mbEncrypted")]
    public bool IsEncrypted { get; protected set; }

    [MetaMember("mbUsedAsBumpmap")]
    public bool UsedAsBumpmap { get; set; } = false;

    [MetaMember("mbUsedAsDetailMap")]
    public bool UsedAsDetailMap { get; set; } = false;

    [MetaMember("mDetailMapBrightness")]
    public float DetailMapBrightness { get; set; }


    public byte[] DdsTextureData { get; set; } = [];

    public byte[] TplTextureData { get; set; } = [];
    // =========================

    /// <summary>
    /// The meta version of this texture.
    /// </summary>
    [MetaMember("mVersion")]
    public int Version { get; set; }

    /// <summary>
    /// The sampler state, bitflag value that contains values from T3SamplerStateValue.
    /// </summary>
    [MetaMember("mSamplerState")]
    public T3SamplerStateBlock SamplerStateBlock { get; set; } = new();

    /// <summary>
    /// The platform type for this texture.
    /// </summary>
    [MetaMember("mPlatform")]
    public EnumPlatformType PlatformType { get; set; } = new();

    /// <summary>
    /// The import scale of the texture file.
    /// </summary>
    [MetaMember("mImportScale")]
    public float ImportScale { get; set; }

    /// <summary>
    /// The import specular power of the texture;
    /// </summary>
    [MetaMember("mImportSpecularPower")]
    public float ImportSpecularPower { get; set; }

    /// <summary>
    /// Whether or not the d3dtx texture contains properties. This was used for their runtime editor. (Always false)
    /// </summary>
    [MetaMember("mToolProps")]
    public ToolProps ToolProps { get; set; } = new();

    /// <summary>
    /// The depth of a volume texture.
    /// </summary>
    [MetaMember("mDepth")]
    public uint Depth { get; set; }

    /// <summary>
    /// The array size of this texture.
    /// </summary>
    [MetaMember("mArraySize")]
    public uint ArraySize { get; set; }

    /// <summary>
    /// The pixel format for this texture.
    /// </summary>
    [MetaMember("mSurfaceFormat")]
    public T3SurfaceFormat SurfaceFormat { get; set; }

    /// <summary>
    /// The texture layout for this texture.
    /// </summary>
    [MetaMember("mTextureLayout")]
    public T3TextureLayout TextureLayout { get; set; }

    /// <summary>
    /// The gamma of the texture.
    /// </summary>
    [MetaMember("mSurfaceGamma")]
    public T3SurfaceGamma SurfaceGamma { get; set; }

    /// <summary>
    /// The multisample (anisotropic) level of the texture.
    /// </summary>
    [MetaMember("mSurfaceMultisample")]
    public T3SurfaceMultisample SurfaceMultisample { get; set; }

    /// <summary>
    /// The resource usage of the texture.
    /// </summary>
    [MetaMember("mResourceUsage")]
    public T3ResourceUsage ResourceUsage { get; set; }

    /// <summary>
    /// The type of the texture. The enum version is for modern games.
    /// </summary>
    [MetaMember("mType")]
    public TextureType TypeEnum { get; set; }

    /// <summary>
    /// [4 bytes] Defines the format of the normal map.
    /// </summary>
    [MetaMember("mNormalMapFormat")]
    public int NormalMapFormat { get; set; }

    /// <summary>
    /// Describes if the color channels were switched. (Usually used for normal maps, console textures and others.)
    /// </summary>
    [MetaMember("mSwizzle")]
    public RenderSwizzleParams Swizzle { get; set; }

    /// <summary>
    /// The glossiness of the texture.
    /// </summary>
    [MetaMember("mSpecularGlossExponent")]
    public float SpecularGlossExponent { get; set; }

    /// <summary>
    /// The brightness scale of the texture. (used for lightmaps)
    /// </summary>
    [MetaMember("mHDRLightmapScale")]
    public float HdrLightmapScale { get; set; }

    /// <summary>
    /// The toon cutoff gradient of the texture.
    /// </summary>
    [MetaMember("mToonGradientCutoff")]
    public float ToonGradientCutoff { get; set; }

    /// <summary>
    /// The alpha type of the texture. (or what will have).
    /// </summary>
    [MetaMember("mAlphaMode")]
    public AlphaMode AlphaModeEnum { get; set; }

    /// <summary>
    /// The color range of the texture.
    /// </summary>
    [MetaMember("mColorMode")]
    public ColorMode ColorMode { get; set; }

    /// <summary>
    /// A vector, defines the UV offset values when the shader on a material samples the texture.
    /// </summary>
    [MetaMember("mUVOffset")]
    public Vector2 UvOffset { get; set; }

    /// <summary>
    /// A vector, defines the UV scale values when the shader on a material samples the texture.
    /// </summary>
    [MetaMember("mUVScale")]
    public Vector2 UvScale { get; set; }

    /// <summary>
    /// An array containing frame names. (Usually unused)
    /// </summary>
    [MetaMember("mArrayFrameNames")]
    public List<Symbol> ArrayFrameNames { get; set; } = [];

    /// <summary>
    /// An array containing a toon gradient region. (Usually unused)
    /// </summary>
    [MetaMember("mToonRegions")]
    public List<T3ToonGradientRegion> ToonRegions { get; set; } = [];


    public StreamHeader RegionMainHeader { get; set; }


    /// <summary>
    /// An array containing each pixel region in the texture.
    /// </summary>
    public List<RegionStreamHeader> RegionHeaders { get; set; } = [];


    public List<AuxiliaryData> AuxilaryData { get; set; } = [];

    private byte[] ConsoleData { get; set; } = []; // PVR, PS3, Xbox 360, etc.
    private byte[] WiiColorData { get; set; } = []; // Wii color data.
    private byte[] WiiAlphaData { get; set; } = []; // Wii alpha data.
    private byte[] JpegData { get; set; } = []; // JPEG data. This is always nill.

    // public ClassInstance? T3Texture { get; set; } // The main header of d3dtx.
    // public ClassInstance? StreamHeader { get; set; } // The stream header (appears after Poker Night 2).
    // List<ClassInstance>? Regions { get; set; } // The regions of the texture (appears after Poker Night 2).
    // List<PrimitiveInstance>? AuxData { get; set; } // The auxiliary data (appears after Poker Night 2).

    // This is strict to legacy d3dtx versions.


    public void Decrypt(Blowfish blowfish)
    {
        if (!IsEncrypted)
            return;

        const int maxEncryptedTextureBufferSize = 2048;
        int bytesToDecipher = Math.Min(maxEncryptedTextureBufferSize, DdsTextureData.Length);
        blowfish.Decipher(DdsTextureData, bytesToDecipher);
        IsEncrypted = false;
    }

    public void Encrypt(Blowfish blowfish)
    {
        if (IsEncrypted)
            return;

        const int maxEncryptedTextureBufferSize = 2048;
        int bytesToDecipher = Math.Min(maxEncryptedTextureBufferSize, DdsTextureData.Length);
        blowfish.Encipher(DdsTextureData, bytesToDecipher);
        IsEncrypted = true;
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<StreamHeader>))]
    public struct StreamHeader
    {
        [MetaMember("mRegionCount")]
        public int RegionCount { get; set; }

        [MetaMember("mAuxDataCount")]
        public int AuxDataCount { get; set; }

        [MetaMember("mTotalDataSize")]
        public int TotalDataSize { get; set; }
    }


    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<RegionStreamHeader>))]
    public class RegionStreamHeader
    {
        // mVersion >= 5
        [MetaMember("mFaceIndex")]
        public int FaceIndex { get; set; }

        // mVersion >= 3
        [MetaMember("mMipIndex")]
        public int MipIndex { get; set; }

        // mVersion >= 4
        [MetaMember("mMipCount")]
        public int MipCount { get; set; }

        // mVersion >= 3
        [MetaMember("mDataSize")]
        public int DataSize { get; set; }

        // mVersion >= 3
        [MetaMember("mPitch")]
        public int Pitch { get; set; }

        // mVersion >= 9
        [MetaMember("mSlicePitch")]
        public int SlicePitch { get; set; }

        public byte[] RegionData { get; set; } = [];
    }

    public class Serializer : MetaClassSerializer<T3Texture>
    {
        private static readonly DefaultClassSerializer<T3Texture> DefaultT3TextureSerializer = new();

        public override void PreSerialize(ref T3Texture obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new T3Texture();
        }

        public override void Serialize(ref T3Texture obj, MetaStream stream)
        {
            // Default Serializer
            DefaultT3TextureSerializer.Serialize(ref obj, stream);

            // The default serializer will throw if there is no serialized T3Texture.
            MetaClass classDescription = stream.GetMetaClass(typeof(T3Texture))!;

            if (stream is MetaStreamWriter streamWriter)
            {
                if (classDescription.ContainsMember("mVersion"))
                {
                    int totalSize = obj.RegionHeaders.Sum(region => region.DataSize);

                    obj.RegionMainHeader = new StreamHeader
                    {
                        TotalDataSize = totalSize,
                        RegionCount = obj.RegionHeaders.Count,
                        // We won't be adding any aux data into the game, it's entirely pointless.
                        AuxDataCount = 0
                    };

                    StreamHeader streamHeader = obj.RegionMainHeader;
                    TTKContext.Instance().GetSerializer<StreamHeader>().Serialize(ref streamHeader, stream);

                    foreach (RegionStreamHeader region in obj.RegionHeaders)
                    {
                        RegionStreamHeader regionStreamHeader = region;
                        TTKContext.Instance().GetSerializer<RegionStreamHeader>()
                            .Serialize(ref regionStreamHeader, stream);
                    }

                    // One would assume that we can combine the loops into one.
                    // No...no we cannot.
                    // In MTRE streams there is no async section, but the region headers are still serialized before the pixel data.
                    // This is also accurate in the serialize function.

                    stream.BeginAsyncSection();
                    foreach (RegionStreamHeader region in obj.RegionHeaders)
                    {
                        streamWriter.Write(region.RegionData);
                    }

                    stream.EndAsyncSection();
                }
                else
                {
                    streamWriter.Write(obj.DdsTextureData.Length);
                    streamWriter.Write(obj.DdsTextureData);

                    if (obj.TplTextureDataSize > 0)
                    {
                        streamWriter.Write(obj.TplTextureData);
                    }
                }
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // This is easier than checking for region stream headers. That can also work.
                if (classDescription.ContainsMember("mVersion"))
                {
                    StreamHeader streamHeader = obj.RegionMainHeader;
                    TTKContext.Instance().GetSerializer<StreamHeader>()?.Serialize(ref streamHeader, stream);
                    obj.RegionMainHeader = streamHeader;

                    obj.RegionHeaders = new List<RegionStreamHeader>(streamHeader.RegionCount);

                    for (var i = 0; i < streamHeader.RegionCount; i++)
                    {
                        var regionStreamHeader = new RegionStreamHeader();
                        TTKContext.Instance().GetSerializer<RegionStreamHeader>()
                            .Serialize(ref regionStreamHeader, stream);
                        obj.RegionHeaders.Add(regionStreamHeader);
                    }

                    // Aux data is a little weird. Please check the
                    if (obj.RegionMainHeader.AuxDataCount > 0)
                    {
                        obj.AuxilaryData = new List<AuxiliaryData>(streamHeader.AuxDataCount);

                        stream.BeginBlock();
                        for (var i = 0; i < obj.RegionMainHeader.AuxDataCount; i++)
                        {
                            var aux = new AuxiliaryData();
                            TTKContext.Instance().GetSerializer<AuxiliaryData>().Serialize(ref aux, stream);
                            obj.AuxilaryData.Add(aux);
                        }

                        stream.EndBlock();
                    }

                    stream.BeginAsyncSection();
                    foreach (RegionStreamHeader region in obj.RegionHeaders)
                    {
                        if (region.MipCount <= 0)
                        {
                            region.MipCount = 1;
                        }

                        if (region.SlicePitch <= 0)
                        {
                            region.SlicePitch = region.DataSize;
                        }

                        region.RegionData = streamReader.ReadBytes(region.DataSize);
                    }

                    stream.EndAsyncSection();
                }
                else
                {
                    if (!obj.HasTextureData)
                    {
                        return;
                    }

                    int bufferSize = streamReader.ReadInt32();
                    obj.DdsTextureData = streamReader.ReadBytes(bufferSize);

                    if (obj.TplTextureDataSize > 0)
                    {
                        obj.TplTextureData = new byte[obj.TplTextureDataSize];

                        obj.TplTextureData = streamReader.ReadBytes(obj.TplTextureDataSize);
                    }
                }
            }
        }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AuxiliaryData>))]
    public class AuxiliaryData
    {
        [MetaMember("mType")]
        public Symbol Type { get; set; } = new(0);

        // This is officially the first serializing hack.
        // The T3Texture::AuxiliaryData is usually not loaded into any game memory, including Poker Night 2.
        // It does exist as a MetaClass in MSV5 and MSV6 games, but I can't find it in PK2, it may be corrupted, not loaded or totally not shipped.
        // Interestingly enough, Poker Night 2 is the only MTRE game which uses the new texture system, including the aux system.
        // In an April 2013 build (pre-release?), there are 2 textures which contain such data:
        // 'adv_celebritypokerroom_meshesa_000.d3dtx' and 'adv_celebritypokerroom_meshesg_000.d3dtx'
        // The data they contain is just an extra PNG per each texture.
        // I do not know if these D3DTXs are loaded successfully since in the serializing function there is no reference to aux data.
        // But clearly they do exist.
        // As a hack for the MTRE version, which has an extra field I presume, I added an artificial member in the PK2 vdb.
        // The type of the member is definitely not 'class Flags', otherwise it would have been serialized in the header too.
        [MetaMember("mUnknown")]
        public int Unknown { get; set; }

        [MetaMember("mData")]
        public BinaryBuffer Data { get; set; } = new();
    }
}
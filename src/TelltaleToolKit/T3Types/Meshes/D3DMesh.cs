using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Meshes.T3Types;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Meshes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<D3DMesh>))]
public class D3DMesh
{
    /// <summary>
    /// The version of the mesh. This property does not exist in earlier games.
    /// </summary>
    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mToolProps")]
    public ToolProps ToolProps { get; set; } = new();

    [MetaMember("mLightmapGlobalScale")]
    public float LightmapGlobalScale { get; set; } = 1.0f;

    [MetaMember("mLightmapTexCoordVersion")]
    public int LightmapTexCoordVersion { get; set; }

    [MetaMember("mLODParamCRC")]
    public ulong LodParamCrc { get; set; }


    // Some of the fields below are for older versions.

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mLightmapUVGenerationType")]
    public int mLightmapUVGenerationType { get; set; }

    [MetaMember("mLightmapTexelAreaPerSurfaceArea")]
    public float LightmapTexelAreaPerSurfaceArea { get; set; }

    [MetaMember("mLightmapTextureWidth")]
    public uint mLightmapTextureWidth { get; set; }

    [MetaMember("mLightmapTextureHeight")]
    public uint mLightmapTextureHeight { get; set; }

    [MetaMember("mbDeformable")]
    public bool Deformable { get; set; }

    [MetaMember("mTriangleSets")]
    public List<TriangleSet> TriangleSets { get; set; } = [];

    [MetaMember("mBonePalettes")]
    public List<List<PaletteEntry>> BonePalettes { get; set; } = [];

    [MetaMember("mbLightmaps")]
    public bool Lightmaps { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; } = new();

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; } = new();

    [MetaMember("mbLowQualityRender")]
    public bool LowQualityRender { get; set; }

    [MetaMember("mbDontTriStrip")]
    public bool DontTriStrip { get; set; }

    [MetaMember("mVertexAnimations")]
    public List<VertexAnimation> VertexAnimations { get; set; } = [];

    [MetaMember("mbVertexAlphaSupport")]
    public bool VertexAlphaSupport { get; set; }

    [MetaMember("mbMeshHasVertexAlpha")]
    public bool MeshHasVertexAlpha { get; set; }

    [MetaMember("mSkinningData")]
    public List<SkinningEntry> SkinningData { get; set; }

    [MetaMember("mBoneData")]
    public List<BoneEntry> BoneData { get; set; }

    [MetaMember("mLocalTransformPalettes")]
    public List<List<LocalTransformEntry>> LocalTransformPalettes { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LocalTransformEntry>))]
    public class LocalTransformEntry
    {
    }


    [MetaMember("mTriangleStripState")]
    public int TriangleStripState { get; set; }

    [MetaMember("mAnimatedVertexCount")]
    public int AnimatedVertexCount { get; set; }

    [MetaMember("mTextures")]
    public List<Texture>[] Textures { get; set; } = new List<Texture>[11];

    [MetaMember("mDiffuseScale")]
    public Vector2 DiffuseScale { get; set; }

    [MetaMember("mLightMapScale")]
    public Vector2 LightMapScale { get; set; }

    [MetaMember("mShadowMapScale")]
    public Vector2 ShadowMapScale { get; set; }

    [MetaMember("mDetailScale")]
    public Vector2 DetailScale { get; set; }

    [MetaMember("mScaledUVScale")]
    public Vector2 ScaledUVScale { get; set; }

    [MetaMember("mToolAnimatedVertexEntries")]
    public List<AnimatedVertexEntry> ToolAnimatedVertexEntries { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AnimatedVertexEntry>))]
    public class AnimatedVertexEntry
    {
    }

    [MetaMember("mToolAnimatedVertexGroupEntries")]
    public Dictionary<Symbol, AnimatedVertexGroupEntry> ToolAnimatedVertexGroupEntries { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AnimatedVertexGroupEntry>))]
    public class AnimatedVertexGroupEntry
    {
    }

    public PropertySet InternalResources { get; set; } = new();
    public T3IndexBuffer T3IndexBuffer { get; set; }
    public T3VertexBuffer[] T3VertexBuffers { get; set; }

    public T3OcclusionMeshData OcclusionMeshData { get; set; } = new();

    /// <summary>
    /// The main mesh data for version above 22. It contains LODs, bone references
    /// </summary>
    public T3MeshData MeshData { get; set; } = new();

    /// <summary>
    /// This is a combination of T3MeshBatch and others for old games ( from Texas Hold'em to unknown).
    /// </summary>
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TriangleSet>))]
    public class TriangleSet
    {
        [MetaMember("mFlags")]
        public Flags Flags { get; set; }

        [MetaMember("mBonePaletteIndex")]
        public int BonePaletteIndex { get; set; }

        [MetaMember("mLocalTransformPaletteIndex")]
        public int LocalTransformPaletteIndex { get; set; }

        [MetaMember("mLocalTransformIndex")]
        public int LocalTransformIndex { get; set; }

        [MetaMember("mGeometryFormat")]
        public int GeometryFormat { get; set; }

        [MetaMember("mMinVertIndex")]
        public int MinVertIndex { get; set; }

        [MetaMember("mMaxVertIndex")]
        public int MaxVertIndex { get; set; }

        [MetaMember("mStartIndex")]
        public int StartIndex { get; set; }

        [MetaMember("mNumPrimitives")]
        public int NumPrimitives { get; set; }

        [MetaMember("mhDiffuseMap")]
        public Handle<T3Texture> T3DiffuseMap { get; set; } = new();

        [MetaMember("mhDetailMap")]
        public Handle<T3Texture> T3DetailMap { get; set; } = new();

        [MetaMember("mVertexShaderName")]
        public string VertexShaderName { get; set; } = string.Empty;

        [MetaMember("mPixelShaderName")]
        public string PixelShaderName { get; set; } = string.Empty;

        [MetaMember("mLightingGroup")]
        public string LightingGroup { get; set; } = string.Empty;

        [MetaMember("mBoundingBox")]
        public BoundingBox BoundingBox { get; set; } = new();

        [MetaMember("mBoundingSphere")]
        public Sphere BoundingSphere { get; set; } = new();

        [MetaMember("mhLightMap")]
        public Handle<T3Texture> T3LightMap { get; set; } = new();

        [MetaMember("mhBumpMap")]
        public Handle<T3Texture> T3BumpMap { get; set; } = new();

        [MetaMember("mbHasPixelShader_RemoveMe")]
        public bool HasPixelShaderRemoveMe { get; set; }

        [MetaMember("mTxIndex")]
        public int[] TxIndex { get; set; } = new int[11];

        [MetaMember("mTriStrips")]
        public List<int> TriStrips { get; set; } = [];

        [MetaMember("mNumTotalIndices")]
        public int NumTotalIndices { get; set; }

        [MetaMember("mbDoubleSided")]
        public bool DoubleSided { get; set; }

        [MetaMember("mbBumpEffectsSpecular")]
        public bool BumpEffectsSpecular { get; set; }

        [MetaMember("mfBumpHeight")]
        public float BumpHeight { get; set; }

        [MetaMember("mfDetailBumpHeight")]
        public float DetailBumpHeight { get; set; }

        [MetaMember("mhEnvMap")]
        public Handle<T3Texture> T3EnvMap { get; set; } = new();

        [MetaMember("mfEccentricity")]
        public float Eccentricity { get; set; }

        [MetaMember("mSpecularColor")]
        public Color SpecularColor { get; set; }

        [MetaMember("mDiffuseColor")]
        public Color DiffuseColor { get; set; }

        [MetaMember("mAmbientColor")]
        public Color AmbientColor { get; set; }

        [MetaMember("mToonOutlineColor")]
        public Color ToonOutlineColor { get; set; }

        [MetaMember("mToonOutlineInvertColor")]
        public Color ToonOutlineInvertColor { get; set; }

        [MetaMember("mbSelfIlluminated")]
        public bool SelfIlluminated { get; set; }

        [MetaMember("mAlphaMode")]
        public int AlphaMode { get; set; }

        [MetaMember("mfReflectivity")]
        public float Reflectivity { get; set; }

        [MetaMember("mToonOutlineSize")]
        public float ToonOutlineSize { get; set; }

        [MetaMember("mToonMaxZConstOutlineSize")]
        public float ToonMaxZConstOutlineSize { get; set; }

        [MetaMember("mToonMinZConstOutlineSize")]
        public float ToonMinZConstOutlineSize { get; set; }

        [MetaMember("mGlowIntensity")]
        public float GlowIntensity { get; set; }

        [MetaMember("mReceiveShadowIntensity")]
        public float ReceiveShadowIntensity { get; set; }

        [MetaMember("mToonShades")]
        public int ToonShades { get; set; }

        [MetaMember("mUVGenMode")]
        public int UvGenMode { get; set; }

        [MetaMember("mUVScreenSpaceScaling")]
        public float UvScreenSpaceScaling { get; set; }

        [MetaMember("mSpecularOnAlpha")]
        public float SpecularOnAlpha { get; set; }

        [MetaMember("mSubsurfaceScateringRadius")]
        public float SubsurfaceScateringRadius { get; set; }

        [MetaClassSerializerGlobal(typeof(TriangleSetSerializer))]
        public class TriangleSetSerializer : MetaClassSerializer<TriangleSet>
        {
            private static readonly DefaultClassSerializer<TriangleSet> DefaultSerializer = new();

            public override void Serialize(ref TriangleSet obj, MetaStream stream)
            {
                DefaultSerializer.PreSerialize(ref obj, stream);
                DefaultSerializer.Serialize(ref obj, stream);

                if (stream is MetaStreamWriter streamWriter)
                {
                }
                else if (stream is MetaStreamReader streamReader)
                {
                    if (stream.GetMetaClass(typeof(TriangleSet)).ContainsMember("mbHasPixelShader_RemoveMe"))
                    {
                        string shaderName = streamReader.ReadString();
                        if (obj.HasPixelShaderRemoveMe)
                        {
                            string pixelShader = streamReader.ReadString();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// This is for old games where bones were embedded inside the mesh.
    /// </summary>
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PaletteEntry>))]
    public class PaletteEntry
    {
        [MetaMember("mBoneName")]
        public string BoneName { get; set; } = string.Empty;

        [MetaMember("mBoneName")]
        public Symbol SymbolBoneName { get; set; }

        [MetaMember("mBoundingBox")]
        public BoundingBox BoundingBox { get; set; } = new();

        [MetaMember("mBoundingSphere")]
        public Sphere BoundingSphere { get; set; } = new();

        [MetaMember("mNumVerts")]
        public int NumVerts { get; set; }

        [MetaMember("mSkeletonIndex")]
        public int SkeletonIndex { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(Serializer))]
    public class Serializer : MetaClassSerializer<D3DMesh>
    {
        private static readonly DefaultClassSerializer<D3DMesh> DefaultSerializer = new();

        public override void Serialize(ref D3DMesh obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                if (obj.Version < 19)
                {
                    SerializeOldD3DMesh(ref obj, stream);
                    return;
                }

                if (obj.Version >= 22)
                {
                    SerializeInternalResources(ref obj, stream);
                    return;
                }

                streamWriter.Write(0);

                if (obj.Version >= 52)
                {
                    bool hasOcclusionData = obj.OcclusionMeshData != null;
                    if (hasOcclusionData)
                    {
                        // TODO: Assign this.
                        stream.BeginBlock();
                        T3OcclusionMeshData objOcclusionMeshData = obj.OcclusionMeshData;
                        TTKGlobalContext.Instance().GetSerializer<T3OcclusionMeshData>()
                            .Serialize(ref objOcclusionMeshData, stream);
                        stream.EndBlock();
                    }
                }

                stream.BeginBlock();
                T3MeshData objMeshData = obj.MeshData;
                TTKGlobalContext.Instance().GetSerializer<T3MeshData>().PreSerialize(ref objMeshData, stream);
                TTKGlobalContext.Instance().GetSerializer<T3MeshData>().Serialize(ref objMeshData, stream);
                stream.EndBlock();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // According to Telltale, the new meshes start from version 20.
                if (obj.Version == 0)
                {
                    SerializeOldD3DMesh(ref obj, stream);
                    return;
                }

                if (obj.Version > 0 && obj.Version < 19)
                {
                    SerializeOldMedD3DMesh(ref obj, stream);
                    return;
                }

                if (obj.Version >= 22)
                {
                    SerializeInternalResources(ref obj, stream);
                }

                int toolData = streamReader.ReadInt32();
                // Telltale skip this data.
                if (toolData > 0)
                {
                    stream.BeginBlock();
                    streamReader.SkipToEndOfCurrentBlock();
                    stream.EndBlock();
                }

                // Probably new meshes?
                if (obj.Version >= 52)
                {
                    bool hasOcclusionData = streamReader.ReadBoolean();
                    if (hasOcclusionData)
                    {
                        // TODO: Assign this.
                        stream.BeginBlock();
                        T3OcclusionMeshData objOcclusionMeshData = obj.OcclusionMeshData;
                        TTKGlobalContext.Instance().GetSerializer<T3OcclusionMeshData>()
                            .Serialize(ref objOcclusionMeshData, stream);
                        stream.EndBlock();
                    }
                }

                stream.BeginBlock();
                T3MeshData objMeshData = obj.MeshData;
                TTKGlobalContext.Instance().GetSerializer<T3MeshData>().PreSerialize(ref objMeshData, stream);
                TTKGlobalContext.Instance().GetSerializer<T3MeshData>().Serialize(ref objMeshData, stream);
                stream.EndBlock();
            }
        }

        private static void SerializeOldMedD3DMesh(ref D3DMesh obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.T3VertexBuffers = new T3VertexBuffer[15];

                if (obj.Flags.Has((int)MeshFlags.HasIndexBuffer))
                {
                    var buffer = new T3IndexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3IndexBuffer>().Serialize(ref buffer, stream);
                    obj.T3IndexBuffer = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasPosStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[0] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasNormStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[1] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasSmoothNormStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[2] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasBlendWeightStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[3] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasBlendIdxStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[4] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasUV1Stream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[5] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasUV2Stream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[6] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasUV3Stream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[7] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasUV4Stream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[8] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasTangentStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[9] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasColorStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[10] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.Unknown))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[11] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasInterleavedStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[12] = buffer;
                }

                if (obj.Flags.Has((int)MeshFlags.HasSoftwareSkinningStream))
                {
                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                    obj.T3VertexBuffers[13] = buffer;
                }
                // Read Index Buffer
                // bool hasIndexBuffer = streamReader.ReadBoolean();
                //
                // if (hasIndexBuffer)
                // {

                // }
                //
                // // Read Vertex Buffer
                // var maxVertexBufferCount = 9;
                // if (obj.VertexAlphaSupport)
                // {
                //     maxVertexBufferCount++;
                // }
                //
                // obj.T3VertexBuffers = new T3VertexBuffer[maxVertexBufferCount];
                //
                // for (int i = 0; i < obj.T3VertexBuffers.Length; i++)
                // {
                //     bool hasVertexBuffer = streamReader.ReadBoolean();
                //
                //     if (!hasVertexBuffer)
                //         continue;
                //
                //     var buffer = new T3VertexBuffer();
                //     TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);
                //
                //     obj.T3VertexBuffers[i] = buffer;
                // }
            }
        }

        private static void SerializeOldD3DMesh(ref D3DMesh obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // Read Index Buffer
                bool hasIndexBuffer = streamReader.ReadBoolean();

                if (hasIndexBuffer)
                {
                    var buffer = new T3IndexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3IndexBuffer>().Serialize(ref buffer, stream);
                    obj.T3IndexBuffer = buffer;
                }

                // Read Vertex Buffer
                var maxVertexBufferCount = 9;
                if (obj.VertexAlphaSupport)
                {
                    maxVertexBufferCount++;
                }

                obj.T3VertexBuffers = new T3VertexBuffer[maxVertexBufferCount];

                for (int i = 0; i < obj.T3VertexBuffers.Length; i++)
                {
                    bool hasVertexBuffer = streamReader.ReadBoolean();

                    if (!hasVertexBuffer)
                        continue;

                    var buffer = new T3VertexBuffer();
                    TTKGlobalContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);

                    obj.T3VertexBuffers[i] = buffer;
                }
            }
        }

        private static void SerializeInternalResources(ref D3DMesh obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.InternalResources = new PropertySet();

                int numRes = streamReader.ReadInt32();

                for (var i = 0; i < numRes; i++)
                {
                    Symbol symbol = streamReader.ReadSymbol();
                    MetaClassType typeSymbol = streamReader.ReadMetaClassType();

                    stream.BeginBlock();

                    object? propertyValue = null;

                    TTKGlobalContext.Instance().GetSerializer(typeSymbol.LinkingType)
                        .PreSerialize(ref propertyValue, stream, typeSymbol);
                    TTKGlobalContext.Instance().GetSerializer(typeSymbol.LinkingType)
                        .Serialize(ref propertyValue, stream);

                    obj.InternalResources.Properties.Add(symbol,
                        new PropertySet.PropertyEntry(propertyValue, typeSymbol));

                    stream.EndBlock();
                }
            }
        }
    }

    [Flags]
    public enum MeshFlags
    {
        HasIndexBuffer = 0x1,
        HasPosStream = 0x2,
        HasNormStream = 0x4,
        HasSmoothNormStream = 0x8,
        HasBlendWeightStream = 0x10,
        HasBlendIdxStream = 0x20,
        HasUV1Stream = 0x40,
        HasUV2Stream = 0x80,
        HasUV3Stream = 0x100,
        HasUV4Stream = 0x200,
        HasTangentStream = 0x400,
        HasColorStream = 0x800,
        Unknown = 0x1000,
        HasVertexAnimation = 0x10000,
        TriangleSetsFixedUp = 0x40000,
        HasZeroVertexAlpha = 0x80000,
        HasInterleavedStream = 0x200000,
        HasSoftwareSkinningStream = 0x400000,
        IsManualSort = 0x10000000, // Vector 3???!?!?
        Deformable = 0x20000000, // It's a string - Interface
    }


    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<VertexAnimation>))]
    public class VertexAnimation
    {
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Texture>))]
    public class Texture
    {
        [MetaMember("mName")]
        public Handle<T3Texture> Name { get; set; }

        [MetaMember("mFlags")]
        public Flags Flags { get; set; }

        [MetaMember("mNameSymbol")]
        public Symbol NameSymbol { get; set; }

        [MetaMember("mBoundingBox")]
        public BoundingBox BoundingBox { get; set; } = new();

        [MetaMember("mBoundingSphere")]
        public Sphere BoundingSphere { get; set; } = new();

        [MetaMember("mMaxObjAreaPerUVArea")]
        public float MaxObjAreaPerUVArea { get; set; }

        [MetaMember("mAverageObjAreaPerUVArea")]
        public float AverageObjAreaPerUVArea { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<SkinningEntry>))]
    public class SkinningEntry
    {
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<BoneEntry>))]
    public class BoneEntry
    {
    }

    public PropertySet? GetMaterialPropertySet(int index)
    {
        var count = 0;
        foreach (PropertySet.PropertyEntry f in InternalResources.Properties.Values)
        {
            if (f.Value is not PropertySet pSet)
                continue;

            if (count == index)
            {
                return pSet;
            }

            count++;
        }

        return null;
    }
    
    public Handle<T3Texture>? GetDiffuseTexture(Handle<PropertySet> symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.ObjectInfo.ObjectName.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Diffuse Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetNormalMapTexture(Handle<PropertySet> symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.ObjectInfo.ObjectName.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Normal Map Texture");

        if (prop is null)
        {
            prop = propSet?.GetProperty("Material - Normal Texture");
        }

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetDetailTexture(Handle<PropertySet> symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.ObjectInfo.ObjectName.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Detail Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetSpecularTexture(Handle<PropertySet> symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.ObjectInfo.ObjectName.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Specular Map Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetDiffuseTexture(Symbol symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Diffuse Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetNormalMapTexture(Symbol symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Normal Map Texture");

        return prop as Handle<T3Texture>;
    }


    public Handle<T3Texture>? GetDiffuseTexture(int index)
    {
        PropertySet? propSet = GetMaterialPropertySet(index);

        object? prop = propSet?.GetProperty("Material - Diffuse Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetNormalMapTexture(int index)
    {
        PropertySet? propSet = GetMaterialPropertySet(index);

        object? prop = propSet?.GetProperty("Material - Normal Map Texture");

        return prop as Handle<T3Texture>;
    }


    public List<Handle<T3Texture>> GetDiffuseTextures()
    {
        List<Handle<T3Texture>> handles = [];
        foreach (PropertySet.PropertyEntry f in InternalResources.Properties.Values)
        {
            if (f.Value is not PropertySet pSet)
                continue;

            object? prop = pSet.GetProperty("Material - Diffuse Texture");

            if (prop is Handle<T3Texture> handle)
            {
                handles.Add(handle);
            }
        }

        return handles;
    }

    public List<Handle<T3Texture>> GetNormalTextures()
    {
        List<Handle<T3Texture>> handles = [];
        foreach (PropertySet.PropertyEntry f in InternalResources.Properties.Values)
        {
            if (f.Value is not PropertySet pSet)
                continue;

            object? prop = pSet.GetProperty("Material - Normal Space");

            if (prop is Handle<T3Texture> handle)
            {
                handles.Add(handle);
            }
        }

        return handles;
    }

    public enum T3VertexComponentType
    {
        // COMPONENT_TYPE_
        Position = 0,
        Uv = 1,
        Normal = 2,
        BlendWeight = 3,
        BlendIndex = 4,
        Colour = 5,

        // ??
        UNSURE_SOFTWARE_SKINNING_DATA = 6,
        SmoothNormal = 7,
        Tangent = 8,
        ShadowMapUv = 9,
        DetailMapUv = 10,
        ScaledUv = 11,
        LightmapUv = 12,
    }

    public enum T3TextureIndex
    {
        //   TEXTURE_INDEX_
        Diffuse = 0,
        LightMap_V0 = 1,
        NormalMap = 2,
        EnvMap = 3,
        Detail = 4,
        DetailNormal = 5, //DETAIL NORMAL? BUMPMAP?
        Specular = 6,
        UNKNOWN1 = 7, //??TEX8
        UNKNOWN2 = 8, //LIGHTWARP?
        UNKNOWN3 = 9, //??TEX10
        STATIC_SHADOW = 10,
        EMMISIVE = 11,
        NORMAL_GLOSS = 12, //TODO CHECK
        AMBIENT_OCCLUSION = 13,
        //From Lucas:
        // Unknowns could be: subsurface scat (normal or v0), lightmap hdr and scaled, sdf detail, spec colour, toon lookup, outling discontinuinity, particle props, lookup map, prefiltered cube, brush lookup,
    };
}
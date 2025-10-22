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

    [MetaMember("mbLowQualityRender")]
    public bool LowQualityRender { get; set; }

    [MetaMember("mbDontTriStrip")]
    public bool DontTriStrip { get; set; }

    [MetaMember("mbVertexAlphaSupport")]
    public bool VertexAlphaSupport { get; set; }

    [MetaMember("mbMeshHasVertexAlpha")]
    public bool MeshHasVertexAlpha { get; set; }


    public PropertySet InternalResources { get; set; } = new();
    public T3IndexBuffer T3IndexBuffer { get; set; }
    public T3VertexBuffer[] T3VertexBuffers { get; set; }

    public T3OcclusionMeshData OcclusionMeshData { get; set; } = new();
    public T3MeshData MeshData { get; set; } = new();

    /// <summary>
    /// This is a combination of T3MeshBatch and others for old games ( from Texas Hold'em to unknown).
    /// </summary>
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TriangleSet>))]
    public class TriangleSet
    {
        [MetaMember("mBonePaletteIndex")]
        public int BonePaletteIndex { get; set; }

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

        [MetaMember("mhLightMap")]
        public Handle<T3Texture> T3LightMap { get; set; } = new();

        [MetaMember("mhBumpMap")]
        public Handle<T3Texture> T3BumpMap { get; set; } = new();

        [MetaMember("mbHasPixelShader_RemoveMe")]
        public bool HasPixelShaderRemoveMe { get; set; }

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

        [MetaMember("mhEnvMap")]
        public Handle<T3Texture> T3EnvMap { get; set; } = new();

        [MetaMember("mfEccentricity")]
        public float Eccentricity { get; set; }

        [MetaMember("mSpecularColor")]
        public Color SpecularColor { get; set; }

        [MetaMember("mAmbientColor")]
        public Color AmbientColor { get; set; }

        [MetaMember("mbSelfIlluminated")]
        public bool SelfIlluminated { get; set; }

        [MetaMember("mAlphaMode")]
        public int AlphaMode { get; set; }

        [MetaMember("mfReflectivity")]
        public float Reflectivity { get; set; }

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
                    string shaderName = streamReader.ReadString();
                    if (obj.HasPixelShaderRemoveMe)
                    {
                        string pixelShader = streamReader.ReadString();
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
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // According to Telltale, the new meshes start from version 20.
                if (obj.Version < 19)
                {
                    SerializeOldD3DMesh(ref obj, stream);
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
                        TTKContext.Instance().GetSerializer<T3OcclusionMeshData>()
                            .Serialize(ref objOcclusionMeshData, stream);
                        stream.EndBlock();
                    }
                }

                stream.BeginBlock();
                T3MeshData objMeshData = obj.MeshData;
                TTKContext.Instance().GetSerializer<T3MeshData>().PreSerialize(ref objMeshData, stream);
                TTKContext.Instance().GetSerializer<T3MeshData>().Serialize(ref objMeshData, stream);
                stream.EndBlock();
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
                    TTKContext.Instance().GetSerializer<T3IndexBuffer>().Serialize(ref buffer, stream);
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
                    TTKContext.Instance().GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);

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

                    TTKContext.Instance().GetSerializer(typeSymbol.LinkingType)
                        .PreSerialize(ref propertyValue, stream, typeSymbol);
                    TTKContext.Instance().GetSerializer(typeSymbol.LinkingType).Serialize(ref propertyValue, stream);

                    obj.InternalResources.Properties.Add(symbol, propertyValue);

                    stream.EndBlock();
                }
            }
        }
    }


    public class VertexAnimation
    {
    }

    public class Texture
    {
    }

    public class SkinningEntry
    {
    }

    public class BoneEntry
    {
    }

    public PropertySet? GetMaterialPropertySet(int index)
    {
        int count = 0;
        foreach (object f in InternalResources.Properties.Values)
        {
            if (f is not PropertySet pSet)
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

    public Handle<T3Texture>? GetNormalMapTexture(Handle<PropertySet>  symbol)
    {
        var propSet = InternalResources.GetProperty(symbol.ObjectInfo.ObjectName.Crc64) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Normal Map Texture");

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
        foreach (object f in InternalResources.Properties.Values)
        {
            if (f is not PropertySet pSet)
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
        foreach (object f in InternalResources.Properties.Values)
        {
            if (f is not PropertySet pSet)
                continue;

            object? prop = pSet.GetProperty("Material - Normal Space");

            if (prop is Handle<T3Texture> handle)
            {
                handles.Add(handle);
            }
        }

        return handles;
    }

    
}
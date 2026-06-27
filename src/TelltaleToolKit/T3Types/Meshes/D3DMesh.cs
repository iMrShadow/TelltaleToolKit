using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Meshes.T3Types;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Skeletons;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Meshes;

[MetaSerializer(typeof(MetaClassSerializer<D3DMesh>))]
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
    public List<SkinningEntry> SkinningData { get; set; } = [];

    [MetaMember("mBoneData")]
    public List<BoneEntry> BoneData { get; set; } = [];

    [MetaMember("mDiffuseTextures")]
    public List<D3DMesh.Texture> mDiffuseTextures { get; set; } = [];

    [MetaMember("mDetailTextures")]
    public List<D3DMesh.Texture> mDetailTextures { get; set; } = [];

    [MetaMember("mDetailBumpTextures")]
    public List<D3DMesh.Texture> mDetailBumpTextures { get; set; } = [];

    [MetaMember("mLightmapTextures")]
    public List<D3DMesh.Texture> mLightmapTextures { get; set; } = [];

    [MetaMember("mBumpmapTextures")]
    public List<D3DMesh.Texture> mBumpmapTextures { get; set; } = [];


    [MetaMember("mEnvironmentTextures")]
    public List<D3DMesh.Texture> mEnvironmentTextures { get; set; } = [];

    [MetaMember("mSpecularTextures")]
    public List<D3DMesh.Texture> mSpecularTextures { get; set; } = [];

    [MetaMember("mSSSTextures")]
    public List<D3DMesh.Texture> mSSSTextures { get; set; } = [];

    [MetaMember("mLocalTransformPalettes")]
    public List<List<LocalTransformEntry>> LocalTransformPalettes { get; set; } = [];

    [MetaMember("mbMeshHasSmoothNormalsSupport")]
    public bool mbMeshHasSmoothNormalsSupport { get; set; }

    [MetaMember("mDiffuseTextureNames")]
    public List<string> mDiffuseTextureNames { get; set; } = [];

    [MetaMember("mDetailTextureNames")]
    public List<string> mDetailTextureNames { get; set; } = [];

    [MetaMember("mLightmapTextureNames")]
    public List<string> mLightmapTextureNames { get; set; } = [];

    [MetaMember("mBumpmapTextureNames")]
    public List<string> mBumpmapTextureNames { get; set; } = [];

    [MetaMember("mEnvironmentTextureNames")]
    public List<string> mEnvironmentTextureNames { get; set; } = [];

    [MetaMember("mbToonRendering")]
    public bool mbToonRendering { get; set; }

    [MetaMember("mbToonLighting")]
    public bool mbToonLighting { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<LocalTransformEntry>))]
    public class LocalTransformEntry
    {
        [MetaMember("mTransform")]
        public Transform Transform { get; set; } = new();

        [MetaMember("mCameraFacingType")]
        public T3CameraFacingType FacingType { get; set; }
    }

    [MetaMember("mTriangleStripState")]
    public int TriangleStripState { get; set; }

    [MetaMember("mAnimatedVertexCount")]
    public int AnimatedVertexCount { get; set; }

    [MetaMember("mTextures")]
    public List<Texture>[] Textures { get; set; }

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

    [MetaMember("mToonOutlineSize")]
    public float ToonOutlineSize { get; set; }

    [MetaMember("mToonMaxZConstOutlineSize")]
    public float ToonMaxZConstOutlineSize { get; set; }

    [MetaMember("mToonMinZConstOutlineSize")]
    public float ToonMinZConstOutlineSize { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<AnimatedVertexEntry>))]
    public class AnimatedVertexEntry;

    [MetaMember("mToolAnimatedVertexGroupEntries")]
    public Dictionary<Symbol, AnimatedVertexGroupEntry> ToolAnimatedVertexGroupEntries { get; set; } = new();

    [MetaSerializer(typeof(MetaClassSerializer<AnimatedVertexGroupEntry>))]
    public class AnimatedVertexGroupEntry;

    [MetaMember("mInternalResources")]
    public List<HandleBase> InternalResources { get; set; } = new();

    public T3IndexBuffer? T3IndexBuffer { get; set; }
    public T3VertexBuffer[]? T3VertexBuffers { get; set; }

    public T3OcclusionMeshData? OcclusionMeshData { get; set; }

    [MetaMember("mbManualSort")]
    public bool ManualSort { get; set; }

    /// <summary>
    /// The main mesh data for version above 22. It contains LODs, bone references
    /// </summary>
    [MetaMember("mMeshData")]
    public T3MeshData MeshData { get; set; } = new();

    /// <summary>
    /// This is a combination of T3MeshBatch and others for old games ( from Texas Hold'em to unknown).
    /// </summary>
    [MetaSerializer(typeof(Serializer))]
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

        [MetaMember("mVertexShaderName")]
        public Symbol VertexShaderNameS { get; set; } = Symbol.Empty;

        [MetaMember("mPixelShaderName")]
        public Symbol PixelShaderNameS { get; set; } = Symbol.Empty;

        [MetaMember("mLightingGroup")]
        public string LightingGroup { get; set; } = string.Empty;

        [MetaMember("mLightingGroup")]
        public Symbol LightingGroupS { get; set; } = Symbol.Empty;

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
        public int[] TxIndex { get; set; }

        [MetaMember("mTriStrips")]
        public List<int> TriStrips { get; set; } = [];

        [MetaMember("mTriStrips")]
        public List<ushort> TriStripsS { get; set; } = [];

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

        [MetaMember("mSpecularPower")]
        public float SpecularPower { get; set; }

        [MetaMember("mBoneMatrix")]
        public int BoneMatrix { get; set; }

        [MetaMember("mToonNumShades")]
        public int ToonNumShades { get; set; }

        [MetaMember("mLocalTransform")]
        public Transform LocalTransform { get; set; }

        [MetaMember("mCameraFacingType")]
        public int CameraFacingType { get; set; }

        [MetaMember("mToonSphericalNormalBoneName")]
        public Symbol ToonSphericalNormalBoneName { get; set; } = Symbol.Empty;


        [MetaMember("mBoneMatrixIndex")]
        public int BoneMatrixIndex { get; set; } = 0;

        [MetaMember("mDiffuseTextureIndex")]
        public int DiffuseTextureIndex { get; set; } = 0;

        [MetaMember("mLightMapTextureIndex")]
        public int LightMapTextureIndex { get; set; } = 0;

        [MetaMember("mDetailTextureIndex")]
        public int DetailTextureIndex { get; set; } = 0;

        [MetaMember("mDetailBumpTextureIndex")]
        public int DetailBumpTextureIndex { get; set; } = 0;

        [MetaMember("mNormalTextureIndex")]
        public int NormalTextureIndex { get; set; } = 0;

        [MetaMember("mEnvTextureIndex")]
        public int EnvTextureIndex { get; set; } = 0;

        [MetaMember("mSpecularTextureIndex")]
        public int SpecularTextureIndex { get; set; } = 0;

        [MetaMember("mSSSTextureIndex")]
        public int SSSTextureIndex { get; set; } = 0;

        [MetaMember("mhSpecularColorMap")]
        public Handle<T3Texture> SpecularColorMap { get; set; }

        [MetaMember("mhAmbientMap")]
        public Handle<T3Texture> AmbientMap { get; set; }

        [MetaMember("mhOutlineDiscontinuityMap")]
        public Handle<T3Texture> OutlineDiscontinuityMap { get; set; }

        [MetaMember("mhToonLightQuantized")]
        public Handle<T3Texture> ToonLightQuantized { get; set; }


        [MetaMember("mbBumpAsNormalMap")]
        public bool BumpAsNormalMap { get; set; }

        [MetaMember("mToonMaterialColor")]
        public Color ToonMaterialColor { get; set; }

        [MetaMember("mToonOffset")]
        public Vector2 ToonOffset { get; set; }

        [MetaMember("mToonEnvLighting")]
        public bool mToonEnvLighting { get; set; }

        [MetaMember("mToonNoNormalDeform")]
        public bool mToonNoNormalDeform { get; set; }

        [MetaMember("mNeedSWSkinning")]
        public bool mNeedSWSkinning { get; set; }

        [MetaMember("mNeedComputeOutline")]
        public bool mNeedComputeOutline { get; set; }

        [MetaMember("mNeedRenderOutline")]
        public bool mNeedRenderOutline { get; set; }

        [MetaMember("mReceiveShadows")]
        public bool mReceiveShadows { get; set; }

        [MetaMember("mCastShadows")]
        public bool mCastShadows { get; set; }

        [MetaMember("mbToonRendering")]
        public bool mbToonRendering { get; set; }

        [MetaMember("mUVScreenSpaceZoom")]
        public bool mUVScreenSpaceZoom { get; set; }

        [MetaMember("mbHasOctree")]
        public bool mbHasOctree { get; set; }

        [MetaMember("mToonSphericalNormals")]
        public bool mToonSphericalNormals { get; set; }

        [MetaMember("mToonSphericalNormalBoneName")]
        public string mToonSphericalNormalBoneName { get; set; }

        [MetaMember("mbVertexAlpha")]
        public bool mbVertexAlpha { get; set; }

        [MetaMember("mbCameraFacing")]
        public bool mbCameraFacing { get; set; }

        [MetaMember("mhSubsurfaceScateringMap")]
        public Handle<T3Texture> mhSubsurfaceScateringMap { get; set; }

        [MetaMember("mbVertexAnimation")]
        public bool mbVertexAnimation { get; set; }

        [MetaMember("mbToonLighting")]
        public bool mbToonLighting { get; set; }

        [MetaMember("mOverride3DAlpha")]
        public bool Override3DAlpha { get; set; }

        [MetaMember("mhDetailBumpMap")]
        public Handle<T3Texture> mhDetailBumpMap { get; set; }

        [MetaMember("mbHasLocalTransform")]
        public bool HasLocalTransform { get; set; }

        [MetaMember("mbDetailBumpAsNormalMap")]
        public bool DetailBumpAsNormalMap { get; set; }


        public class Serializer : MetaSerializer<TriangleSet>
        {
            private static readonly MetaClassSerializer<TriangleSet> s_metaClassSerializer = new();

            public override void Serialize(ref TriangleSet obj, MetaStream stream, MetaClassType? type = null)
            {
                s_metaClassSerializer.PreSerialize(ref obj, stream);
                s_metaClassSerializer.Serialize(ref obj, stream);

                if (stream.Mode is MetaStreamMode.Write)
                {
                }
                else if (stream.Mode is MetaStreamMode.Read)
                {
                    if (stream.GetMetaClass(typeof(TriangleSet))!.ContainsMember("mbHasPixelShader_RemoveMe"))
                    {
                        string shaderName = stream.ReadString();
                        if (obj.HasPixelShaderRemoveMe)
                        {
                            string pixelShader = stream.ReadString();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// This is for old games where bones were embedded inside the mesh.
    /// </summary>
    [MetaSerializer(typeof(MetaClassSerializer<PaletteEntry>))]
    public class PaletteEntry
    {
        [MetaMember("mBoneName")]
        public string BoneName { get; set; } = string.Empty;

        [MetaMember("mBoneName")]
        public Symbol SymbolBoneName { get; set; } = Symbol.Empty;

        [MetaMember("mBoundingBox")]
        public BoundingBox BoundingBox { get; set; } = new();

        [MetaMember("mBoundingSphere")]
        public Sphere BoundingSphere { get; set; } = new();

        [MetaMember("mNumVerts")]
        public int NumVerts { get; set; }

        [MetaMember("mSkeletonIndex")]
        public int SkeletonIndex { get; set; }
    }

    [MetaSerializer(typeof(Serializer))]
    public class Serializer : MetaSerializer<D3DMesh>
    {
        private static readonly MetaClassSerializer<D3DMesh> s_metaClassSerializer = new();

        public override void Serialize(ref D3DMesh obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                var vertexBufferSlots = new (MeshFlags Flag, int Index)[]
                {
                    (MeshFlags.HasPosStream, 0), (MeshFlags.HasNormStream, 1), (MeshFlags.HasBlendWeightStream, 2),
                    (MeshFlags.HasBlendIdxStream, 3), (MeshFlags.HasUV1Stream, 4), (MeshFlags.HasUV2Stream, 5),
                    (MeshFlags.HasUV3Stream, 6), (MeshFlags.HasUV4Stream, 7), (MeshFlags.HasTangentStream, 8),
                    (MeshFlags.HasColorStream, 9), (MeshFlags.HasSmoothNormStream, 10), (MeshFlags.Unknown, 11),
                    (MeshFlags.Unknown2, 12), (MeshFlags.HasInterleavedStream, 13), (MeshFlags.Deformable, 14),
                    (MeshFlags.HasSoftwareSkinningStream, 15)
                };

                if (obj.T3IndexBuffer != null)
                {
                    obj.Flags.Set((int)MeshFlags.HasIndexBuffer);
                }

                if (obj.T3VertexBuffers != null)
                {
                    for (int i = 0; i < obj.T3VertexBuffers.Length; i++)
                    {
                        obj.Flags.Set((int)vertexBufferSlots[i].Flag);
                    }
                }
            }

            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj.Version == 0)
                {
                    SerializeOldD3DMesh(ref obj, stream);
                    return;
                }

                if (obj.Version < 19)
                {
                    SerializeOldMedD3DMesh(ref obj, stream);
                    return;
                }

                if (obj.Version >= 22)
                {
                    SerializeInternalResources(ref obj, stream);
                }

                stream.Write(0);


                if (obj.Version >= 52)
                {
                    bool hasOcclusionData = obj.OcclusionMeshData != null;
                    stream.Write(hasOcclusionData);

                    if (hasOcclusionData)
                    {
                        stream.BeginBlock();
                        T3OcclusionMeshData? objOcclusionMeshData = obj.OcclusionMeshData;
                        stream.Serialize(ref objOcclusionMeshData);
                        stream.EndBlock();
                    }
                }

                stream.BeginBlock();
                T3MeshData objMeshData = obj.MeshData;
                stream.Serialize(ref objMeshData);
            }
            else
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

                int toolData = stream.ReadInt32();
                // Telltale skip this data.
                if (toolData > 0)
                {
                    stream.BeginBlock();
                    stream.SkipToEndOfCurrentBlock();
                    stream.EndBlock();
                }

                // Probably new meshes?
                if (obj.Version >= 52)
                {
                    bool hasOcclusionData = stream.ReadBoolean();
                    if (hasOcclusionData)
                    {
                        stream.BeginBlock();
                        T3OcclusionMeshData objOcclusionMeshData = new();
                        stream.Serialize(ref objOcclusionMeshData);
                        obj.OcclusionMeshData = objOcclusionMeshData;
                        stream.EndBlock();
                    }
                }

                stream.BeginBlock();

                T3MeshData objMeshData = new();
                stream.Serialize(ref objMeshData);
                obj.MeshData = objMeshData;
            }

            stream.EndBlock();
        }

        private static void SerializeOldMedD3DMesh(ref D3DMesh obj, MetaStream stream)
        {
            var vertexBufferSlots = new (MeshFlags Flag, int Index)[]
            {
                (MeshFlags.HasPosStream, 0), (MeshFlags.HasNormStream, 1), (MeshFlags.HasBlendWeightStream, 2),
                (MeshFlags.HasBlendIdxStream, 3), (MeshFlags.HasUV1Stream, 4), (MeshFlags.HasUV2Stream, 5),
                (MeshFlags.HasUV3Stream, 6), (MeshFlags.HasUV4Stream, 7), (MeshFlags.HasTangentStream, 8),
                (MeshFlags.HasColorStream, 9), (MeshFlags.HasSmoothNormStream, 10), (MeshFlags.Unknown, 11),
                (MeshFlags.Unknown2, 12), (MeshFlags.HasInterleavedStream, 13), (MeshFlags.Deformable, 14),
                (MeshFlags.HasSoftwareSkinningStream, 15)
            };

            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj.Flags.Has((int)MeshFlags.HasIndexBuffer))
                {
                    T3IndexBuffer objT3IndexBuffer = obj.T3IndexBuffer;
                    stream.Serialize(ref objT3IndexBuffer);
                }

                for (int i = 0; i < vertexBufferSlots.Length; i++)
                {
                    stream.Serialize(ref obj.T3VertexBuffers[i]);
                }
            }
            else
            {
                obj.T3VertexBuffers = new T3VertexBuffer[16];

                if (obj.Flags.Has((int)MeshFlags.HasIndexBuffer))
                {
                    var buffer = new T3IndexBuffer();
                    stream.Serialize(ref buffer);
                    obj.T3IndexBuffer = buffer;
                }

                foreach ((MeshFlags flag, int index) in vertexBufferSlots)
                {
                    if (obj.Flags.Has((int)flag))
                    {
                        var flags = (MeshFlags)obj.Flags.Data;
                        var buffer = new T3VertexBuffer();
                        stream.Serialize(ref buffer);
                        obj.T3VertexBuffers[index] = buffer;

                        // is compressed
                        if (obj.T3VertexBuffers[index].Flags.Has(1))
                        {
                            // TODO: Decompress data
                            if (flag is MeshFlags.HasPosStream)
                            {
                                buffer.DecompressPositions(stream);
                            }
                            else if (flag is MeshFlags.HasNormStream or MeshFlags.HasTangentStream)
                            {
                                buffer.DecompressNormals(stream);
                            }
                            else if (flag is MeshFlags.HasBlendWeightStream)
                            {
                                buffer.DecompressWeights(stream);
                            }
                            else if (flag is MeshFlags.HasUV1Stream or MeshFlags.HasUV2Stream or MeshFlags.HasUV3Stream or MeshFlags.HasUV4Stream)
                            {
                                buffer.DecompressUV(stream);
                            }
                        }
                    }
                }

                // Read Index Buffer
                // bool hasIndexBuffer = stream.ReadBoolean();
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
                //     bool hasVertexBuffer = stream.ReadBoolean();
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
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                // Read Index Buffer
                bool hasIndexBuffer = stream.ReadBoolean();

                if (hasIndexBuffer)
                {
                    var buffer = new T3IndexBuffer();
                    Toolkit.Instance.GetSerializer<T3IndexBuffer>().Serialize(ref buffer, stream);
                    obj.T3IndexBuffer = buffer;
                }

                // Read Vertex Buffer
                int maxVertexBufferCount = 9;
                if (obj.VertexAlphaSupport)
                {
                    maxVertexBufferCount++;
                }

                obj.T3VertexBuffers = new T3VertexBuffer[maxVertexBufferCount];

                for (int i = 0; i < obj.T3VertexBuffers.Length; i++)
                {
                    bool hasVertexBuffer = stream.ReadBoolean();

                    if (!hasVertexBuffer)
                        continue;

                    var buffer = new T3VertexBuffer();
                    Toolkit.Instance.GetSerializer<T3VertexBuffer>().Serialize(ref buffer, stream);

                    obj.T3VertexBuffers[i] = buffer;
                }
            }
        }

        private static void SerializeInternalResources(ref D3DMesh obj, MetaStream stream)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                obj.InternalResources ??= [];

                stream.Write(obj.InternalResources.Count);

                foreach (HandleBase handle in obj.InternalResources)
                {
                    Symbol symbol = handle?.ObjectInfo?.ObjectName ?? Symbol.Empty;

                    object? propertyValue = handle?.ObjectInfo?.HandleObject;
                    if (propertyValue is null)
                        throw new InvalidOperationException(
                            $"D3DMesh internal resource '{symbol}' has no HandleObject.");

                    MetaClass metaClass = stream.GetMetaClass(propertyValue.GetType());
                    MetaClassType typeSymbol = metaClass.ClassType;
                    handle.ObjectInfo.Type = typeSymbol;

                    stream.Write(symbol);
                    stream.Write(typeSymbol.Symbol.Crc64);

                    stream.BeginBlock();


                    MetaSerializer serializer = Toolkit.Instance.GetSerializer(typeSymbol.LinkingType);
                    serializer.PreSerialize(ref propertyValue, stream, typeSymbol);
                    serializer.Serialize(ref propertyValue, stream);

                    stream.EndBlock();
                }
            }
            else
            {
                // Broken for: adv_johnsHouseInteriorUpstairsFire103_meshesE.d3dmesh in M103, TWD:DE
                obj.InternalResources = [];

                // Part of DC Array
                int numRes = stream.ReadInt32();

                for (int i = 0; i < numRes; i++)
                {
                    // Handle name?
                    Symbol symbol = stream.ReadSymbol();
                    MetaClassType? typeSymbol = stream.ReadMetaClassType();

                    stream.BeginBlock();

                    if (typeSymbol is null)
                    {
                        Toolkit.Instance.Logger.LogError(
                            $"[D3DMesh] Internal resource '{symbol}' has no registered type.");
                        stream.EndBlock();
                        continue;
                    }

                    object? propertyValue = null;

                    Toolkit.Instance.GetSerializer(typeSymbol.LinkingType)
                        .PreSerialize(ref propertyValue!, stream, typeSymbol);
                    Toolkit.Instance.GetSerializer(typeSymbol.LinkingType)
                        .Serialize(ref propertyValue, stream, typeSymbol);

                    var objHandle = new HandleBase
                    {
                        ObjectInfo = { ObjectName = symbol, Type = typeSymbol, HandleObject = propertyValue }
                    };

                    obj.InternalResources.Add(objHandle);

                    stream.EndBlock();
                }
            }
        }
    }

    [Flags]
    public enum MeshFlags
    {
        HasIndexBuffer = 0x1,
        HasPosStream = 0x2, // 1
        HasNormStream = 0x4, // 2
        HasSmoothNormStream = 0x8, // 11
        HasBlendWeightStream = 0x10, // 3
        HasBlendIdxStream = 0x20, // 4
        HasUV1Stream = 0x40, // 5
        HasUV2Stream = 0x80, // 6
        HasUV3Stream = 0x100, // 7
        HasUV4Stream = 0x200, // 8
        HasTangentStream = 0x400, // 9
        HasColorStream = 0x800, // 10
        Unknown = 0x1000, // 12
        Unknown2 = 0x2000, // 13
        HasVertexAnimation = 0x10000,
        TriangleSetsFixedUp = 0x40000,
        HasZeroVertexAlpha = 0x80000,
        HasInterleavedStream = 0x200000, // 14
        HasSoftwareSkinningStream = 0x400000, // 16
        IsManualSort = 0x10000000, // Vector 3???!?!?
        Deformable = 0x2000000, // It's a string - Interface // 15 ACTUALLY NOT A BUFFER
    }

    [MetaSerializer(typeof(MetaClassSerializer<VertexAnimation>))]
    public class VertexAnimation
    {
        [MetaMember("mName")]
        public Symbol Name { get; set; }

        [MetaMember("mResourceGroupMembership")]
        public Dictionary<Symbol, float> ResourceGroupMembership { get; set; } = [];

        [MetaMember("mStartIndex")]
        public int StartIndex { get; set; }

        [MetaMember("mNumVertices")]
        public int NumVertices { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<Texture>))]
    public class Texture
    {
        //'eFlagHasLightmap',0
        //  'eFlagHasNonLightmap',0
        // 'eFlagHasSpecular',0
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

        [MetaMember("mbHasLightmap")]
        public bool mbHasLightmap { get; set; }

        [MetaMember("mbHasNonLightmap")]
        public bool mbHasNonLightmap { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<SkinningEntry>))]
    public class SkinningEntry;

    [MetaSerializer(typeof(MetaClassSerializer<BoneEntry>))]
    public class BoneEntry;

    public PropertySet? GetMaterialPropertySet(int index)
    {
        int count = 0;
        foreach (HandleBase? handle in InternalResources)
        {
            if (handle.ObjectInfo.HandleObject is not PropertySet pSet)
                continue;

            if (count == index)
            {
                return pSet;
            }

            count++;
        }

        return null;
    }

    private object GetInternalResource(Symbol symbol)
    {
        return InternalResources.Find(res => res.ObjectInfo.ObjectName == symbol).ObjectInfo.HandleObject;
    }

    /// <summary>
    /// Attempts to get the diffuse texture (base color) of this mesh
    /// </summary>
    /// <param name="symbol">Material handle <see cref="T3MeshMaterial.Material"/></param>
    /// <returns>Handle to the diffuse texture if it exists, null otherwise</returns>
    public Handle<T3Texture>? GetDiffuseTexture(Handle<PropertySet> symbol)
    {
        var propSet = GetInternalResource(symbol.ObjectInfo.ObjectName) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Diffuse Texture");

        return prop as Handle<T3Texture>;
    }

    /// <summary>
    /// Attempts to get the normal map of this mesh. This can and will still exist even when per-vertex normals exist.
    /// </summary>
    /// <param name="symbol">Material handle <see cref="T3MeshMaterial.Material"/></param>
    /// <returns>Handle to the normal map texture if it exists, null otherwise</returns>
    public Handle<T3Texture>? GetNormalMapTexture(Handle<PropertySet> symbol)
    {
        var propSet = GetInternalResource(symbol.ObjectInfo.ObjectName) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Normal Map Texture");

        if (prop is null)
        {
            prop = propSet?.GetProperty("Material - Normal Texture");
        }

        return prop as Handle<T3Texture>;
    }

    /// <summary>
    /// Attempts to get the detail texture of this mesh.
    /// </summary>
    /// <param name="symbol"> Material handle <see cref="T3MeshMaterial.Material"/></param>
    /// <returns>Handle to the detail texture if it exists, null otherwise</returns>
    public Handle<T3Texture>? GetDetailTexture(Handle<PropertySet> symbol)
    {
        var propSet = GetInternalResource(symbol.ObjectInfo.ObjectName) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Detail Texture");

        return prop as Handle<T3Texture>;
    }

    /// <summary>
    /// Attempts to get the specular texture of this mesh.
    /// </summary>
    /// <param name="symbol"> Material handle <see cref="T3MeshMaterial.Material"/></param>
    /// <returns>Handle to the specular texture if it exists, null otherwise</returns>
    public Handle<T3Texture>? GetSpecularTexture(Handle<PropertySet> symbol)
    {
        var propSet = GetInternalResource(symbol.ObjectInfo.ObjectName) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Specular Map Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetDiffuseTexture(Symbol symbol)
    {
        var propSet = GetInternalResource(symbol) as PropertySet;

        object? prop = propSet?.GetProperty("Material - Diffuse Texture");

        return prop as Handle<T3Texture>;
    }

    public Handle<T3Texture>? GetNormalMapTexture(Symbol symbol)
    {
        var propSet = GetInternalResource(symbol) as PropertySet;

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
        foreach (HandleBase? obj in InternalResources)
        {
            if (obj.ObjectInfo.HandleObject is not PropertySet pSet)
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
        foreach (HandleBase? obj in InternalResources)
        {
            if (obj.ObjectInfo.HandleObject is not PropertySet pSet)
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
    }
}

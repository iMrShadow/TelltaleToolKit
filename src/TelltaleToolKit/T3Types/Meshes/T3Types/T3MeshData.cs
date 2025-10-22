using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class T3MeshData
{
    [Flags]
    [MetaClassSerializerGlobal(typeof(EnumSerializer<MeshFlags>))]
    public enum MeshFlags
    {
        eDeformable = 0x1,
        eHasLocalCameraFacing = 0x2,
        eHasLocalCameraFacingY = 0x4,
        eHasLocalCameraFacingLocalY = 0x8,
        eHasCPUSkinning = 0x10,
        eHasComputeSkinning = 0x20
    }

    [MetaMember("mLODs")]
    public List<T3MeshLOD> LODs { get; set; }

    [MetaMember("mTextures")]
    public List<T3MeshTexture> Textures { get; set; }

    [MetaMember("mMaterials")]
    public List<T3MeshMaterial> Materials { get; set; }

    [MetaMember("mMaterialOverrides")]
    public List<T3MeshMaterialOverride> MaterialOverrides { get; set; }

    [MetaMember("mBones")]
    public List<T3MeshBoneEntry> Bones { get; set; }

    [MetaMember("mLocalTransforms")]
    public List<T3MeshLocalTransformEntry> LocalTransforms { get; set; }

    [MetaMember("mMaterialRequirements")]
    public T3MaterialRequirements MaterialRequirements { get; set; }

    [MetaMember("mVertexStreams")]
    public BitSetBase VertexStreams { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mEndianType")]
    public T3MeshEndianType EndianType { get; set; }

    [MetaMember("mPositionScale")]
    public Vector3 PositionScale { get; set; }

    [MetaMember("mPositionWScale")]
    public Vector3 PositionWScale { get; set; }

    [MetaMember("mPositionOffset")]
    public Vector3 PositionOffset { get; set; }

    [MetaMember("mLightmapTexelAreaPerSurfaceArea")]
    public float LightmapTexelAreaPerSurfaceArea { get; set; }

    [MetaMember("mPropertyKeyBase")]
    public Symbol PropertyKeyBase { get; set; }

    [MetaMember("mVertexCount")]
    public uint VertexCount { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mMeshPreload")]
    public List<T3MeshEffectPreload> MeshPreload { get; set; } = [];

    // Batman
    [MetaMember("mBonePalettes")]
    public List<List<T3MeshBonePaletteEntry>> BonePalettes { get; set; }
    // TODO:
    // public List<T3MeshEffectPreload> MeshPreload { get; set; } = [];

    [MetaMember("mLocalTransformPalettes")]
    public List<List<T3MeshLocalTransformEntry>> LocalTransformPalettes { get; set; }

    public List<T3GFXVertexState> VertexStates { get; set; } = [];
    public T3MeshCPUSkinningData CPUSkinningData { get; set; }
    public T3MeshTexCoordTransform[] TexCoordTransform { get; set; } = new T3MeshTexCoordTransform[4];

    public class Serializer : MetaClassSerializer<T3MeshData>
    {
        private static readonly DefaultClassSerializer<T3MeshData> DefaultSerializer = new();

        public override void Serialize(ref T3MeshData obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                uint uvTransforms = streamReader.ReadUInt32();

                for (var i = 0; i < uvTransforms; i++)
                {
                    int uvLayer = streamReader.ReadInt32();

                    // Experimenting with a helper function.
                    TTK.PreSerialize(ref obj.TexCoordTransform[uvLayer], stream);
                    TTK.Serialize(ref obj.TexCoordTransform[uvLayer], stream);
                    // TTKContext.Instance().GetSerializer<T3MeshTexCoordTransform>().Serialize(ref obj.TexCoordTransform[vector], stream);
                }

                if ((obj.Flags.Data & (int)MeshFlags.eHasCPUSkinning) != 0)
                {
                    T3MeshCPUSkinningData t3MeshCpuSkinningData = obj.CPUSkinningData;
                    TTK.PreSerialize(ref t3MeshCpuSkinningData, stream);
                    TTK.Serialize(ref t3MeshCpuSkinningData, stream);
                }

                int vertexStates = streamReader.ReadInt32();

                for (var i = 0; i < vertexStates; i++)
                {
                    var state = new T3GFXVertexState();
                    TTK.Serialize(ref state, stream);
                    obj.VertexStates.Add(state);
                }
            }
        }
    }
}

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshBonePaletteEntry>))]
public class T3MeshBonePaletteEntry
{
    [MetaMember("mBoneName")]
    public Symbol BoneName { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mNumVerts")]
    public int NumVerts { get; set; }
}
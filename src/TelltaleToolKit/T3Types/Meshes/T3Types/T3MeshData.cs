using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(Serializer))]
public class T3MeshData
{
    [Flags]
    [MetaSerializer(typeof(EnumSerializer<MeshFlags>))]
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

    [MetaMember("mLocalTransformPalettes")]
    public List<List<T3MeshLocalTransformEntry>> LocalTransformPalettes { get; set; }

    public List<T3GFXVertexState> VertexStates { get; set; } = [];
    public T3MeshCPUSkinningData CPUSkinningData { get; set; }
    public T3MeshTexCoordTransform[] TexCoordTransform { get; set; } = [new(), new(), new(), new()];

    public class Serializer : MetaSerializer<T3MeshData>
    {
        private static readonly MetaClassSerializer<T3MeshData> s_metaClassSerializer = new();


        public override void Serialize(ref T3MeshData obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                uint uvTransformCount = 0;
                // Referenced from Lucas Saragosa's serialization code
                for (var i = 0; i < Math.Min(obj.TexCoordTransform.Length, 4); i++)
                {
                    T3MeshTexCoordTransform transform = obj.TexCoordTransform[i];
                    if (transform.Scale != Vector2.One || transform.Offset != Vector2.Zero)
                    {
                        uvTransformCount++;
                    }
                }

                stream.Write(uvTransformCount);

                for (var i = 0; i < Math.Min(obj.TexCoordTransform.Length, 4); i++)
                {
                    T3MeshTexCoordTransform transform = obj.TexCoordTransform[i];

                    if (transform.Scale == Vector2.One && transform.Offset == Vector2.Zero)
                    {
                        continue;
                    }

                    stream.Write((uint)i);
                    stream.Serialize(ref transform);
                    obj.TexCoordTransform[i] = transform;
                }

                bool hasCpuSkinning = (obj.Flags.Data & (int)MeshFlags.eHasCPUSkinning) != 0;
                if (hasCpuSkinning)
                {
                    T3MeshCPUSkinningData cpuSkinning = obj.CPUSkinningData;
                    stream.Serialize(ref cpuSkinning);
                    obj.CPUSkinningData = cpuSkinning;
                }

                stream.Write(obj.VertexStates.Count);

                for (var i = 0; i < obj.VertexStates.Count; i++)
                {
                    T3GFXVertexState state = obj.VertexStates[i];
                    stream.Serialize(ref state);
                    obj.VertexStates[i] = state;
                }
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                uint uvTransforms = stream.ReadUInt32();

                for (var i = 0; i < uvTransforms; i++)
                {
                    int uvLayer = stream.ReadInt32();

                    // Experimenting with a helper function.
                    stream.Serialize(ref obj.TexCoordTransform[uvLayer]);
                    // TTKContext.Instance().GetSerializer<T3MeshTexCoordTransform>().Serialize(ref obj.TexCoordTransform[vector], stream);
                }

                if ((obj.Flags.Data & (int)MeshFlags.eHasCPUSkinning) != 0)
                {
                    T3MeshCPUSkinningData t3MeshCpuSkinningData = obj.CPUSkinningData;
                    stream.Serialize(ref t3MeshCpuSkinningData);
                    obj.CPUSkinningData = t3MeshCpuSkinningData;
                }

                int vertexStates = stream.ReadInt32();

                for (var i = 0; i < vertexStates; i++)
                {
                    var state = new T3GFXVertexState();
                    stream.Serialize(ref state);
                    obj.VertexStates.Add(state);
                }
            }
        }
    }
}

[MetaSerializer(typeof(MetaClassSerializer<T3MeshBonePaletteEntry>))]
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

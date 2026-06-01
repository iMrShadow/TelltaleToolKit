using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OcclusionMeshData>))]
public class T3OcclusionMeshData
{
    [MetaMember("mData")]
    public BinaryBuffer? Data { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mBatches")]
    public List<T3OcclusionMeshBatch>? Batches { get; set; }

    [MetaMember("mVertexCount")]
    public uint VertexCount { get; set; }
}

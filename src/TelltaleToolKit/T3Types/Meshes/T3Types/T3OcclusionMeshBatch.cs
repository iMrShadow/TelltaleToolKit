using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3OcclusionMeshBatch>))]
public class T3OcclusionMeshBatch
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();

    [MetaMember("mStartIndex")]
    public long StartIndex { get; set; }

    [MetaMember("mNumTriangles")]
    public long NumTriangles { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3OcclusionMeshBatch>))]
public class T3OcclusionMeshBatch
{
    [MetaMember("mFlags")]
    public Flags Flags { get; set; } = new();

    [MetaMember("mStartIndex")]
    public long StartIndex { get; set; }

    [MetaMember("mNumTriangles")]
    public long NumTriangles { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MeshBoneEntry>))]
public class T3MeshBoneEntry
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

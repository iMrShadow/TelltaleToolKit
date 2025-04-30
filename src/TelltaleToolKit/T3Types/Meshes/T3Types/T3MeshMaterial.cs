using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshMaterial>))]
public class T3MeshMaterial
{
    [MetaMember("mhMaterial")]
    public Handle<PropertySet> Material { get; set; }

    [MetaMember("mBaseMaterialName")]
    public Symbol BaseMaterialName { get; set; }

    [MetaMember("mLegacyRenderTextureProperty")]
    public Symbol LegacyRenderTextureProperty { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }
}
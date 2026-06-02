using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MeshMaterialOverride>))]

public class T3MeshMaterialOverride
{
    [MetaMember("mhOverrideMaterial")]
    public Handle<PropertySet> OverrideMaterial { get; set; } = new();

    [MetaMember("mMaterialIndex")]
    public uint MaterialIndex { get; set; }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshMaterialOverride>))]

public class T3MeshMaterialOverride
{
    [MetaMember("mhOverrideMaterial")]
    public Handle<PropertySet> OverrideMaterial { get; set; } = new();

    [MetaMember("mMaterialIndex")]
    public uint MaterialIndex { get; set; }
}

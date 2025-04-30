using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialNestedMaterial>))]

public class T3MaterialNestedMaterial
{
    [MetaMember("mhMaterial")]
    public Handle<PropertySet> Material { get; set; }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialStaticParameter>))]
public class T3MaterialStaticParameter
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mNestedMaterialIndex")]
    public int NestedMaterialIndex { get; set; }
}

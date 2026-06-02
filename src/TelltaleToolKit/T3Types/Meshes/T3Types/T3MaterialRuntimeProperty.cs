using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MaterialRuntimeProperty>))]
public class T3MaterialRuntimeProperty
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mRuntimeName")]
    public Symbol RuntimeName { get; set; }
}

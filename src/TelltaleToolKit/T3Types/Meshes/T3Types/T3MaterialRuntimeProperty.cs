using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialRuntimeProperty>))]
public class T3MaterialRuntimeProperty
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mRuntimeName")]
    public Symbol RuntimeName { get; set; }
}
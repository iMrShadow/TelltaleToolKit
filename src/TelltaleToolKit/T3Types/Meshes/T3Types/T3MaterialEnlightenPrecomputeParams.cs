using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialEnlightenPrecomputeParams>))]
public class T3MaterialEnlightenPrecomputeParams
{
    [MetaMember("mIndirectReflectivity")]
    public float IndirectReflectivity { get; set; }= 1.0f;

    [MetaMember("mIndirectTransparency")]
    public float IndirectTransparency { get; set; }
}

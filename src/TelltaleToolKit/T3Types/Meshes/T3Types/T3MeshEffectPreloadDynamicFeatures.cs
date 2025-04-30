using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshEffectPreloadDynamicFeatures>))]
public class T3MeshEffectPreloadDynamicFeatures
{
    [MetaMember("mDynamicFeatures")]
    public BitSetBase DynamicFeatures { get; set; }

    [MetaMember("mPriority")]
    public int Priority { get; set; }
}
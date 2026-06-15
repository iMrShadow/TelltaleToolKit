using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MeshEffectPreloadDynamicFeatures>))]
public class T3MeshEffectPreloadDynamicFeatures
{
    [MetaMember("mDynamicFeatures")]
    public BitSetBase DynamicFeatures { get; set; }

    [MetaMember("mPriority")]
    public int Priority { get; set; }
}

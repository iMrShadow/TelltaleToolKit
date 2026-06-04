using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous.PreloadPackage;

[MetaSerializer(typeof(MetaClassSerializer<RuntimeDataScene>))]
public class RuntimeDataScene
{
    [MetaMember("mResources")]
    public List<ResourceKey> Resources { get; set; } = [];
}

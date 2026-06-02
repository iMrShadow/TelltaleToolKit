using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ResourceGroups>))]
public class ResourceGroups
{
    // String and symbol at the same time
    [MetaMember("mGroups")]
    public Dictionary<Symbol, float> Groups { get; set; } = new();
}

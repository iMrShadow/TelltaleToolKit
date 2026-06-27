using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(MetaClassSerializer<ResourceGroups>))]
public class ResourceGroups
{
    [MetaMember("mGroups")]
    public Dictionary<Symbol, float> GroupsS { get; set; } = [];

    [MetaMember("mGroups")]
    public Dictionary<string, float> Groups { get; set; } = [];
}

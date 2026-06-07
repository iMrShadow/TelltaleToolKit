using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<ClipResourceFilter>))]
public class ClipResourceFilter
{
    [MetaMember("mResources")]
    public HashSet<Symbol> Resources { get; set; } = [];
    [MetaMember("mbExclusiveMode")]
    public bool ExclusiveMode { get; set; }
}

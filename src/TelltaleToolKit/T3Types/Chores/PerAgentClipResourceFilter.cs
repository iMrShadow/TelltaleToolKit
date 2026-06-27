using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<PerAgentClipResourceFilter>))]
public class PerAgentClipResourceFilter
{
    [MetaMember("mIncludedAgents")]
    public Dictionary<string, ClipResourceFilter> IncludedAgents { get; set; } = [];

    [MetaMember("mExcludedAgents")]
    public HashSet<string> ExcludedAgents { get; set; } = [];
    [MetaMember("mbExclusiveMode")]
    public bool ExclusiveMode { get; set; }
}

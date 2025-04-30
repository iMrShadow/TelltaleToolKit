using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.ActorMaps;

/// <summary>
/// Represents the class for .amap files.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AgentMap>))]
public class AgentMap
{
    [MetaMember("maAgents")]
    public Dictionary<string, AgentMapEntry> Agents { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AgentMapEntry>))]
    public class AgentMapEntry
    {
        [MetaMember("mzName")]
        public string Name { get; set; } = string.Empty;

        [MetaMember("mzActor")]
        public string Actor { get; set; } = string.Empty;

        [MetaMember("mazModels")]
        public HashSet<string> Models { get; set; } = [];

        [MetaMember("mazGuides")]
        public HashSet<string> Guides { get; set; } = [];

        [MetaMember("mazStyleIdles")]
        public HashSet<string> StyleIdles { get; set; } = [];
    }
}
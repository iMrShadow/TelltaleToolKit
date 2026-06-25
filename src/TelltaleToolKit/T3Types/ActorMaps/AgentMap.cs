using System.Collections.Generic;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.ActorMaps;

/// <summary>
/// Represents the class for .amap files.
/// </summary>
[MetaSerializer(typeof(MetaClassSerializer<AgentMap>))]
public class AgentMap
{
    [MetaMember("maAgents")]
    public Dictionary<string, AgentMapEntry> Agents { get; set; } = new();

    [MetaSerializer(typeof(MetaClassSerializer<AgentMapEntry>))]
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

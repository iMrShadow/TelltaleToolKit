using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<ActorAgentBinding>))]
public class ActorAgentBinding
{
    [MetaMember("mActorName")]
    public string ActorName { get; set; } = string.Empty;

    [MetaMember("mActorName")]
    public Symbol ActorNameS { get; set; }

    [MetaMember("mAgentName")]
    public string AgentName { get; set; } = string.Empty;

    [MetaMember("mAgentName")]
    public Symbol mAgentNameS { get; set; }
}

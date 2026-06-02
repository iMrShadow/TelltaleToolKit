using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ActorAgentBinding>))]
public class ActorAgentBinding
{
    [MetaMember("mActorName")]
    public string ActorName { get; set; } = string.Empty;
}

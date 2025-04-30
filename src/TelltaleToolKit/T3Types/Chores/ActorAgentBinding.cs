using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ActorAgentBinding>))]
public class ActorAgentBinding
{
    [MetaMember("mActorName")]
    public string ActorName { get; set; } = string.Empty;
}
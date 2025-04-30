using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.ActorMaps;

/// <summary>
/// Represents the main class for .aam files.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ActorAgentMapper>))]
public class ActorAgentMapper
{
    [MetaMember("mActorAgentMap")]
    public PropertySet ActorAgentMap { get; set; } = new();

    [MetaMember("mActionActors")]
    public HashSet<string> ActionActors { get; set; } = [];
}
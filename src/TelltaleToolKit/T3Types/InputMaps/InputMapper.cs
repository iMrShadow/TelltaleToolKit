using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.InputMaps;

[MetaSerializer(typeof(MetaClassSerializer<InputMapper>))]
public class InputMapper
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mMappedEvents")]
    public List<EventMapping> MappedEvents { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<EventMapping>))]
    public class EventMapping
    {
        [MetaMember("mInputCode")]
        public InputCode InputCode { get; set; }

        [MetaMember("mEvent")]
        public EventType Event { get; set; }

        [MetaMember("mScriptFunction")]
        public string ScriptFunction { get; set; } = string.Empty;

        [MetaMember("mControllerIndexOverride")] // new games
        public int ControllerIndexOverride { get; set; }
    }

    [MetaSerializer(typeof(EnumSerializer<EventType>))]
    public enum EventType
    {
        ForceDword = -1,
        BeginEvent = 0,
        EndEvent = 1,
        MouseMove = 2,
        EitherEvent = 3,
    }

    public class RawEvent
    {
    }
}

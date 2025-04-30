using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.InputMaps;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<InputMapper>))]
public class InputMapper
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mMappedEvents")]
    public List<EventMapping> MappedEvents { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EventMapping>))]
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

    [MetaClassSerializerGlobal(typeof(EnumSerializer<EventType>))]
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
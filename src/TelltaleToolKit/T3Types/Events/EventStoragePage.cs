using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Events;

//.EPAGE FILES
[MetaSerializer(typeof(Serializer))]
public class EventStoragePage
{
    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mSessionID")]
    public ulong SessionID { get; set; }

    [MetaMember("mFlushedNameOnDisk")]
    public string FlushedNameOnDisk { get; set; } = string.Empty;

    public List<EventLoggerEvent> Events = [];

    public class Serializer : MetaSerializer<EventStoragePage>
    {
        private static readonly MetaClassSerializer<EventStoragePage> s_metaClassSaveGameSerializer = new();

        public override void Serialize(ref EventStoragePage obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSaveGameSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Events.Count);
                foreach (var loggerEvent in obj.Events)
                {
                    EventLoggerEvent eventLoggerEvent = loggerEvent;
                    stream.Serialize(ref eventLoggerEvent);
                }
            }
            else
            {
                int eventCount = stream.ReadInt32();
                stream.BeginObject("Events", true);
                for (int i = 0; i < eventCount; i++)
                {
                    var loggerEvent = new EventLoggerEvent();
                    stream.Serialize(ref loggerEvent);
                    obj.Events.Add(loggerEvent);
                }

                stream.EndObject("Events");
            }
        }
    }
}

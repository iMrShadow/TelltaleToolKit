using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Events;

[MetaSerializer(typeof(Serializer))]
public class EventLoggerEvent
{
    [MetaMember("mEventID")]
    public uint EventID { get; set; }

    [MetaMember("mMaxSeverity")]
    public int MaxSeverity { get; set; }

    public class TypeHeader
    {
        [MetaMember("mType")]
        public Symbol Type { get; set; } = Symbol.Empty;

        [MetaMember("mData")]
        public List<EventData> Data { get; set; } = [];
    }

    public List<TypeHeader> EventData { get; set; } = [];

    public class Serializer : MetaSerializer<EventLoggerEvent>
    {
        private static readonly MetaClassSerializer<EventLoggerEvent> s_metaClassSaveGameSerializer = new();

        public override void Serialize(ref EventLoggerEvent obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSaveGameSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                stream.BeginBlock();
                uint dataCount = stream.ReadUInt32();
                uint childCount = stream.ReadUInt32();

                for (uint i = 0; i < dataCount; i++)
                {
                    TypeHeader header = new() { Type = stream.ReadSymbol() };

                    int count = stream.ReadInt32();

                    for (int j = 0; j < count; j++)
                    {
                        EventData eventData = new() { DataType = (EventDataType)stream.ReadByte() };
                        header.Data.Add(eventData);
                    }

                    obj.EventData.Add(header);
                }

                foreach (var header in obj.EventData)
                {
                    stream.BeginObject(header.Type.ToString(), true);

                    foreach (var eventData in header.Data)
                    {
                        stream.BeginObject("EventData");
                        switch (eventData.DataType)
                        {
                            case EventDataType.eEventData_Symbol:
                                eventData.DataSymbol = stream.ReadSymbol();
                                break;
                            case EventDataType.eEventData_Int:
                                eventData.DataInt = stream.ReadInt64();
                                break;
                            case EventDataType.eEventData_Double:
                                eventData.DataDouble = stream.ReadDouble();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        stream.EndObject("EventData");
                        stream.BeginObject("EventSeverity");
                        eventData.Severity = stream.ReadByte();
                        stream.EndObject("EventSeverity");
                    }

                    stream.EndObject(header.Type.ToString());
                }

                stream.EndBlock();
            }
        }
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Events;

[MetaSerializer(typeof(Serializer))]
public class EventStorage
{
    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mSessionID")]
    public ulong SessionID { get; set; }

    [MetaMember("mPages")]
    public List<PageEntry> Pages { get; set; } = [];

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mLastEventID")]
    public uint LastEventID { get; set; }

    [MetaMember("mEventStoragePageSize")]
    public int EventStoragePageSize { get; set; }

    public EventStoragePage? CurrentPage { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<PageEntry>))]
    public class PageEntry
    {
        [MetaMember("mhPage")]
        public Handle<EventStoragePage> Page { get; set; } = new();

        [MetaMember("mMaxEventID")]
        public uint MaxEventID { get; set; }
    }

    public class Serializer : MetaSerializer<EventStorage>
    {
        private static readonly MetaClassSerializer<EventStorage> s_metaClassSaveGameSerializer = new();

        public override void Serialize(ref EventStorage obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSaveGameSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                bool hasPage = stream.ReadBoolean();
                if (!hasPage)
                {
                    return;
                }

                var page = new EventStoragePage();
                stream.Serialize(ref page);
                obj.CurrentPage = page;
            }
        }
    }
}

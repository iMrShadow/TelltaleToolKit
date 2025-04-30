using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class Note : IGenerator, IOwner
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; }

    [MetaMember("Baseclass_UID::Owner")]
    public Owner Owner { get; set; }
    
    [MetaMember("mEntries")]
    public List<Entry> Entries { get; set; } = [];

    [MetaMember("mName")]
    public string Name { get; set; }

    public class Entry : IGenerator, IOwner
    {
        [MetaMember("Baseclass_UID::Generator")]
        public Generator Generator { get; set; }

        [MetaMember("Baseclass_UID::Owner")]
        public Owner Owner { get; set; }

        [MetaMember("mAuthor")]
        public string Author { get; set; }

        [MetaMember("mStamp")]
        public DateStamp Stamp { get; set; }

        [MetaMember("mCategory")]
        public string Category { get; set; }

        [MetaMember("mText")]
        public string Text { get; set; }
    }

    public class Serializer : MetaClassSerializer<Note>
    {
        private static readonly DefaultClassSerializer<Note> DefaultClassSerializer = new();

        public override void Serialize(ref Note obj, MetaStream stream)
        {
            DefaultClassSerializer.PreSerialize(ref obj, stream);
            DefaultClassSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // mEntries is not serialized.
                int numEntries = streamReader.ReadInt32();
                for (var i = 0; i < numEntries; i++)
                {
                    Entry? entry = null;
                    TTK.PreSerialize(ref entry, stream);
                    TTK.Serialize(ref entry, stream);
                    obj.Entries.Add(entry);
                }
            }
        }
    }
}
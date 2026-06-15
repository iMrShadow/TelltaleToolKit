using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
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

    public class Serializer : MetaSerializer<Note>
    {
        private static readonly MetaClassSerializer<Note> s_metaClassSerializer = new();

        public override void Serialize(ref Note obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                // mEntries is not serialized.
                int numEntries = stream.ReadInt32();
                for (var i = 0; i < numEntries; i++)
                {
                    Entry? entry = null;
                    stream.Serialize(ref entry);
                    obj.Entries.Add(entry);
                }
            }
        }
    }
}

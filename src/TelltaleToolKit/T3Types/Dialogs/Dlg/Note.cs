using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class Note : IGenerator, IOwner
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; } = new();

    [MetaMember("Baseclass_UID::Owner")]
    public Owner Owner { get; set; } = new();

    [MetaMember("mEntries")]
    public List<Entry> Entries { get; set; } = [];

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaSerializer(typeof(MetaClassSerializer<Entry>))]
    public class Entry : IGenerator, IOwner
    {
        [MetaMember("Baseclass_UID::Generator")]
        public Generator Generator { get; set; } = new();

        [MetaMember("Baseclass_UID::Owner")]
        public Owner Owner { get; set; } = new();

        [MetaMember("mAuthor")]
        public string Author { get; set; } = string.Empty;

        [MetaMember("mStamp")]
        public DateStamp Stamp { get; set; } = new();

        [MetaMember("mCategory")]
        public string Category { get; set; } = string.Empty;

        [MetaMember("mText")]
        public string Text { get; set; } = string.Empty;
    }

    public class Serializer : MetaSerializer<Note>
    {
        private static readonly MetaClassSerializer<Note> s_metaClassSerializer = new();

        public override void Serialize(ref Note obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Entries.Count);

                foreach (var entry in obj.Entries)
                {
                    Entry entry1 = entry;
                    stream.Serialize(ref entry1);
                }
            }
            else
            {
                int numEntries = stream.ReadInt32();
                for (int i = 0; i < numEntries; i++)
                {
                    Entry entry = new();
                    stream.Serialize(ref entry);
                    obj.Entries.Add(entry);
                }
            }
        }
    }
}

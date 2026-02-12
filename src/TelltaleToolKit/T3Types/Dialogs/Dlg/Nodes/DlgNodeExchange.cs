using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DlgNodeExchange : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaMember("mPriority")]
    public float Priority { get; set; }

    [MetaMember("mhChore")]
    public Handle<Chore> Chore { get; set; }

    [MetaMember("mEntries")]
    public List<Entry> Entries { get; set; }

    public NoteCollection Notes { get; set; }
    public DlgLineCollection Lines { get; set; }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<EntryType>))]
    public enum EntryType
    {
        Line = 1,
        Note = 2
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Entry>))]
    public class Entry
    {
        [MetaMember("mID")]
        public int Id { get; set; }

        [MetaMember("mType")]
        public EntryType Type { get; set; }
    }

    public class Serializer : MetaClassSerializer<DlgNodeExchange>
    {
        private static readonly DefaultClassSerializer<DlgNodeExchange> DefaultClassSerializer = new();

        public override void Serialize(ref DlgNodeExchange obj, MetaStream stream)
        {
            DefaultClassSerializer.PreSerialize(ref obj, stream);
            DefaultClassSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }

            if (stream is MetaStreamReader)
            {
                if ((obj.DlgNode.Flags.Data & 1) != 0)
                {
                    var notes = new NoteCollection();
                    stream.PreSerialize(ref notes);
                    stream.Serialize(ref notes);
                    obj.Notes = notes;
                }

                if ((obj.DlgNode.Flags.Data & 2) != 0)
                {
                    var lines = new DlgLineCollection();
                    stream.PreSerialize(ref lines);
                    stream.Serialize(ref lines);
                    obj.Lines = lines;
                }
            }
        }
    }
}
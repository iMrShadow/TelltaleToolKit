using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(Serializer))]
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

    [MetaSerializer(typeof(EnumSerializer<EntryType>))]
    public enum EntryType
    {
        Line = 1,
        Note = 2
    }

    [MetaSerializer(typeof(MetaClassSerializer<Entry>))]
    public class Entry
    {
        [MetaMember("mID")]
        public int Id { get; set; }

        [MetaMember("mType")]
        public EntryType Type { get; set; }
    }

    public class Serializer : MetaSerializer<DlgNodeExchange>
    {
        private static readonly MetaClassSerializer<DlgNodeExchange> s_metaClassSerializer = new();

        public override void Serialize(ref DlgNodeExchange obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            } else
            {
                if ((obj.DlgNode.Flags.Data & 1) != 0)
                {
                    var notes = new NoteCollection();
                    stream.Serialize(ref notes);
                    obj.Notes = notes;
                }

                if ((obj.DlgNode.Flags.Data & 2) != 0)
                {
                    var lines = new DlgLineCollection();
                    stream.Serialize(ref lines);
                    obj.Lines = lines;
                }
            }
        }
    }
}

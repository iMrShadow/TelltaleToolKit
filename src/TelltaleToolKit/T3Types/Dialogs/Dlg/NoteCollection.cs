using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class NoteCollection : IGenerator
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; }

    [MetaMember("mNotes")]
    public Dictionary<int, Note> Notes { get; set; } = new();

    public class Serializer : MetaClassSerializer<NoteCollection>
    {
        public override void Serialize(ref NoteCollection obj, MetaStream stream)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                // mEntries is not serialized.
                int numNotes = stream.ReadInt32();
                for (var i = 0; i < numNotes; i++)
                {
                    Note? note = null;
                    stream.Serialize(ref note);
                    // TODO: Set the correct IDs. Not a priority, I haven't seen this serialized anywhere.
                    obj.Notes.Add(i, note);
                }
            }
        }
    }
}

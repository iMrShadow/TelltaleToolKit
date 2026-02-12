using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
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
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }

            if (stream is MetaStreamReader streamReader)
            {
                // mEntries is not serialized.
                int numNotes = streamReader.ReadInt32();
                for (var i = 0; i < numNotes; i++)
                {
                    Note? note = null;
                    stream.PreSerialize(ref note);
                    stream.Serialize(ref note);
                    // TODO: Set the correct IDs. Not a priority, I haven't seen this serialized anywhere.
                    obj.Notes.Add(i, note);
                }
            }
        }
    }
}
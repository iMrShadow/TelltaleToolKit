using System.Collections.Generic;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class NoteCollection : IGenerator
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; } = new();

    [MetaMember("mNotes")]
    public Dictionary<int, Note> Notes { get; set; } = new();

    public class Serializer : MetaSerializer<NoteCollection>
    {
        public override void Serialize(ref NoteCollection obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Notes.Count);
                foreach (var note in obj.Notes.Values)
                {
                    Note note1 = note;
                    stream.Serialize(ref note1);
                }
            }
            else
            {
                int numNotes = stream.ReadInt32();
                for (int i = 0; i < numNotes; i++)
                {
                    Note note = new();
                    stream.Serialize(ref note);
                    obj.Notes.Add(i, note);
                }
            }
        }
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class JiraRecordManager
{
    public Dictionary<string, JiraRecord> Records = [];

    public class Serializer : MetaSerializer<JiraRecordManager>
    {
        public override void PreSerialize(ref JiraRecordManager? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new JiraRecordManager();
        }

        public override void Serialize(ref JiraRecordManager obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.BeginDebugSection())
            {
                if (stream.Mode is MetaStreamMode.Write)
                {
                    stream.Write(obj.Records.Count);

                    foreach (string name in obj.Records.Keys)
                    {
                        stream.Write(name);
                    }
                }
                else
                {
                    int numRecords = stream.ReadInt32();

                    for (int i = 0; i < numRecords; i++)
                    {
                        string key = stream.ReadString();
                        var record = new JiraRecord();

                        obj.Records[key] = record;
                    }
                }
            }

            stream.EndDebugSection();
        }
    }
}

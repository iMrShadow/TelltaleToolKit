using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;
// TODO:

[MetaSerializer(typeof(Serializer))]
public class JiraRecordManager
{
    public Dictionary<string, JiraRecord> Records = new();

    public class Serializer : MetaSerializer<JiraRecordManager>
    {
        public override void PreSerialize(ref JiraRecordManager? obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj == null)
            {
                obj = new JiraRecordManager();
            }
        }

        public override void Serialize(ref JiraRecordManager obj, MetaStream stream)
        {
            stream.BeginDebugSection();

            if (stream.IsSectionEmpty())
            {
                stream.EndDebugSection();
                return;
            }

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int numRecords = stream.ReadInt32();

                for (int i = 0; i < numRecords; i++)
                {
                    string key = stream.ReadString();
                    var record = new JiraRecord();

                    obj.Records[key] = record;
                }
            }

            stream.EndDebugSection();
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

// TODO:

[MetaClassSerializerGlobal(typeof(Serializer))]
public class JiraRecordManager
{
    public Dictionary<string, JiraRecord> Records = [];

    public class Serializer : MetaClassSerializer<JiraRecordManager>
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

            if (stream is MetaStreamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                var numRecords = streamReader.ReadInt32();
                
                for (int i = 0; i < numRecords; i++)
                {
                    var key = streamReader.ReadString();
                    var record = new JiraRecord();

                    obj.Records[key] = record;
                }
            }

            stream.EndDebugSection();
        }
    }
}
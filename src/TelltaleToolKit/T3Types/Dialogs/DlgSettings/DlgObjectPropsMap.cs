using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.DlgSettings;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DlgObjectPropsMap : IGenerator
{
    public List<GroupDefinition> GroupDefinitions { get; set; }

    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; }

    public class GroupDefinition : IOwner
    {
        [MetaMember("mVer")]
        public int Ver { get; set; } = 2;

        [MetaMember("mGroupCat")]
        public int GroupCat { get; set; } = 1000;

        [MetaMember("mhProps")]
        public Handle<PropertySet> Props { get; set; }

        [MetaMember("Baseclass_UID::Owner")]
        public Owner Owner { get; set; }
    }

    public class Serializer : MetaClassSerializer<DlgObjectPropsMap>
    {
        public override void Serialize(ref DlgObjectPropsMap obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int groupDefinitionSize = streamReader.ReadInt32();

                for (var i = 0; i < groupDefinitionSize; i++)
                {
                    var groupDefinition = new GroupDefinition();
                    TTK.PreSerialize(ref groupDefinition, stream);
                    TTK.Serialize(ref groupDefinition, stream);
                    obj.GroupDefinitions.Add(groupDefinition);
                }
            }
        }
    }
}
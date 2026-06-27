using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.DlgSettings;

[MetaSerializer(typeof(Serializer))]
public class DlgObjectPropsMap : IGenerator
{
    [MetaMember("Baseclass_UID::Generator")]
    public Generator Generator { get; set; }

    public List<GroupDefinition> GroupDefinitions { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<GroupDefinition>))]
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

    public class Serializer : MetaSerializer<DlgObjectPropsMap>
    {
        public override void PreSerialize(ref DlgObjectPropsMap? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new DlgObjectPropsMap();
        }

        public override void Serialize(ref DlgObjectPropsMap obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int groupDefinitionSize = stream.ReadInt32();

                for (var i = 0; i < groupDefinitionSize; i++)
                {
                    var groupDefinition = new GroupDefinition();
                    stream.Serialize(ref groupDefinition);
                    obj.GroupDefinitions.Add(groupDefinition);
                }
            }
        }
    }
}

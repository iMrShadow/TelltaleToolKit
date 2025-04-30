using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DlgConditionSet
{
    public List<IDlgCondition> Conditions { get; set; }
    
    public class Serializer : MetaClassSerializer<DlgConditionSet>
    {
        public override void Serialize(ref DlgConditionSet obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int numChildren = streamReader.ReadInt32();

                for (var i = 0; i < numChildren; i++)
                {
                    MetaClassType type = streamReader.ReadMetaClassType();
                    MetaClassSerializer conditionSerializer = TTKContext.Instance().GetSerializer(type.LinkingType);

                    object? dlgConditionSet = null;
                    conditionSerializer.PreSerialize(ref dlgConditionSet, stream);
                    conditionSerializer.Serialize(ref dlgConditionSet, stream);

                    if (dlgConditionSet is IDlgCondition condition)
                        obj.Conditions.Add(condition);
                }
            }
        }
    }
}
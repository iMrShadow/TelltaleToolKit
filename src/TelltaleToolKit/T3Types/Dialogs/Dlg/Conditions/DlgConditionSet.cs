using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DlgConditionSet
{
    public List<IDlgCondition> Conditions { get; set; } = [];

    public class Serializer : MetaClassSerializer<DlgConditionSet>
    {
        public override void PreSerialize(ref DlgConditionSet obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new DlgConditionSet();
        }


        public override void Serialize(ref DlgConditionSet obj, MetaStream stream)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int numChildren = stream.ReadInt32();

                for (var i = 0; i < numChildren; i++)
                {
                    MetaClassType? type = stream.ReadMetaClassType();
                    if (type == null)
                        throw new InvalidOperationException("[DlgConditionSet] Type is not registered.");

                    MetaClassSerializer conditionSerializer = Toolkit.Instance.GetSerializer(type.LinkingType);

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

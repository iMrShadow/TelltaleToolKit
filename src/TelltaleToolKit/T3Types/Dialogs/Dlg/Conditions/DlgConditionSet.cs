using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class DlgConditionSet
{
    public List<IDlgCondition> Conditions { get; set; } = [];

    public class Serializer : MetaSerializer<DlgConditionSet>
    {
        public override void PreSerialize(ref DlgConditionSet? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new DlgConditionSet();
        }


        public override void Serialize(ref DlgConditionSet obj, MetaStream stream, MetaClassType? type = null)
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
                    MetaClassType? typeS = stream.ReadMetaClassType();
                    if (typeS == null)
                        throw new InvalidOperationException("[DlgConditionSet] Type is not registered.");

                    MetaSerializer conditionSerializer = Toolkit.Instance.GetSerializer(typeS.LinkingType);

                    object? dlgConditionSet = null;
                    conditionSerializer.PreSerialize(ref dlgConditionSet, stream, typeS);
                    conditionSerializer.Serialize(ref dlgConditionSet, stream, typeS);

                    if (dlgConditionSet is IDlgCondition condition)
                        obj.Conditions.Add(condition);
                }
            }
        }
    }
}

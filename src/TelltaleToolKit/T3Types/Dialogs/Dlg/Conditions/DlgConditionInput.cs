using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgConditionInput>))]
public class DlgConditionInput : IDlgCondition
{
    [MetaMember("Baseclass_DlgCondition")]
    public DlgCondition DlgCondition { get; set; }
}

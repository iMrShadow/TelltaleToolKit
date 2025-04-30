using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgConditionInput>))]
public class DlgConditionInput : IDlgCondition
{
    [MetaMember("Baseclass_DlgCondition")]
    public DlgCondition DlgCondition { get; set; }
}
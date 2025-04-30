using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Rules;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgConditionRule>))]
public class DlgConditionRule : IDlgCondition
{
    [MetaMember("Baseclass_DlgCondition")]
    public DlgCondition DlgCondition { get; set; }

    [MetaMember("mRule")]
    public Rule Rule { get; set; }
}
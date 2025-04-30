using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgVisibilityConditionsOwner>))]
public class DlgVisibilityConditionsOwner
{
    [MetaMember("mVisCond")]
    public DlgVisibilityConditions VisCond { get; set; }
}
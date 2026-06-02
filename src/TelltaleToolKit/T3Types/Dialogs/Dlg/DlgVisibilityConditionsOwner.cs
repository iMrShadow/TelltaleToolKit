using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgVisibilityConditionsOwner>))]
public class DlgVisibilityConditionsOwner
{
    [MetaMember("mVisCond")]
    public DlgVisibilityConditions VisCond { get; set; }
}

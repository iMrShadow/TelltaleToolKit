using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(MetaClassSerializer<DlgChild>))]
public class DlgChild : IDlgChainHead, IDlgVisibilityConditionsOwner, IDlgObjectPropsOwner
{
    [MetaMember("Baseclass_DlgChainHead")]
    public DlgChainHead DlgChainHead { get; set; }

    [MetaMember("Baseclass_DlgVisibilityConditionsOwner")]
    public DlgVisibilityConditionsOwner DlgVisibilityConditionsOwner { get; set; }

    [MetaMember("Baseclass_DlgObjectPropsOwner")]
    public DlgObjectPropsOwner DlgObjectPropsOwner { get; set; }

    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mParent")]
    public DlgNodeLink Parent { get; set; }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgChild>))]
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
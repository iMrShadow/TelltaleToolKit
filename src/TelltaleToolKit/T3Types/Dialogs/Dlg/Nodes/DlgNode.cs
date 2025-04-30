using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNode>))]
public class DlgNode : IDlgObjIdOwner, IDlgObjectPropsOwner, IDlgVisibilityConditionsOwner, ITaskOwner
{
    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }

    [MetaMember("Baseclass_DlgObjectPropsOwner")]
    public DlgObjectPropsOwner DlgObjectPropsOwner { get; set; }

    [MetaMember("Baseclass_DlgVisibilityConditionsOwner")]
    public DlgVisibilityConditionsOwner DlgVisibilityConditionsOwner { get; set; }

    //   public DlgNode DlgNode { get; set; }

    [MetaMember("Baseclass_TaskOwner")]
    public TaskOwner TaskOwner { get; set; }

    [MetaMember("mPrev")]
    public DlgNodeLink Prev { get; set; }

    [MetaMember("mNext")]
    public DlgNodeLink Next { get; set; }

    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mChainContextTypeID")]
    public ChainContextTypeID ChainContextTypeID { get; set; }
}
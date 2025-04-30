using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeCancelChoices>))]
public class DlgNodeCancelChoices : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<CancelGroupT>))]
    public enum CancelGroupT
    {
        AllActiveChoices = 1,
    }

    [MetaMember("mCancelGroup")]
    public CancelGroupT CancelGroup { get; set; }
}
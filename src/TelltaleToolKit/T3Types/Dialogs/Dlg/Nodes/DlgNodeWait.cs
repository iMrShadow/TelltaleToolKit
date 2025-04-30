using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeWait>))]
public class DlgNodeWait : IDlgNode, IDlgConditionSet
{
    [MetaMember("Baseclass_DlgNode")]

    public DlgNode DlgNode { get; set; }

    [MetaMember("Baseclass_DlgConditionSet")]

    public DlgConditionSet DlgConditionSet { get; set; }
}
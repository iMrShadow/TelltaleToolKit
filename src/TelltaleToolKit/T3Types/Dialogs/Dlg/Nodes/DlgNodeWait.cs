using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeWait>))]
public class DlgNodeWait : IDlgNode, IDlgConditionSet
{
    [MetaMember("Baseclass_DlgNode")]

    public DlgNode DlgNode { get; set; }

    [MetaMember("Baseclass_DlgConditionSet")]

    public DlgConditionSet DlgConditionSet { get; set; }
}

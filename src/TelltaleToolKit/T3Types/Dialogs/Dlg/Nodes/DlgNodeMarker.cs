using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeMarker>))]
public class DlgNodeMarker : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }
}

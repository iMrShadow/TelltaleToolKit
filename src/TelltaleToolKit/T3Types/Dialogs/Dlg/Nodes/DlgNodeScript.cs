using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeScript>))]
public class DlgNodeScript : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaMember("mScriptText")]
    public string ScriptText { get; set; }

    [MetaMember("mbBlocking")]
    public bool Blocking { get; set; }

    [MetaMember("mbExecuteOnInstanceRetire")]
    public bool ExecuteOnInstanceRetire { get; set; }
}

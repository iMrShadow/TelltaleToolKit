using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeScript>))]
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
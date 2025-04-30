using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeChore>))]
public class DlgNodeChore : IDlgNode
{
    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }
    
    [MetaMember("mChore")]
    public Handle<Chore> Chore { get; set; }

    [MetaMember("mPriority")]
    public int Priority { get; set; }

    [MetaMember("mLooping")]
    public bool Looping { get; set; }
}
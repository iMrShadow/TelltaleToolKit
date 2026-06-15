using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeChore>))]
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

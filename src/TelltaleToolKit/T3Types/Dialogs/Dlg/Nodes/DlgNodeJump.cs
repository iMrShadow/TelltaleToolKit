using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeJump>))]
public class DlgNodeJump : IDlgNode
{
    [MetaSerializer(typeof(EnumSerializer<JumpBehaviour>))]
    public enum JumpBehaviour
    {
        JumpAndExecute = 1,
        JumpExecuteAndReturn = 2,
        Return = 3
    }

    [MetaSerializer(typeof(EnumSerializer<JumpTargetClass>))]
    public enum JumpTargetClass
    {
        ToName = 1,
        ToParent = 2,
        ToNodeAfterParentWaitNode = 3
    }

    [MetaSerializer(typeof(EnumSerializer<VisibilityBehaviour>))]
    public enum VisibilityBehaviour
    {
        IgnoreVisibility = 1,
        ObeyVisibility = 2,
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumJumpBehaviour>))]
    public struct EnumJumpBehaviour
    {
       [MetaMember("mVal")]
       public JumpBehaviour Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumJumpTargetClass>))]
    public struct  EnumJumpTargetClass
    {
       [MetaMember("Val")]
       public JumpTargetClass Val { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumVisibilityBehaviour>))]
    public struct  EnumVisibilityBehaviour
    {
        [MetaMember("mVal")]
        public VisibilityBehaviour Val { get; set; }
    }

    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; } = new();

    [MetaMember("mJumpToLink")]
    public DlgNodeLink JumpToLink { get; set; } = new();

    [MetaMember("mJumpToName")]
    public Symbol JumpToName { get; set; } = Symbol.Empty;

    [MetaMember("mJumpTargetClass")]
    public JumpTargetClass JumpTargetClassE { get; set; }

    [MetaMember("mJumpBehavior")]
    public JumpBehaviour JumpBehaviourE { get; set; }

    [MetaMember("mVisibilityBehavior")]
    public VisibilityBehaviour VisibilityBehaviourE { get; set; }

    [MetaMember("mChoiceTransparency")]
    public int ChoiceTransparency { get; set; }

    [MetaMember("mhJumpToDlg")]
    public Handle<Dlg> JumpToDlg { get; set; } = new();
}

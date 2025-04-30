using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeJump>))]
public class DlgNodeJump : IDlgNode
{
    // TODO: Not exactly the right names.
    [MetaClassSerializerGlobal(typeof(EnumSerializer<JumpBehaviourEnum>))]
    public enum JumpBehaviourEnum
    {
        JumpAndExecute = 1,
        JumpExecuteAndReturn = 2,
        Return = 3
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<JumpTargetClassEnum>))]
    public enum JumpTargetClassEnum
    {
        ToName = 1,
        ToParent = 2,
        ToNodeAfterParentWaitNode = 3
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<VisibilityBehaviourEnum>))]
    public enum VisibilityBehaviourEnum
    {
        IgnoreVisibility = 1,
        ObeyVisibility = 2,
    }

    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }
    
    [MetaMember("mJumpToLink")]
    public DlgNodeLink JumpToLink { get; set; }

    [MetaMember("mJumpToName")]
    public Symbol JumpToName { get; set; }

    [MetaMember("mJumpTargetClass")]
    public JumpTargetClassEnum JumpTargetClass { get; set; }

    [MetaMember("mJumpBehavior")]
    public JumpBehaviourEnum JumpBehaviour { get; set; }

    [MetaMember("mVisibilityBehavior")]
    public VisibilityBehaviourEnum VisibilityBehaviour { get; set; }

    [MetaMember("mChoiceTransparency")]
    public int ChoiceTransparency { get; set; }

    [MetaMember("mhJumpToDlg")]
    public Handle<Dlg> JumpToDlg { get; set; }
}
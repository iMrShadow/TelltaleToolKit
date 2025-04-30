using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgNodeIdle>))]
public class DlgNodeIdle : IDlgNode
{
    [MetaClassSerializerGlobal(typeof(EnumSerializer<OverrideOption>))]
    public enum OverrideOption
    {
        UseDefaults = 1,
        Override = 2
    }

    [MetaMember("mhIdle")]
    public Handle<Chore> Idle { get; set; }

    [MetaMember("mTransitionTimeOverride")]
    public float TransitionTimeOverride { get; set; }

    [MetaMember("mTransitionStyleOverride")]
    public int TransitionStyleOverride { get; set; }

    [MetaMember("mIdleSlot")]
    public int IdleSlot { get; set; }

    [MetaMember("mOverrideOptionTime")]
    public EnumOverrideOption OverrideOptionTime { get; set; }

    [MetaMember("mOverrideOptionStyle")]
    public EnumOverrideOption OverrideOptionStyle { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumOverrideOption>))]
    public struct EnumOverrideOption
    {
        [MetaMember("mVal")]
        public OverrideOption Value { get; set; }
    }

    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }
}
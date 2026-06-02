using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;

[MetaSerializer(typeof(MetaClassSerializer<DlgNodeSequence>))]
public class DlgNodeSequence : IDlgNode
{
    [MetaSerializer(typeof(EnumSerializer<PlaybackModeT>))]
    public enum PlaybackModeT
    {
        Sequential = 1,
        Shuffle = 2
    }

    [MetaSerializer(typeof(EnumSerializer<PlayPositionT>))]
    public enum PlayPositionT
    {
        Unspecified = 1,
        First = 2,
        Last = 3
    }

    [MetaSerializer(typeof(EnumSerializer<RepeatT>))]
    public enum RepeatT
    {
        Indefinitely = 1,
        One = 2,
        Two = 3,
        Three = 4,
        Four = 5,
        Five = 6,
        Six = 7,
        MaxPlusOne = 8
    }

    [MetaSerializer(typeof(EnumSerializer<LifetimeModeT>))]
    public enum LifetimeModeT
    {
        Looping = 1,
        SingleSequence = 2,
        SingleSequenceRepeatFinal = 3
    }

    [MetaMember("Baseclass_DlgNode")]
    public DlgNode DlgNode { get; set; }

    [MetaMember("mPlaybackMode")]
    public PlaybackModeT PlaybackMode { get; set; }

    [MetaMember("mLifetimeMode")]
    public LifetimeModeT LifetimeMode { get; set; }

    [MetaMember("mElements")]
    public DlgChildSetElement Elements { get; set; }

    [MetaMember("mElemUseCriteria")]
    public DlgNodeCriteria ElemUseCriteria { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<Element>))]
    public class Element : IDlgChild
    {
        [MetaMember("Baseclass_DlgChild")]
        public DlgChild DlgChild { get; set; }

        [MetaMember("mRepeat")]
        public RepeatT Repeat { get; set; }

        [MetaMember("mPlayPosition")]
        public PlayPositionT PlayPosition { get; set; }
    }


    [MetaSerializer(typeof(MetaClassSerializer<DlgChildSetElement>))]
    public class DlgChildSetElement : IDlgChildSet
    {
        [MetaMember("Baseclass_DlgChildSet")]
        public DlgChildSet DlgChildSet { get; set; }
    }
}

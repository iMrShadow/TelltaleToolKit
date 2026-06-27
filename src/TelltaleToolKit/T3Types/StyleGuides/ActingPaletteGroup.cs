using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(MetaClassSerializer<ActingPaletteGroup>))]
public class ActingPaletteGroup
{
    [MetaSerializer(typeof(EnumSerializer<IdleTransition>))]
    public enum IdleTransition
    {
        transitionLinear = 1,
        transitionEaseInOut = 2,
        transitionUnused = 3
    }

    [MetaMember("Baseclass_UID::Owner")]
    public Owner BaseClassOwner { get; set; } = new();

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mIdle")]
    public AnimOrChore Idle { get; set; } = new();

    [MetaMember("mWeight")]
    public float Weight { get; set; }

    [MetaMember("mTransitionIn")]
    public AnimOrChore TransitionIn { get; set; } = new();

    [MetaMember("mTransitionOut")]
    public AnimOrChore TransitionOut { get; set; } = new();

    [MetaMember("mTalkingIdle")]
    public AnimOrChore TalkingIdle { get; set; } = new();

    [MetaMember("mMumbleMouth")]
    public AnimOrChore MumbleMouth { get; set; } = new();

    [MetaMember("mTransitions")]
    public List<ActingPaletteTransition> Transitions { get; set; } = [];

    [MetaMember("mIdleTransitionTimeOverride")]
    public float IdleTransitionTimeOverride { get; set; }

    [MetaMember("mhIdleTransitionMap")]
    public Handle<TransitionMap> IdleTransitionMap { get; set; } = new();

    [MetaMember("mIdleTransitionKind")]
    public EnumIdleTransition IdleTransitionKind { get; set; }

    [MetaMember("mRandomAutoMin")]
    public float RandomAutoMin { get; set; }

    [MetaMember("mRandomAutoMax")]
    public float RandomAutoMax { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<ActingPaletteTransition>))]
    public class ActingPaletteTransition
    {
        [MetaMember("mTransition")]
        public string Transition { get; set; } = string.Empty;

        [MetaMember("mTransitionIn")]
        public AnimOrChore TransitionIn { get; set; } = new();

        [MetaMember("mCenterOffset")]
        public float CenterOffset { get; set; }

        [MetaMember("mPreDelay")]
        public float PreDelay { get; set; }

        [MetaMember("mPostDelay")]
        public float PostDelay { get; set; }

        [MetaMember("mFadeTime")]
        public float FadeTime { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<EnumIdleTransition>))]
    public struct EnumIdleTransition
    {
        [MetaMember("mVal")]
        public IdleTransition Val { get; set; }
    }

    // Yes, this is actually how Telltale describes this class.


    [MetaMember("mTransition1")]
    public string Transition1 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn1")]
    public AnimOrChore TransitionIn1 { get; set; } = new();

    [MetaMember("mCenterOffset1")]
    public float CenterOffset1 { get; set; }

    [MetaMember("mPreDelay1")]
    public float PreDelay1 { get; set; }

    [MetaMember("mPostDelay1")]
    public float PostDelay1 { get; set; }

    [MetaMember("mTransition2")]
    public string Transition2 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn2")]
    public AnimOrChore TransitionIn2 { get; set; } = new();

    [MetaMember("mCenterOffset2")]
    public float CenterOffset2 { get; set; }

    [MetaMember("mPreDelay2")]
    public float PreDelay2 { get; set; }

    [MetaMember("mPostDelay2")]
    public float PostDelay2 { get; set; }

    [MetaMember("mTransition3")]
    public string Transition3 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn3")]
    public AnimOrChore TransitionIn3 { get; set; } = new();

    [MetaMember("mCenterOffset3")]
    public float CenterOffset3 { get; set; }

    [MetaMember("mPreDelay3")]
    public float PreDelay3 { get; set; }

    [MetaMember("mPostDelay3")]
    public float PostDelay3 { get; set; }

    [MetaMember("mTransition4")]
    public string Transition4 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn4")]
    public AnimOrChore TransitionIn4 { get; set; } = new();

    [MetaMember("mCenterOffset4")]
    public float CenterOffset4 { get; set; }

    [MetaMember("mPreDelay4")]
    public float PreDelay4 { get; set; }

    [MetaMember("mPostDelay4")]
    public float PostDelay4 { get; set; }

    [MetaMember("mTransition5")]
    public string Transition5 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn5")]
    public AnimOrChore TransitionIn5 { get; set; } = new();

    [MetaMember("mCenterOffset5")]
    public float CenterOffset5 { get; set; }

    [MetaMember("mPreDelay5")]
    public float PreDelay5 { get; set; }

    [MetaMember("mPostDelay5")]
    public float PostDelay5 { get; set; }

    [MetaMember("mTransition6")]
    public string Transition6 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn6")]
    public AnimOrChore TransitionIn6 { get; set; } = new();

    [MetaMember("mCenterOffset6")]
    public float CenterOffset6 { get; set; }

    [MetaMember("mPreDelay6")]
    public float PreDelay6 { get; set; }

    [MetaMember("mPostDelay6")]
    public float PostDelay6 { get; set; }

    [MetaMember("mTransition7")]
    public string Transition7 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn7")]
    public AnimOrChore TransitionIn7 { get; set; } = new();

    [MetaMember("mCenterOffset7")]
    public float CenterOffset7 { get; set; }

    [MetaMember("mPreDelay7")]
    public float PreDelay7 { get; set; }

    [MetaMember("mPostDelay7")]
    public float PostDelay7 { get; set; }

    [MetaMember("mTransition8")]
    public string Transition8 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn8")]
    public AnimOrChore TransitionIn8 { get; set; } = new();

    [MetaMember("mCenterOffset8")]
    public float CenterOffset8 { get; set; }

    [MetaMember("mPreDelay8")]
    public float PreDelay8 { get; set; }

    [MetaMember("mPostDelay8")]
    public float PostDelay8 { get; set; }

    [MetaMember("mTransition9")]
    public string Transition9 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn9")]
    public AnimOrChore TransitionIn9 { get; set; } = new();

    [MetaMember("mCenterOffset9")]
    public float CenterOffset9 { get; set; }

    [MetaMember("mPreDelay9")]
    public float PreDelay9 { get; set; }

    [MetaMember("mPostDelay9")]
    public float PostDelay9 { get; set; }

    [MetaMember("mTransition10")]
    public string Transition10 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn10")]
    public AnimOrChore TransitionIn10 { get; set; } = new();

    [MetaMember("mCenterOffset10")]
    public float CenterOffset10 { get; set; }

    [MetaMember("mPreDelay10")]
    public float PreDelay10 { get; set; }

    [MetaMember("mPostDelay10")]
    public float PostDelay10 { get; set; }

    [MetaMember("mTransition11")]
    public string Transition11 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn11")]
    public AnimOrChore TransitionIn11 { get; set; } = new();

    [MetaMember("mCenterOffset11")]
    public float CenterOffset11 { get; set; }

    [MetaMember("mPreDelay11")]
    public float PreDelay11 { get; set; }

    [MetaMember("mPostDelay11")]
    public float PostDelay11 { get; set; }

    [MetaMember("mTransition12")]
    public string Transition12 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn12")]
    public AnimOrChore TransitionIn12 { get; set; } = new();

    [MetaMember("mCenterOffset12")]
    public float CenterOffset12 { get; set; }

    [MetaMember("mPreDelay12")]
    public float PreDelay12 { get; set; }

    [MetaMember("mPostDelay12")]
    public float PostDelay12 { get; set; }

    [MetaMember("mTransition13")]
    public string Transition13 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn13")]
    public AnimOrChore TransitionIn13 { get; set; } = new();

    [MetaMember("mCenterOffset13")]
    public float CenterOffset13 { get; set; }

    [MetaMember("mPreDelay13")]
    public float PreDelay13 { get; set; }

    [MetaMember("mPostDelay13")]
    public float PostDelay13 { get; set; }

    [MetaMember("mTransition14")]
    public string Transition14 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn14")]
    public AnimOrChore TransitionIn14 { get; set; } = new();

    [MetaMember("mCenterOffset14")]
    public float CenterOffset14 { get; set; }

    [MetaMember("mPreDelay14")]
    public float PreDelay14 { get; set; }

    [MetaMember("mPostDelay14")]
    public float PostDelay14 { get; set; }

    [MetaMember("mTransition15")]
    public string Transition15 { get; set; } = string.Empty;

    [MetaMember("mTransitionIn15")]
    public AnimOrChore TransitionIn15 { get; set; } = new();

    [MetaMember("mCenterOffset15")]
    public float CenterOffset15 { get; set; }

    [MetaMember("mPreDelay15")]
    public float PreDelay15 { get; set; }

    [MetaMember("mPostDelay15")]
    public float PostDelay15 { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlog;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaSerializer(typeof(MetaClassSerializer<DialogItem>))]
public class DialogItem : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mExchanges")]
    public List<int> Exchanges { get; set; } = [];

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mDispTextProxy")]
    public LanguageResProxy DispTextProxy { get; set; } = new();

    [MetaMember("mPlaybackMode")]
    public PlaybackMode PlaybackModeE { get; set; }

    [MetaMember("mPlaybackMode")]
    public EnumPlaybackMode PlaybackModeStruct { get; set; } = new();

    [MetaMember("mEnterScript")]
    public string EnterScript { get; set; } = string.Empty;

    [MetaMember("mExitScript")]
    public string ExitScript { get; set; } = string.Empty;

    [MetaMember("mExitTrigger")]
    public int ExitTrigger { get; set; }

    [MetaMember("mBranchLink")]
    public string BranchLink { get; set; } = string.Empty;

    [MetaMember("mbSpoken")]
    public bool Spoken { get; set; }

    [MetaMember("mbFallbackModeOn")]
    public bool FallbackModeOn { get; set; }

    [MetaMember("mFallbackInput")]
    public int FallbackInput { get; set; }

    [MetaMember("mbResetCurExchangeOnBranchReEntry")]
    public bool ResetCurExchangeOnBranchReEntry { get; set; }

    [MetaMember("mbAllowAutoActing")]
    public bool AllowAutoActing { get; set; }

    [MetaMember("mbCutscene")]
    public bool Cutscene { get; set; }

    [MetaMember("mDispTextProxy")]
    public LanguageResourceProxy  mDispTextProxy { get; set; }

    [MetaMember("mhTexture")]
    public Handle<T3Texture> mhTexture { get; set; }

    [MetaMember("mUserData")]
    public Symbol mUserData { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<EnumPlaybackMode>))]
    public struct EnumPlaybackMode
    {
        [MetaMember("mVal")]
        public PlaybackMode Value { get; set; }
    }

    [MetaSerializer(typeof(EnumSerializer<PlaybackMode>))]
    public enum PlaybackMode
    {
        sequential_looping = 0x0,
        sequential_repeat_final = 0x1,
        sequential_die_off = 0x2,
        shuffle_repeat_all = 0x3,
        shuffle_repeat_final = 0x4,
        shuffle_die_off = 0x5,
        first_then_shuffle_repeat_remaining = 0x6,
    }
}

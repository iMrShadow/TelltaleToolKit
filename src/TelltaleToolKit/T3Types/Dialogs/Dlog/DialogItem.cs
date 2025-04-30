using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DialogItem>))]
public class DialogItem : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mExchanges")]
    public List<int> Exchanges { get; set; } = [];

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mDispTextProxy")]
    public LanguageResourceProxy DispTextProxy { get; set; } = new();

    [MetaMember("mPlaybackMode")]
    public int PlaybackMode { get; set; } // TODO Change to PlayBackMode

    [MetaMember("mPlaybackMode")]
    public EnumPlaybackMode PlaybackModeStruct { get; set; } = new(); // TODO Change to PlayBackMode

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

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumPlaybackMode>))]
    public struct EnumPlaybackMode
    {
        [MetaMember("mVal")]
        public PlaybackModeEnum Value { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<PlaybackModeEnum>))]
    public enum PlaybackModeEnum
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
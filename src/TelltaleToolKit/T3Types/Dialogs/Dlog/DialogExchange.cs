using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DialogExchange>))]
public class DialogExchange : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mLines")]
    public List<int> Lines { get; set; } = [];

    [MetaMember("mBranchLink")]
    public string BranchLink { get; set; } = string.Empty;

    [MetaMember("mEnterScript")]
    public string EnterScript { get; set; } = string.Empty;

    [MetaMember("mExitScript")]
    public string ExitScript { get; set; } = string.Empty;

    [MetaMember("mDispTextProxy")]
    public LanguageResourceProxy DispTextProxy { get; set; } = new();

    [MetaMember("mExitTrigger")]
    public int ExitTrigger { get; set; }

    [MetaMember("mhChore")]
    public Handle<Chore> Chore { get; set; } = new();
}
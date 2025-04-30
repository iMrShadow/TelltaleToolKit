using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DialogBranch>))]
public class DialogBranch : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mItems")]
    public List<int> Items { get; set; } = [];

    [MetaMember("mEnterItemID")]
    public int EnterItemId { get; set; }

    [MetaMember("mExitItemID")]
    public int ExitItemId { get; set; }

    [MetaMember("mEnterItems")]
    public List<int> EnterItems { get; set; } = [];

    [MetaMember("mExitItems")]
    public List<int> ExitItems { get; set; } = [];

    [MetaMember("mEnterScript")]
    public string EnterScript { get; set; } = string.Empty;

    [MetaMember("mExitScript")]
    public string ExitScript { get; set; } = string.Empty;

    [MetaMember("mPersistBGChore")]
    public bool PersistBackgroundChore { get; set; }
}
using System.Collections.Generic;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlog;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaSerializer(typeof(MetaClassSerializer<DialogBranch>))]
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

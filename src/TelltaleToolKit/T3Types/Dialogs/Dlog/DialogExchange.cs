using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Chores;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Dialogs.Dlog;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaSerializer(typeof(MetaClassSerializer<DialogExchange>))]
public class DialogExchange : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mLines")]
    public List<int> Lines { get; set; } = [];

    [MetaMember("mElems")]
    public List<ExchangeElem> mElems { get; set; } = [];

    [MetaMember("mBranchLink")]
    public string BranchLink { get; set; } = string.Empty;

    [MetaMember("mEnterScript")]
    public string EnterScript { get; set; } = string.Empty;

    [MetaMember("mExitScript")]
    public string ExitScript { get; set; } = string.Empty;

    [MetaMember("mDispTextProxy")]
    public LanguageResProxy DispTextProxy { get; set; } = new();

    [MetaMember("mExitTrigger")]
    public int ExitTrigger { get; set; }

    [MetaMember("mhChore")]
    public Handle<Chore> Chore { get; set; } = new();

    [MetaMember("mDispTextProxy")]
    public LanguageResourceProxy  mDispTextProxy { get; set; }


    [MetaMember("mhTexture")]
    public Handle<T3Texture> mhTexture { get; set; }

    [MetaMember("mNotes")]
    public NoteCollection mNotes { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<ExchangeElem>))]
    public class ExchangeElem
    {
        [MetaMember("mID")]
        public int ID { get; set; }

        [MetaMember("mType")]
        public int Type { get; set; }
    }
}

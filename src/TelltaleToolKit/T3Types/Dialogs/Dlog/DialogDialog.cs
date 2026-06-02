using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlog;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DialogDialog>))]
public class DialogDialog : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mBranches")]
    public List<int> Branches { get; set; } = [];
}

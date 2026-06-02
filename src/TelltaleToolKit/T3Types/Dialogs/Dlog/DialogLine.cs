using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlog;
using TelltaleToolKit.T3Types.Languages.Landb;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DialogLine>))]
public class DialogLine : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mLangResProxy")]
    public LanguageResProxy LangResProxy { get; set; } = new();
}

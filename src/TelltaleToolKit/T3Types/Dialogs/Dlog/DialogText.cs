using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlog;
using TelltaleToolKit.T3Types.Languages.Landb;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaSerializer(typeof(MetaClassSerializer<DialogText>))]
public class DialogText : IDialogBase
{
    [MetaMember("Baseclass_DialogBase")]
    public DialogBase DialogBase { get; set; } = new();

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mLangResProxy")]
    public LanguageResProxy LangResProxy { get; set; } = new();

    [MetaMember("mLangResProxy")]
    public LanguageResourceProxy LangResourceProxy { get; set; } = new();

}

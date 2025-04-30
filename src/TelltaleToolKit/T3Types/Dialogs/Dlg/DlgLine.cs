using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common.UID;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<DlgLine>))]
public class DlgLine : IOwner, IDlgObjIdOwner
{
    [MetaMember("Baseclass_UID::Owner")]
    public Owner Owner { get; set; }

    [MetaMember("Baseclass_DlgObjIdOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }

    [MetaMember("mLangResProxy")]
    public LanguageResourceProxy LangResProxy { get; set; }
}
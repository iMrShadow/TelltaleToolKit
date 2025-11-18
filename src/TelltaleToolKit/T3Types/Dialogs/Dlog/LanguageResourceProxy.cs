using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageResourceProxy>))]
public class LanguageResourceProxy
{
    [MetaMember("mID")]
    public uint LangId { get; set; }
}
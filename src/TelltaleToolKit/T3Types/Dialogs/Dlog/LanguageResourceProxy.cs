using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageResourceProxy>))]
public class LanguageResourceProxy
{
    [MetaMember("mID")]
    public uint LangId { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaSerializer(typeof(MetaClassSerializer<LanguageResourceProxy>))]
public class LanguageResourceProxy
{
    [MetaMember("mID")]
    public uint LangId { get; set; }
}

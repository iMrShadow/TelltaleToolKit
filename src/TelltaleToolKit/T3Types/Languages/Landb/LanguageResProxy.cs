using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageResProxy>))]
public class LanguageResProxy
{
    [MetaMember("mID")]
    public int Id { get; set; }
}
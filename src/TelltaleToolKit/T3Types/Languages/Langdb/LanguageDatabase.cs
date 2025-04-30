using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Langdb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageDatabase>))]
public class LanguageDatabase
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mLanguageResources")]
    public Dictionary<int, LanguageResource> LanguageResources { get; set; } = [];
}
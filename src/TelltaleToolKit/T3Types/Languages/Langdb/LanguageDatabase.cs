using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Langdb;

[MetaSerializer(typeof(MetaClassSerializer<LanguageDatabase>))]
public class LanguageDatabase
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mLanguageResources")]
    public Dictionary<int, LanguageResource> LanguageResources { get; set; } = [];
}

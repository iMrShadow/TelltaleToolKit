using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaSerializer(typeof(MetaClassSerializer<LanguageResProxy>))]
public class LanguageResProxy
{
    [MetaMember("mID")]
    public uint Id { get; set; }
}

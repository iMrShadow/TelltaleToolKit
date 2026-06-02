using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<LanguageResLocal>))]
public class LanguageResLocal
{
    [MetaMember("mPrefix")]
    public string Prefix { get; set; }

    [MetaMember("mText")]
    public string Text { get; set; }

    [MetaMember("mLocalInfo")]
    public LocalizeInfo LocalInfo { get; set; }
}

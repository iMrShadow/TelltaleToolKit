using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<StyleGuideMapper>))]
public class StyleGuideMapper
{
    [MetaMember("mStyleGuideMap")]
    public Dictionary<Handle<StyleGuide>, Handle<StyleGuide>> StyleGuideMap { get; set; } = new();
}
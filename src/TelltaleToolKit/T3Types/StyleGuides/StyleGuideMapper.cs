using System.Collections.Generic;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(MetaClassSerializer<StyleGuideMapper>))]
public class StyleGuideMapper
{
    [MetaMember("mStyleGuideMap")]
    public Dictionary<Handle<StyleGuide>, Handle<StyleGuide>> StyleGuideMap { get; set; } = new();
}

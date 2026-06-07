using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<StyleIdleTransitionsRes>))]
public class StyleIdleTransitionsRes
{
    [MetaMember("mOwningAgent")]
    public string OwningAgent { get; set; } = string.Empty;

    [MetaMember("mGuideName")]
    public string GuideName { get; set; } = string.Empty;
}

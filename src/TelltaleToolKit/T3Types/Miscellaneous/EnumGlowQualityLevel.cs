using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumGlowQualityLevel>))]
public struct EnumGlowQualityLevel
{
    [MetaMember("mVal")]
    public GlowQualityLevel Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<GlowQualityLevel>))]
    public enum GlowQualityLevel
    {
        Old = 0x0,
        Low = 0x1,
        Medium = 0x2,
        High = 0x3
    }
}

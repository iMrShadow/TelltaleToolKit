using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOQualityLevel>))]
public struct EnumHBAOQualityLevel
{
    [MetaMember("mVal")]
    public HBAOQualityLevel Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HBAOQualityLevel>))]
    public enum HBAOQualityLevel
    {
        Lowest = 0x0,
        Low = 0x1,
        Medium = 0x2,
        High = 0x3,
        Highest = 0x4
    }
}

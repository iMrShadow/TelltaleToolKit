using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumDOFQualityLevel>))]
public struct EnumDOFQualityLevel
{
    [MetaMember("mVal")]
    public DOFQualityLevel Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<DOFQualityLevel>))]
    public enum DOFQualityLevel
    {
        Disabled = 0x0,
        Low = 0x1,
        Medium = 0x2,
        High = 0x3
    }
}

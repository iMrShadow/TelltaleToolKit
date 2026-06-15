using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumDOFQualityLevel>))]
public struct EnumDOFQualityLevel
{
    [MetaMember("mVal")]
    public DOFQualityLevel Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<DOFQualityLevel>))]
public enum DOFQualityLevel
{
    // TODO:
}

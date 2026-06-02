using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumBokehQualityLevel>))]
public struct EnumBokehQualityLevel
{
    [MetaMember("mVal")]
    public BokehQualityLevel Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<BokehQualityLevel>))]
public enum BokehQualityLevel
{
    // TODO:
}

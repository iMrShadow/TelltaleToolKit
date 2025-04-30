using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumBokehQualityLevel>))]
public struct EnumBokehQualityLevel
{
    [MetaMember("mVal")]
    public BokehQualityLevel Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<BokehQualityLevel>))]
public enum BokehQualityLevel
{
    // TODO:
}
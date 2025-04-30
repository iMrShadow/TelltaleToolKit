using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAOQualityLevel>))]
public struct EnumHBAOQualityLevel
{
    [MetaMember("mVal")]
    public HBAOQualityLevel Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAOQualityLevel>))]
public enum HBAOQualityLevel
{
    // TODO:
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumGlowQualityLevel>))]
public struct EnumGlowQualityLevel
{
    [MetaMember("mVal")]
    public GlowQualityLevel Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<GlowQualityLevel>))]
public enum GlowQualityLevel
{
    // TODO:
}
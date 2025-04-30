using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAOBlurQuality>))]
public struct EnumHBAOBlurQuality
{
    [MetaMember("mVal")]
    public HBAOBlurQuality Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAOBlurQuality>))]
public enum HBAOBlurQuality
{
    // TODO:
}
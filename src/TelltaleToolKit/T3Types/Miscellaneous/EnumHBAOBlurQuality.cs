using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOBlurQuality>))]
public struct EnumHBAOBlurQuality
{
    [MetaMember("mVal")]
    public HBAOBlurQuality Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<HBAOBlurQuality>))]
public enum HBAOBlurQuality
{
    // TODO:
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOResolution>))]
public struct EnumHBAOResolution
{
    [MetaMember("mVal")]
    public HBAOResolution Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HBAOResolution>))]
    public enum HBAOResolution
    {
        Full = 0x0,
        Half = 0x1,
        Quarter = 0x2
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumBokehOcclusionType>))]
public struct EnumBokehOcclusionType
{
    [MetaMember("mVal")]
    public BokehOcclusionType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<BokehOcclusionType>))]
    public enum BokehOcclusionType
    {
        Disabled = 0x0,
        ZTestAndScaleOccluded = 0x1,
        ScaleOccluded = 0x2,
        ZTest = 0x3
    }
}

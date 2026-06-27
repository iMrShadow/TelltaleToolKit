using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderTAAJitterType>))]
public struct EnumRenderTAAJitterType
{
    [MetaMember("mVal")]
    public RenderTAAJitterType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RenderTAAJitterType>))]
    public enum RenderTAAJitterType
    {
        None = 0x1,
        Uniform2x = 0x2,
        Hammersley4x = 0x3,
        Hammersley8x = 0x4
    }
}

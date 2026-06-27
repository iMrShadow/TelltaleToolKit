using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderAntialiasType>))]
public struct EnumRenderAntialiasType
{
    [MetaMember("mVal")]
    public RenderAntialiasType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RenderAntialiasType>))]
    public enum RenderAntialiasType
    {
        None = 0x0,
        FXAA = 0x1,
        SMAA = 0x2,
        MSAA_2x = 0x3,
        MSAA_4x = 0x4,
        MSAA_8x = 0x5,
        TAA_MSAA_2x = 0x6,
        TAA_MSAA_4x = 0x7,
        TAA_MSAA_8x = 0x8,
        TAA = 0x9
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderMaskWrite>))]
public struct EnumRenderMaskWrite
{
    [MetaMember("mVal")]
    public RenderMaskWrite Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RenderMaskWrite>))]
    public enum RenderMaskWrite
    {
        None = 0x1,
        Set = 0x2,
        Clear = 0x3
    }
}

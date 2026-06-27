using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderMaskTest>))]
public struct EnumRenderMaskTest
{
    [MetaMember("mVal")]
    public RenderMaskTest Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RenderMaskTest>))]
    public enum RenderMaskTest
    {
        None = 0x1,
        Set = 0x2,
        Clear = 0x3
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderTextureResolution>))]
public struct EnumRenderTextureResolution
{
    [MetaMember("mVal")]
    public RenderTextureResolution Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RenderTextureResolution>))]
    public enum RenderTextureResolution
    {
        Small = 0x1,
        Medium = 0x2,
        Default = 0x3,
        Ultra = 0x4
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumRenderTextureResolution>))]
public struct EnumRenderTextureResolution
{
    [MetaMember("mVal")]
    public RenderTextureResolution Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<RenderTextureResolution>))]
public enum RenderTextureResolution
{
    // TODO:
}

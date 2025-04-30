using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumRenderLightmapUVGenerationType>))]
public struct EnumRenderLightmapUVGenerationType
{
    [MetaMember("mVal")]
    public RenderLightmapUVGenerationType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<RenderLightmapUVGenerationType>))]
public enum RenderLightmapUVGenerationType
{
    // TODO:
}

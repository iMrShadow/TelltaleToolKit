using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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

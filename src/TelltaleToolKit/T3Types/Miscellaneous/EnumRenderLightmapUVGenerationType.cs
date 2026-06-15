using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderLightmapUVGenerationType>))]
public struct EnumRenderLightmapUVGenerationType
{
    [MetaMember("mVal")]
    public RenderLightmapUVGenerationType Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<RenderLightmapUVGenerationType>))]
public enum RenderLightmapUVGenerationType
{
    // TODO:
}

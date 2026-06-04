using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderLightmapUVGenerationType>))]
public struct EnumRenderLightmapUVGenerationType
{
    [MetaMember("mVal")]
    public RenderLightmapUVGenerationType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RenderLightmapUVGenerationType>))]
    public enum RenderLightmapUVGenerationType
    {
        Default = 0,
        Auto = 1,
        UV0 = 2,
        UV1 = 3,
        UV2 = 4,
        UV3 = 5
    }
}

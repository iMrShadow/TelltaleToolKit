using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRenderAntialiasType>))]
public struct EnumRenderAntialiasType
{
    [MetaMember("mVal")]
    public RenderAntialiasType Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<RenderAntialiasType>))]
public enum RenderAntialiasType
{
    // TODO:
}

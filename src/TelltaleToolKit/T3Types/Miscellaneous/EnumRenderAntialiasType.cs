using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumRenderAntialiasType>))]
public struct EnumRenderAntialiasType
{
    [MetaMember("mVal")]
    public RenderAntialiasType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<RenderAntialiasType>))]
public enum RenderAntialiasType
{
    // TODO:
}

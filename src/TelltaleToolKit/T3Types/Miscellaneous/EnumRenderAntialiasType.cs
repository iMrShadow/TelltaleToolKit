using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumRenderTAAJitterType>))]
public struct EnumRenderTAAJitterType
{
    [MetaMember("mVal")]
    public RenderTAAJitterType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<RenderTAAJitterType>))]
public enum RenderTAAJitterType
{
    // TODO:
}

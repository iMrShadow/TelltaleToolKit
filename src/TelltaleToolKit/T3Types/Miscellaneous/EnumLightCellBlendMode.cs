using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumLightCellBlendMode>))]
public struct EnumLightCellBlendMode
{
    [MetaMember("mVal")]
    public LightCellBlendMode Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<LightCellBlendMode>))]
public enum LightCellBlendMode
{
    // TODO:
}

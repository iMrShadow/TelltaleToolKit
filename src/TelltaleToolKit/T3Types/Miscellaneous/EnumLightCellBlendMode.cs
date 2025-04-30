using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
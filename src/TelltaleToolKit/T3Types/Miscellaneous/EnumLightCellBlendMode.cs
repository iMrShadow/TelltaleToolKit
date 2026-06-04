using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumLightCellBlendMode>))]
public struct EnumLightCellBlendMode
{
    [MetaMember("mVal")]
    public LightCellBlendMode Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<LightCellBlendMode>))]
    public enum LightCellBlendMode
    {
        Normal = 0x0,
        Dodge = 0x1,
        Multiply = 0x2,
        Screen = 0x3,
        Overlay = 0x4
    }
}

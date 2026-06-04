using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumTextOrientationType>))]
public struct EnumTextOrientationType
{
    [MetaMember("mVal")]
    public TextOrientationType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<TextOrientationType>))]
    public enum TextOrientationType
    {
        Screen = 0x0,
        WorldZ = 0x1,
        WorldXYZ = 0x2
    }
}

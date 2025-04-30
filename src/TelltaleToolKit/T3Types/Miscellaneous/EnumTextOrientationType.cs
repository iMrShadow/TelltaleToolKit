using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumTextOrientationType>))]
public struct EnumTextOrientationType
{
    [MetaMember("mVal")]
    public TextOrientationType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<TextOrientationType>))]
public enum TextOrientationType
{
    //    eTextOrientation_
    Screen = 0x0,
    WorldZ = 0x1,
    WorldXYZ = 0x2,
}
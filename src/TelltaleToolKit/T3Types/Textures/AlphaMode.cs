using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures;

[MetaClassSerializerGlobal(typeof(EnumSerializer<AlphaMode>))]
public enum AlphaMode
{
    Unknown = -1,
    NoAlpha = 0,
    AlphaTest = 1,
    AlphaBlend = 2,
}
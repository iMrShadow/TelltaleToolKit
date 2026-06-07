using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures;

[MetaSerializer(typeof(EnumSerializer<ColorMode>))]
public enum ColorMode
{
    // eTxColor
    Unknown = -1,
    Full = 0,
    Black = 1,
    Grayscale = 2,
    GrayscaleAlpha = 3
}

using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures;

[MetaClassSerializerGlobal(typeof(EnumSerializer<ColorMode>))]

public enum ColorMode
{
    eTxColorUnknown = -1,
    eTxColorFull = 0,
    eTxColorBlack = 1,
    eTxColorGrayscale = 2,
    eTxColorGrayscaleAlpha = 3,
}

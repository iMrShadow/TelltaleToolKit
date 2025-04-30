namespace TelltaleToolKit.T3Types.Textures.T3Types;

[Flags]
public enum T3SamplerStateValue
{
    WrapU = 0xF, // 15
    WrapV = 0xF0, // 240
    Filtered = 0x100, // 256
    BorderColor = 0x1E00, // 7680
    GammaCorrect = 0x2000, // 8192
    MipBias = 0x3FC000, // 4177920
    Count = 0x6,
}
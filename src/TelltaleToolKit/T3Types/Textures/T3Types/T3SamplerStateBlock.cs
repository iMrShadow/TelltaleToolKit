using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

/// <summary>
/// Compact sampler state packed into a single 32-bit unsigned integer.
/// Bit layout (inclusive):
/// bits 0..3   : WrapU (4 bits)
/// bits 4..7   : WrapV (4 bits)
/// bit  8      : Filtered (1 bit)
/// bits 9..12  : Border Color (4 bits)
/// bit  13     : Gamma Correct (1 bit)
/// bits 14..21 : Mip Bias (8 bits)
/// Remaining bits unused/reserved.
/// 
/// This class provides typed accessors and several utility functions.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3SamplerStateBlock>))]
public class T3SamplerStateBlock
{
    [MetaMember("mData")]
    public uint Data { get; set; }

    private struct Entry
    {
        public int Shift;
        public uint Mask;

        public Entry(int shift, int width)
        {
            Shift = shift;
            Mask = (1u << width) - 1u << shift;
        }
    }

    public TextureWrapMode WrapU
    {
        get => (TextureWrapMode)InternalGet(T3SamplerStateValue.WrapU);
        set => InternalSet(T3SamplerStateValue.WrapU, (uint)value);
    }

    public TextureWrapMode WrapV
    {
        get => (TextureWrapMode)InternalGet(T3SamplerStateValue.WrapV);
        set => InternalSet(T3SamplerStateValue.WrapV, (uint)value);
    }

    public bool Filtered
    {
        get => InternalGet(T3SamplerStateValue.Filtered) != 0;
        set => InternalSet(T3SamplerStateValue.Filtered, value ? 1u : 0u);
    }

    public TextureBorderColor BorderColor
    {
        get => (TextureBorderColor)InternalGet(T3SamplerStateValue.BorderColor);
        set => InternalSet(T3SamplerStateValue.BorderColor, (uint)value);
    }

    public bool GammaCorrect
    {
        get => InternalGet(T3SamplerStateValue.GammaCorrect) != 0;
        set => InternalSet(T3SamplerStateValue.GammaCorrect, value ? 1u : 0u);
    }

    public byte MipBias
    {
        get => (byte)InternalGet(T3SamplerStateValue.MipBias);
        set => InternalSet(T3SamplerStateValue.MipBias, value);
    }

    // Raw data access
    public uint RawData
    {
        get => Data;
        set => Data = value;
    }

    private uint InternalGet(T3SamplerStateValue state)
    {
        Entry e = Entries[(int)state];
        return (Data & e.Mask) >> e.Shift;
    }

    private void InternalSet(T3SamplerStateValue state, uint value)
    {
        Entry e = Entries[(int)state];
        // clamp value to width
        uint widthMask = e.Mask >> e.Shift;
        value &= widthMask;
        Data = (Data & ~e.Mask) | ((value << e.Shift) & e.Mask);
    }

    private static readonly Entry[] Entries =
    [
        new Entry(0, 4), // WrapU

        new Entry(4, 4), // WrapV

        new Entry(8, 1), // Filter

        new Entry(9, 4), // Border

        new Entry(13, 1), // Gamma

        new Entry(14, 8) // MipBias
    ];


    public enum T3SamplerStateValue
    {
        // eSamplerState_..._Value
        WrapU = 0x0, // TextureWrapMode.
        WrapV = 0x1, // TextureWrapMode.
        Filtered = 0x2, // bool
        BorderColor = 0x3, // TextureBorderColor 
        GammaCorrect = 0x4, // bool
        MipBias = 0x5, // char
    }

    public enum TextureWrapMode
    {
        // Texture_Wrap
        Clamp = 0,
        Wrap = 1,
        Border = 2,
    }

    public enum TextureBorderColor
    {
        //  TEXTURE_BORDER_COLOR_
        Black = 0, // Color(0,0,0,0)
        White = 1, // Color(1,1,1,1)
    }
}
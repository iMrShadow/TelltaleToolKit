using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Fonts;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Font>))]
public class Font
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mbFiltered")]
    public bool Filtered { get; set; }

    [MetaMember("mHeight")]
    public float Height { get; set; }

    [MetaMember("mGlyphInfo")]
    public List<GlyphInfo> GlyphInformation { get; set; } = [];

    [MetaMember("mTexturePages")]
    public List<T3Texture> T3TexturePages { get; set; } = [];

    [MetaMember("mbUnicode")]
    public bool Unicode { get; set; }

    [MetaMember("mBase")]
    public float Base { get; set; }

    [MetaMember("mGlyphInfo")]
    public Dictionary<uint, GlyphInfo> GlyphInformationDic { get; set; } = [];

    [MetaMember("mIsDistanceField")]
    public bool IsDistanceField { get; set; }

    [MetaMember("mIsRuntime")]
    public bool IsRuntime { get; set; }

    [MetaMember("mIsFiltered")]
    public bool IsFiltered { get; set; }

    [MetaMember("mTtfData")]
    public BinaryBuffer TtfData { get; set; }

    [MetaMember("mBasePointSize")]
    public float BasePointSize { get; set; }

    [MetaMember("mPreferredPointSizes")]
    public List<uint> PreferredPointSizes { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<GlyphInfo>))]
    public class GlyphInfo
    {
        [MetaMember("mTexturePage")]
        public int TexturePage { get; set; }

        [MetaMember("mChannel")]
        public int Channel { get; set; }

        [MetaMember("mGlyph")]
        public Rect<float> Glyph { get; set; } = new();

        [MetaMember("mWidth")]
        public float Width { get; set; }

        [MetaMember("mHeight")]
        public float Height { get; set; }

        [MetaMember("mXOffset")]
        public float XOffset { get; set; }

        [MetaMember("mYOffset")]
        public float YOffset { get; set; }

        [MetaMember("mXAdvance")]
        public float XAdvance { get; set; }

        public override string ToString() => $"{TexturePage} {Glyph}";
    }

    public class FontCreateParam
    {
    }
}
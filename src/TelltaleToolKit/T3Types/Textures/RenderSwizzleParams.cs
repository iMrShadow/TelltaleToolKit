using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<RenderSwizzleParams>))]
public struct RenderSwizzleParams
{
    [MetaMember("mSwizzle[0]")]
    public byte Channel0 { get; set; }

    [MetaMember("mSwizzle[1]")]
    public byte Channel1 { get; set; }

    [MetaMember("mSwizzle[2]")]
    public byte Channel2 { get; set; }

    [MetaMember("mSwizzle[3]")]
    public byte Channel3 { get; set; }
}
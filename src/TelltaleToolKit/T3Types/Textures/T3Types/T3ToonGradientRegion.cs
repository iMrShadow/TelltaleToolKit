using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3ToonGradientRegion>))]
public struct T3ToonGradientRegion
{
    [MetaMember("mColor")]
    public Color Color { get; set; }

    [MetaMember("mSize")]
    public float Size { get; set; }
}
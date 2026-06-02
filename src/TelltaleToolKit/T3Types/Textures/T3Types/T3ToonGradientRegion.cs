using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3ToonGradientRegion>))]
public struct T3ToonGradientRegion
{
    [MetaMember("mColor")]
    public Color Color { get; set; }

    [MetaMember("mSize")]
    public float Size { get; set; }
}

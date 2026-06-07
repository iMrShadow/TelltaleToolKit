using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaSerializer(typeof(MetaClassSerializer<Rect>))]
public struct Rect
{
    [MetaMember("left")]
    public int Left { get; set; }

    [MetaMember("right")]
    public int Right { get; set; }

    [MetaMember("top")]
    public int Top { get; set; }

    [MetaMember("bottom")]
    public int Bottom { get; set; }
}

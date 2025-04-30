using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Rect>))]
public struct Rect
{
    [MetaMember("left")] public int Left { get; set; }
    [MetaMember("right")] public int Right { get; set; }
    [MetaMember("top")] public int Top { get; set; }
    [MetaMember("bottom")] public int Bottom { get; set; }
}
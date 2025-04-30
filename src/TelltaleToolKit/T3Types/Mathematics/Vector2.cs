using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Vector2>))]
public struct Vector2
{
    [MetaMember("x")]
    public float X { get; set; }

    [MetaMember("y")]
    public float Y { get; set; }
    
    private Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public static readonly Vector2 Zero = new();

    public static readonly Vector2 One = new(1.0f, 1.0f);
//    public readonly override string ToString() => $"{this}";
    public readonly override string ToString() => $"({X}, {Y})";

}
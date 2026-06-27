using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaSerializer(typeof(RectSerializer<>), typeof(Rect<>))]
public class Rect<T>
{
    [MetaMember("left")]
    public T Left { get; set; } = default!;

    [MetaMember("right")]
    public T Right { get; set; } = default!;

    [MetaMember("top")]
    public T Top { get; set; } = default!;

    [MetaMember("bottom")]
    public T Bottom { get; set; } = default!;

    public override string ToString() => $"{Left}";
}

public class RectSerializer<T> : MetaSerializer<Rect<T>>
{
    private static readonly MetaClassSerializer<Rect<T>> s_metaClassSerializer = new();

    public override void PreSerialize(ref Rect<T>? obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Read)
        {
            if (obj is null)
            {
                obj = new Rect<T>();
            }
        }
    }


    public override void Serialize(ref Rect<T> obj, MetaStream stream, MetaClassType? type = null) =>
        s_metaClassSerializer.Serialize(ref obj, stream);
}

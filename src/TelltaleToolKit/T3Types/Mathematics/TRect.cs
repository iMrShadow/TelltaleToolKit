using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(RectClassSerializer<>), typeof(Rect<>))]
public class Rect<T>
{
    [MetaMember("left")] public T Left { get; set; } = default!;
    [MetaMember("right")] public T Right { get; set; } = default!;
    [MetaMember("top")] public T Top { get; set; } = default!;
    [MetaMember("bottom")] public T Bottom { get; set; } = default!;

    public override string ToString() => $"{Left}";
}

public class RectClassSerializer<T> : MetaClassSerializer<Rect<T>>
{
    private static readonly DefaultClassSerializer<Rect<T>> DefaultSerializer = new();

    /// <inheritdoc/>
    public override void PreSerialize(ref Rect<T> obj, MetaStream stream, MetaClassType? type)
    {
        if (stream.Mode is MetaStreamMode.Read)
        {
            if (obj is null)
                obj = new Rect<T>();
        }
    }


    public override void Serialize(ref Rect<T> obj, MetaStream stream)
    {
        DefaultSerializer.Serialize(ref obj, stream);
    }
}

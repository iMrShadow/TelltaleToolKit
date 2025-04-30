using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

public class Range<T>
{
    [MetaMember("min")]
    public T Min { get; set; } = default!;

    [MetaMember("max")]
    public T Max { get; set; } = default!;
}

[MetaClassSerializerGlobal(typeof(RangeClassSerializer<>), typeof(Range<>))]
public class RangeClassSerializer<T> : MetaClassSerializer<Range<T>>
{
    private static readonly DefaultClassSerializer<Range<T>> DefaultSerializer = new();

    /// <inheritdoc/>
    public override void PreSerialize(ref Range<T> obj, MetaStream stream, MetaClassType? type)
    {
        if (stream is MetaStreamReader streamReader)
        {
            if (obj is null)
                obj = new Range<T>();
        }
    }

    public override void Serialize(ref Range<T> obj, MetaStream stream)
    {
        DefaultSerializer.PreSerialize(ref obj, stream);
        DefaultSerializer.Serialize(ref obj, stream);
    }
}
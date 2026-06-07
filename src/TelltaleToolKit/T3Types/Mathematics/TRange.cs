using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

public class Range<T>
{
    [MetaMember("min")]
    public T Min { get; set; } = default!;

    [MetaMember("max")]
    public T Max { get; set; } = default!;
}

[MetaSerializer(typeof(RangeSerializer<>), typeof(Range<>))]
public class RangeSerializer<T> : MetaSerializer<Range<T>>
{
    private static readonly MetaClassSerializer<Range<T>> s_metaClassSerializer = new();

    // TODO: Null serializer
    public override void PreSerialize(ref Range<T>? obj, MetaStream stream, MetaClassType? type = null) =>
        obj ??= new Range<T>();

    public override void Serialize(ref Range<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        s_metaClassSerializer.PreSerialize(ref obj!, stream);
        s_metaClassSerializer.Serialize(ref obj, stream);
    }
}

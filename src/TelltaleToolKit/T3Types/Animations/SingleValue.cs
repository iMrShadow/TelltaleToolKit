using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(SingleValue<>.Serializer), typeof(SingleValue<>))]
public class SingleValue<T> : IAnimationValueInterface
{
    public T Value { get; set; }

    public class Serializer : MetaSerializer<SingleValue<T>>
    {
        public override void PreSerialize(ref SingleValue<T>? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new SingleValue<T>();
        }

        public override void Serialize(ref SingleValue<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                T objValue = obj.Value;
                stream.Serialize(ref objValue);
            }
            else
            {
                T value = default;
                stream.Serialize(ref value!);
                obj.Value = value;
            }
        }
    }

    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();
}

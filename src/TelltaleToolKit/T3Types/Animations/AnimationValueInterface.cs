using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(AnimationValueInterface<>.Serializer), typeof(AnimationValueInterface<>))]
public class AnimationValueInterface<T> : IAnimationValueInterface
{
    [MetaMember("Baseclass_AnimationValueInterfaceBase")]
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public class Serializer : MetaSerializer<AnimationValueInterface<T>>
    {
        private static readonly MetaClassSerializer<AnimationValueInterface<T>> s_metaClassSerializer = new();

        public override void PreSerialize(ref AnimationValueInterface<T>? obj, MetaStream stream,
            MetaClassType? type = null)
        {
            obj ??= new AnimationValueInterface<T>();
        }

        public override void Serialize(ref AnimationValueInterface<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);
        }
    }
}

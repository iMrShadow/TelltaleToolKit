using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

// TODO: Add interface
[MetaSerializer(typeof(AnimatedValueInterface<>.Serializer), typeof(AnimatedValueInterface<>))]
public class AnimatedValueInterface<T> : IAnimatedValueInterface
{
    [MetaMember("Baseclass_AnimationValueInterfaceBase")]
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public class Serializer : MetaSerializer<AnimatedValueInterface<T>>
    {
        private static readonly MetaClassSerializer<AnimatedValueInterface<T>> s_metaClassSerializer = new();

        public override void Serialize(ref AnimatedValueInterface<T> obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

// TODO: Add interface
[MetaClassSerializerGlobal(typeof(AnimatedValueInterface<>.Serializer), typeof(AnimatedValueInterface<>))]
public class AnimatedValueInterface<T> : IAnimatedValueInterface
{
    [MetaMember("Baseclass_AnimationValueInterfaceBase")]
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    public class Serializer : MetaClassSerializer<AnimatedValueInterface<T>>
    {
        private static readonly DefaultClassSerializer<AnimatedValueInterface<T>> DefaultSerializer = new();

        public override void Serialize(ref AnimatedValueInterface<T> obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);
        }
    }
}
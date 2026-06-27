using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(KeyframedValue<>.Serializer), typeof(KeyframedValue<>))]
public class KeyframedValue<T> : IAnimationValueInterface where T : notnull
{
    [MetaMember("Baseclass_AnimatedValueInterface<T>")]
    public AnimationValueInterface<T> BaseClassAnimationValueInterface { get; set; } = new();

    public AnimationValueInterfaceBase AnimationValueInterfaceBase
    {
        get => BaseClassAnimationValueInterface.AnimationValueInterfaceBase;
        set => BaseClassAnimationValueInterface.AnimationValueInterfaceBase = value;
    }

    [MetaMember("mMinVal")]
    public T MinVal { get; set; }

    [MetaMember("mMaxVal")]
    public T MaxVal { get; set; }

    [MetaMember("mSamples")]
    public List<Sample> Samples { get; set; } = [];

    [MetaSerializer(typeof(KeyframedValue<>.Sample.Serializer), typeof(KeyframedValue<>.Sample))]
    public class Sample
    {
        [MetaMember("mTime")]
        public float Time { get; set; }

        [MetaMember("mbInterpolateToNextKey")]
        public bool InterpolateToNextKey { get; set; }

        [MetaMember("mValue")]
        public T Value { get; set; } = default!;

        [MetaMember("mTangentMode")]
        public TangentMode TangentMode { get; set; }

        public class Serializer : MetaSerializer<Sample>
        {
            private static readonly MetaClassSerializer<Sample> s_metaClassSerializer = new();

            public override void Serialize(ref Sample obj, MetaStream stream, MetaClassType? type = null)
            {
                s_metaClassSerializer.PreSerialize(ref obj, stream);
                s_metaClassSerializer.Serialize(ref obj, stream);
            }
        }
    }

    public class Serializer : MetaSerializer<KeyframedValue<T>>
    {
        private static readonly MetaClassSerializer<KeyframedValue<T>> s_metaClassSerializer = new();

        public override void PreSerialize(ref KeyframedValue<T>? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new KeyframedValue<T>();
        }

        public override void Serialize(ref KeyframedValue<T> obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
            }
        }
    }
}

[MetaSerializer(typeof(EnumSerializer<TangentMode>))]
public enum TangentMode
{
    // eTangent
    Unknown = 0,
    Stepped = 1,
    Knot = 2,
    Smooth = 3,
    Flat = 4
}

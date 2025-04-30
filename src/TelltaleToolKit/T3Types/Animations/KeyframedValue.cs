using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(KeyframedValue<>.Serializer), typeof(KeyframedValue<>))]
public class KeyframedValue<T> : IAnimatedValueInterface where T : notnull
{
    [MetaMember("Baseclass_AnimatedValueInterface<T>")]
    public AnimatedValueInterface<T> BaseClassAnimatedValueInterface { get; set; } = new();

    public AnimationValueInterfaceBase AnimationValueInterfaceBase
    {
        get => BaseClassAnimatedValueInterface.AnimationValueInterfaceBase;
        set => BaseClassAnimatedValueInterface.AnimationValueInterfaceBase = value;
    }

    [MetaMember("mMinVal")]
    public T MinVal { get; set; }

    [MetaMember("mMaxVal")]
    public T MaxVal { get; set; }

    [MetaMember("mSamples")]
    public List<Sample> Samples { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(KeyframedValue<>.Sample.Serializer), typeof(KeyframedValue<>.Sample))]
    public class Sample
    {
        [MetaMember("mTime")]
        public float Time { get; set; }

        [MetaMember("mbInterpolateToNextKey")]
        public bool InterpolateToNextKey { get; set; }

        [MetaMember("mValue")]
        public T Value { get; set; }

        [MetaMember("mTangentMode")]
        public TangentMode TangentMode { get; set; }

        public class Serializer : MetaClassSerializer<Sample>
        {
            private static readonly DefaultClassSerializer<Sample> DefaultSerializer = new();

            public override void Serialize(ref Sample obj, MetaStream stream)
            {
                DefaultSerializer.PreSerialize(ref obj, stream);
                DefaultSerializer.Serialize(ref obj, stream);
            }
        }
    }

    public class Serializer : MetaClassSerializer<KeyframedValue<T>>
    {
        private static readonly DefaultClassSerializer<KeyframedValue<T>> DefaultSerializer = new();

        public override void Serialize(ref KeyframedValue<T> obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
            }
        }
    }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<TangentMode>))]
public enum TangentMode
{
    // eTangent
    Unknown = 0,
    Stepped = 1,
    Knot = 2,
    Smooth = 3,
    Flat = 4
}
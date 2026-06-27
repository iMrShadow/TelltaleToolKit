using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Animations;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaSerializer(typeof(MetaClassSerializer<SkeletonPoseValue>))]
public class SkeletonPoseValue : IAnimationValueInterface
{
    [MetaMember("mBones")]
    public List<BoneEntry> Bones { get; set; } = [];

    [MetaMember("mSamples")]
    public List<Sample> Samples { get; set; } = [];

    [MetaMember("mFastBoneCount")]
    public int FastBoneCount { get; set; }

    [MetaMember("Baseclass_AnimationValueInterfaceBase")]
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();

    [MetaSerializer(typeof(MetaClassSerializer<Sample>))]
    public class Sample
    {
        [MetaMember("mTime")]
        public float Time { get; set; }

        [MetaMember("mRecipTimeToNextSample")]
        public float RecipTimeToNextSample { get; set; }

        [MetaMember("mValues")]
        public List<Transform> Values { get; set; } = [];

        [MetaMember("mFastValues")]
        public List<Transform> FastValues { get; set; } = [];

        [MetaMember("mTangents")]
        public List<int> Tangents { get; set; } = [];

        [MetaMember("mValues")]
        public List<ValueEntry> ValuesS { get; set; } = [];
    }

    [MetaSerializer(typeof(MetaClassSerializer<BoneEntry>))]
    public class BoneEntry
    {
        [MetaMember("mName")]
        public Symbol Name { get; set; } = Symbol.Empty;

        [MetaMember("mFlags")]
        public uint Flags { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<ValueEntry>))]
    public class ValueEntry
    {
        [MetaMember("mValue")]
        public Transform Value { get; set; } = new();

        [MetaMember("mTangentMode")]
        public int TangentMode { get; set; }
    }
}

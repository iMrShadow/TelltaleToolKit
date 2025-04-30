using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Skeletons;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Skeleton>))]
public class Skeleton
{
    [MetaMember("mEntries")]
    public List<Entry> Entries { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Entry>))]
    public class Entry
    {
        // This is a string and a symbol at the same time
        [MetaMember("mJointName")]
        public Symbol JointName { get; set; }

        // This is a string and a symbol at the same time
        [MetaMember("mParentName")]
        public Symbol ParentName { get; set; }

        [MetaMember("mParentIndex")]
        public int ParentIndex { get; set; }

        // This is a string and a symbol at the same time
        [MetaMember("mMirrorBoneName")]
        public Symbol MirrorBoneName { get; set; }

        [MetaMember("mMirrorBoneIndex")]
        public int MirrorBoneIndex { get; set; }

        [MetaMember("mBoneLength")]
        public float BoneLength { get; set; }

        [MetaMember("mBoneDir")]
        public Vector3 BoneDir { get; set; }

        [MetaMember("mBoneRotationAdjus")]
        public Quaternion BoneRotationAdjustment { get; set; }

        [MetaMember("mLocalPos")]
        public Vector3 LocalPosition { get; set; } = new();

        [MetaMember("mLocalQuat")]
        public Quaternion LocalQuat { get; set; } = new();

        [MetaMember("mRestXform")]
        public Transform RestXform { get; set; } = new();

        [MetaMember("mGlobalTranslationScale")]
        public Vector3 GlobalTranslationScale { get; set; } = new();

        [MetaMember("mLocalTranslationScale")]
        public Vector3 LocalTranslationScale { get; set; } = new();

        [MetaMember("mAnimTranslationScale")]
        public Vector3 AnimTranslationScale { get; set; } = new();

        // This is a string and a symbol at the same time
        [MetaMember("mResourceGroupMembership")]
        public Dictionary<Symbol, float> ResourceGroupMembership { get; set; } = [];

        [MetaMember("mConstraints")]
        public BoneConstraints Constraints { get; set; } = new();

        [MetaMember("mFlags")]
        public Flags Flags { get; set; }
    }
}
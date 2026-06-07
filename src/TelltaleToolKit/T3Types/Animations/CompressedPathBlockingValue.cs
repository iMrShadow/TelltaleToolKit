using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Skeletons;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<CompressedPathBlockingValue>))]
public class CompressedPathBlockingValue : IAnimationValueInterface
{
    [MetaMember("Baseclass_KeyframedValue<Transform>")]
    public KeyframedValue<Transform> BaseclassCompressedPathBlockingValue { get; set; } = new();

    [MetaMember("mCompressedPathInfoKeys")]
    public KeyframedValue<CompressedPathInfoKey> CompressedPathInfoKeys { get; set; } = new();

    [MetaMember("mAgentName")]
    public Symbol AgentName { get; set; }

    [MetaSerializer(typeof(MetaClassSerializer<CompressedPathInfoKey>))]
    public class CompressedPathInfoKey
    {
        [MetaMember("mFocusAgentName")]
        public Symbol FocusAgentName { get; set; }

        [MetaMember("mFocusAgentBoneName")]
        public Symbol FocusAgentBoneName { get; set; }

        [MetaMember("mFocusAgentOffset")]
        public Vector3 FocusAgentOffset { get; set; }

        [MetaMember("mfDampingFactor")]
        public float DampingFactor { get; set; }
    }

    public AnimationValueInterfaceBase AnimationValueInterfaceBase
    {
        get => BaseclassCompressedPathBlockingValue.AnimationValueInterfaceBase;
        set => BaseclassCompressedPathBlockingValue.AnimationValueInterfaceBase = value;
    }
}

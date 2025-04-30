using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PhonemeKey>))]
public class PhonemeKey
{
    [MetaMember("mPhoneme")]
    public Symbol Phoneme { get; set; }

    [MetaMember("mFadeInTime")]
    public float FadeInTime { get; set; }

    [MetaMember("mHoldTime")]
    public float HoldTime { get; set; }

    [MetaMember("mFadeOutTime")]
    public float FadeOutTime { get; set; }

    [MetaMember("mTargetContribution")]
    public float TargetContribution { get; set; }
}
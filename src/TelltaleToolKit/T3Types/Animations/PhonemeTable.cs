using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PhonemeTable>))]
public class PhonemeTable
{
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<PhonemeEntry>))]
    public class PhonemeEntry
    {
        [MetaMember("mAnimation")]
        public AnimOrChore Animation { get; set; }

        [MetaMember("mContributionScalar")]
        public float ContributionScalar { get; set; } = 1.0f;

        [MetaMember("mTimeScalar")]
        public float TimeScalar { get; set; } = 1.0f;
    }

    [MetaMember("mName")]
    public string Name { get; set; }

    [MetaMember("mContributionScaler")]
    public float ContributionScaler { get; set; } = 1.0f;

    [MetaMember("mAnimations")]
    public Dictionary<Symbol, PhonemeEntry> Animations { get; set; } = [];
}
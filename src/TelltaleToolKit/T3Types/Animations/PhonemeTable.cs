using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(MetaClassSerializer<PhonemeTable>))]
public class PhonemeTable
{
    [MetaSerializer(typeof(MetaClassSerializer<PhonemeEntry>))]
    public class PhonemeEntry
    {
        [MetaMember("mAnimation")]
        public AnimOrChore Animation { get; set; }

        [MetaMember("mContributionScalar")]
        public float ContributionScalar { get; set; } = 1.0f;

        [MetaMember("mTimeScalar")]
        public float TimeScalar { get; set; } = 1.0f;

        [MetaMember("mRelativePriority")]
        public int RelativePriority { get; set; }
    }

    [MetaMember("mName")]
    public string Name { get; set; }

    [MetaMember("mContributionScaler")]
    public float ContributionScaler { get; set; } = 1.0f;

    [MetaMember("mAnimations")]
    public Dictionary<Symbol, PhonemeEntry> Animations { get; set; } = new();
}

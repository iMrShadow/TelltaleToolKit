using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(MetaClassSerializer<ActingPalette>))]
public class ActingPalette
{
    [MetaSerializer(typeof(EnumSerializer<ActiveDuring>))]
    public enum ActiveDuring
    {
        always = 1,
        talking = 2,
        listening = 3
    }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mPriority")]
    public int Priority { get; set; }

    [MetaMember("mbActiveByDefault")]
    public bool ActiveByDefault { get; set; }

    [MetaMember("mTimeBetweenActions")]
    public Range<float> TimeBetweenActions { get; set; } = new();

    [MetaMember("mAnimsOrChores")]
    public List<AnimOrChore> AnimOrChores { get; set; } = [];

    [MetaMember("mActiveDuring")]
    public EnumActiveDuring ActiveDuringEnum { get; set; }

    [MetaMember("Baseclass_ActingOverridablePropOwner")]
    public ActingOverridablePropOwner BaseClassActingOverridablePropOwner { get; set; }

    [MetaMember("Baseclass_UID::Owner")]
    public Owner BaseClassOwner { get; set; }

    [MetaMember("mFirstActionDelayRange")]
    public Range<float> FirstActionDelayRange { get; set; }

    [MetaMember("mResources")]
    public List<ActingResource> Resources { get; set; } = [];

    [MetaMember("mGroupMembershipUID")]
    public int GroupMembershipUID { get; set; }

    [MetaMember("mAnimFadeInOut")]
    public float AnimFadeInOut { get; set; }

    [MetaMember("mAnimPreDelay")]
    public float AnimPreDelay { get; set; }

    [MetaMember("mAnimPostDelay")]
    public float AnimPostDelay { get; set; }

    [MetaMember("mScaleRange")]
    public Range<float> ScaleRange { get; set; }

    [MetaMember("mPaletteResourceGroups")]
    public Dictionary<string, float> PaletteResourceGroups { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<EnumActiveDuring>))]
    public struct EnumActiveDuring
    {
        [MetaMember("mVal")]
        public ActiveDuring Val { get; set; }
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Audio;

// SoundAmbience

[MetaSerializer(typeof(MetaClassSerializer<AmbienceDefinition>))]
public class AmbienceDefinition
{
    [MetaMember("mEvents")]
    public List<EventContext> Events { get; set; } = [];
}

[MetaSerializer(typeof(MetaClassSerializer<EventContext>))]
public class EventContext
{
    [MetaMember("mBaseclass_SoundEventNameBase")]
    public SoundEventNameBase BaseclassSoundEventNameBase { get; set; } = new();

    [MetaMember("mEventname")]
    public SoundEventName0 Eventname { get; set; } = new();

    [MetaMember("mPlayChance")]
    public float PlayChance { get; set; }

    [MetaMember("mSilentTimeRange")]
    public Range<float> SilentTimeRange { get; set; } = new();

    [MetaMember("mPlayTimeRange")]
    public Range<float> PlayTimeRange { get; set; } = new();

    [MetaMember("mVolumeRangedB")]
    public Range<float> VolumeRangedB { get; set; } = new();

    [MetaMember("mVolumeFadeTimeRange")]
    public Range<float> VolumeFadeTimeRange { get; set; } = new();
}

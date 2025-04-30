using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

/// <summary>
/// Main class for .reverb files.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<SoundReverbDefinition>))]
public class SoundReverbDefinition
{
    [MetaMember("mbEnabled")]
    public bool Enabled { get; set; }

    [MetaMember("mfRoomEffectLevel")]
    public float RoomEffectLevel { get; set; }

    [MetaMember("mfRoomEffectLevelHighFrequency")]
    public float RoomEffectLevelHighFrequency { get; set; }

    [MetaMember("mfRoomEffectLevelLowFrequency")]
    public float RoomEffectLevelLowFrequency { get; set; }

    [MetaMember("mfDecayTime")]
    public float DecayTime { get; set; }

    [MetaMember("mfDecayHighFrequencyRatio")]
    public float DecayHighFrequencyRatio { get; set; }

    [MetaMember("mfReflections")]
    public float Reflections { get; set; }

    [MetaMember("mfReflectionsDelay")]
    public float ReflectionsDelay { get; set; }

    [MetaMember("mfReverb")]
    public float Reverb { get; set; }

    [MetaMember("mfReverbDelay")]
    public float ReverbDelay { get; set; }

    [MetaMember("mfHighFrequencyReference")]
    public float HighFrequencyReference { get; set; }

    [MetaMember("mfLowFrequencyReference")]
    public float LowFrequencyReference { get; set; }

    [MetaMember("mfDiffusion")]
    public float Diffusion { get; set; }

    [MetaMember("mfDensity")]
    public float Density { get; set; }
}
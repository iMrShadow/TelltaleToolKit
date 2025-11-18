using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<SoundReverbPreset>))]
public class SoundReverbPreset
{
    [MetaMember("mPreset")]
    public int Preset { get; set; }
}
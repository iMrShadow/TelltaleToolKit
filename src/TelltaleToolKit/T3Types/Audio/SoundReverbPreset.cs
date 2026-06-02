using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

[MetaSerializer(typeof(MetaClassSerializer<SoundReverbPreset>))]
public class SoundReverbPreset
{
    [MetaMember("mPreset")]
    public int Preset { get; set; }
}

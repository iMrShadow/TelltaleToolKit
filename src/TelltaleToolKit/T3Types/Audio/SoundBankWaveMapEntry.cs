using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

[MetaSerializer(typeof(MetaClassSerializer<SoundBankWaveMapEntry>))]
public class SoundBankWaveMapEntry
{
    [MetaMember("fLengthSeconds")]
    public float LengthSeconds { get; set; }

    [MetaMember("strFileName")]
    public string FileName { get; set; } = string.Empty;
}

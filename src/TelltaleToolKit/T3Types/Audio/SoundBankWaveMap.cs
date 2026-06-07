using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

//.BANKWAVEMAP FILES
[MetaSerializer(typeof(MetaClassSerializer<SoundBankWaveMap>))]
public class SoundBankWaveMap
{
    [MetaMember("mWaveMap")]
    public Dictionary<Symbol, SoundBankWaveMapEntry> WaveMap { get; set; } = [];
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

[MetaSerializer(typeof(MetaClassSerializer<EnumSoundMode>))]
public class AudioSound
{
    public struct EnumSoundMode
    {
        [MetaMember("mVal")]
        public SoundMode SoundMode { get; set; }
    }

    [MetaSerializer(typeof(EnumSerializer<SoundMode>))]
    public enum SoundMode
    {
        mono = 0,
        stereo = 1,
        surround = 2
    }
}

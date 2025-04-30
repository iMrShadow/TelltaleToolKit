using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumSoundMode>))]
public class AudioSound
{
    // TODO: Check CSI Hard Evidence
    public struct EnumSoundMode
    {
        [MetaMember("mVal")]
        public SoundMode SoundMode { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<SoundMode>))]
    public enum SoundMode
    {
        // TODO:
        // Verify
    }
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

public class FootSteps
{
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<FootstepBank>))]
    public class FootstepBank
    {
        [MetaMember("mSounds")]
        public List<Handle<SoundData>> Sounds { get; set; } = [];

        [MetaMember("mMaterialFootsteps")]
        public Dictionary<SoundFootsteps.EnumMaterial, List<Handle<SoundData>>> MaterialFootsteps { get; set; } = [];
    }
}
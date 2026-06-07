using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

public class FootSteps
{
    [MetaSerializer(typeof(MetaClassSerializer<FootstepBank>))]
    public class FootstepBank
    {
        [MetaMember("mSounds")]
        public List<Handle<SoundData>> Sounds { get; set; } = [];

        [MetaMember("mMaterialFootsteps")]
        public Dictionary<SoundFootsteps.EnumMaterial, List<Handle<SoundData>>> MaterialFootsteps { get; set; } =
            new();
    }

    internal class FootStepMonitor
    {
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Audio;

public class FootSteps2
{
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<FootstepBank>))]
    public class FootstepBank
    {
        [MetaMember("mEventName")]
        public SoundEventName EventName { get; set; } // NT_DEFAULT is int, adjust as needed

        [MetaMember("mMaterialMap")]
        public Dictionary<SoundFootsteps.EnumMaterial, SoundEventName> MaterialMap { get; set; }
    }
}
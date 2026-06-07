using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Miscellaneous;

namespace TelltaleToolKit.T3Types.Audio;

public class FootSteps2
{
    [MetaSerializer(typeof(MetaClassSerializer<FootstepBank>))]
    public class FootstepBank
    {
        [MetaMember("mEventName")]
        public SoundEventName0 EventName { get; set; } // NT_DEFAULT is int, adjust as needed

        [MetaMember("mMaterialMap")]
        public Dictionary<SoundFootsteps.EnumMaterial, SoundEventName0> MaterialMap { get; set; }
    }
}

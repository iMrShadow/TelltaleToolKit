using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

public class SoundFootsteps
{
    [MetaSerializer(typeof(MetaClassSerializer<EnumMaterial>))]
    public struct EnumMaterial
    {
        [MetaMember("mVal")]

        public Material Val { get; set; }
    }

    [MetaSerializer(typeof(EnumSerializer<Material>))]
    public enum Material
    {
        Never = 0x1,
        Less = 0x2,
        Equal = 0x3,
        LessEqual = 0x4,
        Greater = 0x5,
        NotEqual = 0x6,
        GreaterEqual = 0x7,
        Always = 0x8,
    }
}

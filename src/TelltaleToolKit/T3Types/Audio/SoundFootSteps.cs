using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

public class SoundFootsteps
{
    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumMaterial>))]
    public struct EnumMaterial
    {
        [MetaMember("mVal")]

        public Material Val { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(EnumSerializer<Material>))]
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
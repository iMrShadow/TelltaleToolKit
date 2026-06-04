using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterColorType>))]
public struct EnumEmitterColorType
{
    [MetaMember("mVal")]
    public EmitterColorType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EmitterColorType>))]
    public enum EmitterColorType
    {
        Constant = 0x1,
        Random_Lerp = 0x2,
        Random_Discrete = 0x3,
        BurstLife_Lerp = 0x4,
        Index_Lerp = 0x5,
        KeyControl01 = 0x6
    }
}

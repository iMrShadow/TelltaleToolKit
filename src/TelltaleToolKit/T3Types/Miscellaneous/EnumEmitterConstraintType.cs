using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterConstraintType>))]
public struct EnumEmitterConstraintType
{
    [MetaMember("mVal")]
    public EmitterConstraintType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EmitterConstraintType>))]
    public enum EmitterConstraintType
    {
        _None = 0x1,
        _0Point = 0x2,
        _1Point = 0x3,
        _2Point = 0x4
    }
}

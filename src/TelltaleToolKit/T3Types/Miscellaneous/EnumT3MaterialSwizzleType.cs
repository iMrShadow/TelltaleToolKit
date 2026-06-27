using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3MaterialSwizzleType>))]
public struct EnumT3MaterialSwizzleType
{
    [MetaMember("mVal")]
    public T3MaterialSwizzleType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3MaterialSwizzleType>))]
    public enum T3MaterialSwizzleType
    {
        None = 0x0,
        X = 0x1,
        Y = 0x2,
        Z = 0x3,
        W = 0x4,
        Zero = 0x5,
        One = 0x6,
        FirstComponent = 0x1,
        FirstNumeric = 0x5
    }
}

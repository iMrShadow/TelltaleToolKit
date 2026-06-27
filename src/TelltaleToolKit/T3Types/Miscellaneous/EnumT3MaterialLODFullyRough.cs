using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3MaterialLODFullyRough>))]
public struct EnumT3MaterialLODFullyRough
{
    [MetaMember("mVal")]
    public T3MaterialLODFullyRough Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3MaterialLODFullyRough>))]
    public enum T3MaterialLODFullyRough
    {
        Never = 0x0,
        OnLowestQuality = 0x1,
        OnLowQuality = 0x2,
        Always = 0x3
    }
}

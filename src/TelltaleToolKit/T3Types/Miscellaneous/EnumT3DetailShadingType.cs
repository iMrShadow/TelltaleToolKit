using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3DetailShadingType>))]
public struct EnumT3DetailShadingType
{
    [MetaMember("mVal")]
    public T3DetailShadingType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3DetailShadingType>))]
    public enum T3DetailShadingType
    {
        No_Detail_Map = 0x0,
        Old_Toon = 0x1,
        Sharp_Detail = 0x2,
        Packed_Detail_And_Tiled_Packed_Detail = 0x3,
        Packed_Detail = 0x4,
        Single_Channel_Detail = 0x5,
        Animated_Detail = 0x6
    }
}

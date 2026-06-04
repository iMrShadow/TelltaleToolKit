using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumDepthOfFieldType>))]
public struct EnumDepthOfFieldType
{
    [MetaMember("mVal")]
    public DepthOfFieldType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<DepthOfFieldType>))]
    public enum DepthOfFieldType
    {
        Default = 0x1,
        Brush = 0x2
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHTextAlignmentType>))]
public struct EnumHTextAlignmentType
{
    [MetaMember("mVal")]
    public HTextAlignmentType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HTextAlignmentType>))]
    public enum HTextAlignmentType
    {
        None = 0,
        LeftJustified = 1,
        Centered = 2,
        RightJustified = 3
    }
}

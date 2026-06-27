using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOPreset>))]
public struct EnumHBAOPreset
{
    [MetaMember("mVal")]
    public HBAOPreset Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HBAOPreset>))]
    public enum HBAOPreset
    {
        FromTool = 0x0,
        XBone = 0x1,
        PS4 = 0x2,
        Disabled = 0x3,
        Low = 0x4,
        Medium = 0x5,
        High = 0x6,
        Ultra = 0x7
    }
}

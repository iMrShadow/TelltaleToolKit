using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOPreset>))]
public struct EnumHBAOPreset
{
    [MetaMember("mVal")]
    public HBAOPreset Val { get; set; }
}

[MetaSerializer(typeof(EnumSerializer<HBAOPreset>))]
public enum HBAOPreset
{
    // TODO:
}

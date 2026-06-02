using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAOPreset>))]
public struct EnumHBAOPreset
{
    [MetaMember("mVal")]
    public HBAOPreset Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAOPreset>))]
public enum HBAOPreset
{
    // TODO:
}

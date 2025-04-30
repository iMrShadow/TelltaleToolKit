using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
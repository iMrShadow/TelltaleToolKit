using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAOResolution>))]
public struct EnumHBAOResolution
{
    [MetaMember("mVal")]
    public HBAOResolution Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAOResolution>))]
public enum HBAOResolution
{
    // TODO:
}
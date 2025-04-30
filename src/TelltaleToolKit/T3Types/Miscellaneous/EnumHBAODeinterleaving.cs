using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAODeinterleaving>))]
public struct EnumHBAODeinterleaving
{
    [MetaMember("mVal")]
    public HBAODeinterleaving Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAODeinterleaving>))]
public enum HBAODeinterleaving
{
    // TODO:
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAODeinterleaving>))]
public struct EnumHBAODeinterleaving
{
    [MetaMember("mVal")]
    public HBAODeinterleaving Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HBAODeinterleaving>))]
    public enum HBAODeinterleaving
    {
        _Disabled = 0x0,
        _2x = 0x1,
        _4x = 0x2
    }
}

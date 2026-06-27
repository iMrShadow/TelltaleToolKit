using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumeTangentModes>))]
public struct EnumeTangentModes
{
    [MetaMember("mVal")]
    public TangentModes Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<TangentModes>))]
    public enum TangentModes
    {
        eTangentUnknown,
        eTangentStepped,
        eTangentKnot,
        eTangentSmooth,
        eTangentFlat
    }
}

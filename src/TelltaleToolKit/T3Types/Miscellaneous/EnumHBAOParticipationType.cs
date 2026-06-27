using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOParticipationType>))]
public struct EnumHBAOParticipationType
{
    [MetaMember("mVal")]
    public HBAOParticipationType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HBAOParticipationType>))]
    public enum HBAOParticipationType
    {
        Auto = 0,
        ForceOn = 1,
        ForceOff = 2
    }
}

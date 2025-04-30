using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAOParticipationType>))]
public struct EnumHBAOParticipationType
{
    [MetaMember("mVal")]
    public HBAOParticipationType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAOParticipationType>))]
public enum HBAOParticipationType
{
    // TODO:
}
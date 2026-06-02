using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

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

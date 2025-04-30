using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumParticleSortMode>))]
public struct EnumParticleSortMode
{
    [MetaMember("mVal")]
    public ParticleSortMode Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<ParticleSortMode>))]
public enum ParticleSortMode
{
    //  eParticleSortMode_
    None = 0x1,
    ByDistance = 0x2,
    YoungestFirst = 0x3,
    OldestFirst = 0x4,
}
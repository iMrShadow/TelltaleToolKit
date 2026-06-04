using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumParticleSortMode>))]
public struct EnumParticleSortMode
{
    [MetaMember("mVal")]
    public ParticleSortMode Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<ParticleSortMode>))]
    public enum ParticleSortMode
    {
        None = 0x1,
        ByDistance = 0x2,
        YoungestFirst = 0x3,
        OldestFirst = 0x4
    }
}

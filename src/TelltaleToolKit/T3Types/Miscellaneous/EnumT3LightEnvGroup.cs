using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvGroup>))]
public struct EnumT3LightEnvGroup
{
    [MetaMember("mVal")]
    public T3LightEnvGroup Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvGroup>))]
public enum T3LightEnvGroup
{
    //   eLightEnvGroup_
    None = -2,
    Default = -1,
    Group0 = 0x0,
    Group1 = 0x1,
    Group2 = 0x2,
    Group3 = 0x3,
    Group4 = 0x4,
    Group5 = 0x5,
    Group6 = 0x6,
    Group7 = 0x7,
    CountWithNoAmbient = 0x8,
    Group8_Unused = 0x8,
    Group9_Unused = 0x9,
    Group10_Unused = 0xA,
    Group11_Unused = 0xB,
    Group12_Unused = 0xC,
    Group13_Unused = 0xD,
    Group14_Unused = 0xE,
    Group15_Unused = 0xF,
    AmbientGroup0 = 0x10,
    AmbientGroup1 = 0x11,
    AmbientGroup2 = 0x12,
    AmbientGroup3 = 0x13,
}
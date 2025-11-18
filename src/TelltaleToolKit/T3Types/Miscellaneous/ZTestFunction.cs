using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ZTestFunction>))]
public struct ZTestFunction
{
    [MetaMember("mZTestType")]

    public FuncTypes ZTestType { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<FuncTypes>))]
public enum FuncTypes
{
    Never = 0x1,
    Less = 0x2,
    Equal = 0x3,
    LessEqual = 0x4,
    Greater = 0x5,
    NotEqual = 0x6,
    GreaterEqual = 0x7,
    Always = 0x8,
}
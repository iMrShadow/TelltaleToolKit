using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<ZTestFunction>))]
public struct ZTestFunction
{
    [MetaMember("mZTestType")]

    public FuncTypes ZTestType { get; set; }

    [MetaSerializer(typeof(EnumSerializer<FuncTypes>))]
    public enum FuncTypes
    {
        Never = 0x1,
        Less = 0x2,
        Equal = 0x3,
        LessEqual = 0x4,
        Greater = 0x5,
        NotEqual = 0x6,
        GreaterEqual = 0x7,
        Always = 0x8
    }
}

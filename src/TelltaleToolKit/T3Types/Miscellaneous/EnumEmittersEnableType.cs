using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmittersEnableType>))]
public struct EnumEmittersEnableType
{
    [MetaMember("mVal")]
    public EmittersEnableType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EmittersEnableType>))]
    public enum EmittersEnableType
    {
        All = 1,
        Random = 2,
        Sequential = 3
    }
}

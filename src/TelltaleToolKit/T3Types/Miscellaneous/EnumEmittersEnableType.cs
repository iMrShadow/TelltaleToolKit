using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmittersEnableType>))]
public struct EnumEmittersEnableType
{
    [MetaMember("mVal")]
    public EmittersEnableType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmittersEnableType>))]
public enum EmittersEnableType
{
    // TODO:
}

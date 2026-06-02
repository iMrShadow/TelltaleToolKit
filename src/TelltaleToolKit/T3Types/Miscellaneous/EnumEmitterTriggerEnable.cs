using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterTriggerEnable>))]
public struct EnumEmitterTriggerEnable
{
    [MetaMember("mVal")]
    public EmitterTriggerEnable Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterTriggerEnable>))]
public enum EmitterTriggerEnable
{
    // TODO:
}

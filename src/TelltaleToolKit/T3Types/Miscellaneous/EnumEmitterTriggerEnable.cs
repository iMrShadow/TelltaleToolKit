using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
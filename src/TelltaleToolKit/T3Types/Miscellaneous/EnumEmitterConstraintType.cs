using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterConstraintType>))]
public struct EnumEmitterConstraintType
{
    [MetaMember("mVal")]
    public EmitterConstraintType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterConstraintType>))]
public enum EmitterConstraintType
{
    // TODO:
}

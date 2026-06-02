using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

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

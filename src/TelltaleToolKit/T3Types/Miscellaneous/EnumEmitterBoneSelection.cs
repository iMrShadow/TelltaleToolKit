using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterBoneSelection>))]
public struct EnumEmitterBoneSelection
{
    [MetaMember("mVal")]
    public EmitterBoneSelection Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterBoneSelection>))]
public enum EmitterBoneSelection
{
    // TODO:
}

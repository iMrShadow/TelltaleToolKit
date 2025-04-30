using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
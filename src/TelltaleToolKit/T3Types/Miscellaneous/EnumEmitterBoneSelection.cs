using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterBoneSelection>))]
public struct EnumEmitterBoneSelection
{
    [MetaMember("mVal")]
    public EmitterBoneSelection Val { get; set; }


    [MetaSerializer(typeof(EnumSerializer<EmitterBoneSelection>))]
    public enum EmitterBoneSelection
    {
        All = 0x1,
        Children = 0x2
    }
}

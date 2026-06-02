using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumTonemapType>))]
public struct EnumTonemapType
{
    [MetaMember("mVal")]
    public TonemapType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<TonemapType>))]
public enum TonemapType
{
    // TODO:
}

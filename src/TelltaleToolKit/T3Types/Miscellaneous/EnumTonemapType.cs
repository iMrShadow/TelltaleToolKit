using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
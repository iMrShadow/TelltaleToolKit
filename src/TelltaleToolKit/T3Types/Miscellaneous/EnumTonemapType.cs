using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(MetaClassSerializer<EnumTonemapType>))]
public struct EnumTonemapType
{
    [MetaMember("mVal")]
    public TonemapType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<TonemapType>))]
    public enum TonemapType
    {
        Default = 1,
        Filmic = 2
    }
}

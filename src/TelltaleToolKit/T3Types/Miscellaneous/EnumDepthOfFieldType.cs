using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumDepthOfFieldType>))]
public struct EnumDepthOfFieldType
{
    [MetaMember("mVal")]
    public DepthOfFieldType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<DepthOfFieldType>))]
public enum DepthOfFieldType
{
    // TODO:
}
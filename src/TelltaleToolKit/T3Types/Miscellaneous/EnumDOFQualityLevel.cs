using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumDOFQualityLevel>))]
public struct EnumDOFQualityLevel
{
    [MetaMember("mVal")]
    public DOFQualityLevel Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<DOFQualityLevel>))]
public enum DOFQualityLevel
{
    // TODO:
}
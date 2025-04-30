using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumBokehOcclusionType>))]
public struct EnumBokehOcclusionType
{
    [MetaMember("mVal")]
    public BokehOcclusionType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<BokehOcclusionType>))]
public enum BokehOcclusionType
{
    // TODO:
}
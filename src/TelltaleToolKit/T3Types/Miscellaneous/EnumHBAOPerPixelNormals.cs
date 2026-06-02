using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumHBAOPerPixelNormals>))]
public struct EnumHBAOPerPixelNormals
{
    [MetaMember("mVal")]
    public HBAOPerPixelNormals Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<HBAOPerPixelNormals>))]
public enum HBAOPerPixelNormals
{
    //TODO:
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumHBAOPerPixelNormals>))]
public struct EnumHBAOPerPixelNormals
{
    [MetaMember("mVal")]
    public HBAOPerPixelNormals Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<HBAOPerPixelNormals>))]
    public enum HBAOPerPixelNormals
    {
        GBuffer = 0x0,
        Reconstructed = 0x1
    }
}

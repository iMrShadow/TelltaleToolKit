using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<MovieCaptureInfo>))]
public class MovieCaptureInfo
{
    [MetaSerializer(typeof(EnumSerializer<CompressorType>))]
    public enum CompressorType
    {
        IV50 = 5,
        MSVC = 4,
        IV32 = 3,
        CVID = 2,
        Uncompressed = 1
    }

    [MetaMember("mFPS")]
    public int Fps { get; set; }

    [MetaMember("mCType")]
    public EnumCompressorType CType { get; set; } = new();

    [MetaSerializer(typeof(MetaClassSerializer<EnumCompressorType>))]
    public struct EnumCompressorType
    {
        [MetaMember("mVal")]
        public CompressorType Val { get; set; }
    }
}

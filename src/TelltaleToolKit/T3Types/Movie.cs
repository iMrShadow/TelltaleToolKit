using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<MovieCaptureInfo>))]
public class MovieCaptureInfo
{
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
    [MetaSerializer(typeof(EnumSerializer<CompressorType>))]
    public enum CompressorType{}
}

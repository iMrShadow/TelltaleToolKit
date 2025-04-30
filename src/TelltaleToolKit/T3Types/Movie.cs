using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<MovieCaptureInfo>))]
public class MovieCaptureInfo
{
    [MetaMember("mFPS")]
    public int Fps { get; set; }

    [MetaMember("mCType")]
    public EnumCompressorType CType { get; set; } = new();


    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumCompressorType>))]
    public struct EnumCompressorType
    {
        [MetaMember("mVal")]
        public CompressorType Val { get; set; }
    }
    [MetaClassSerializerGlobal(typeof(EnumSerializer<CompressorType>))]
    public enum CompressorType;
}
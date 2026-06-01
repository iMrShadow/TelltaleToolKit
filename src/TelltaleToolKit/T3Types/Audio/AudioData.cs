using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Audio;

/// <summary>
/// Main class for .aud files.
/// It is mainly used up to TWDS1 (MTRE).
/// In MSV4+ games, only the filename and length are serialized (if these files are even found in the first place).
/// Presumably replaced by FMOD.
/// </summary>
[MetaClassSerializerGlobal(typeof(Serializer))]
public class AudioData
{
    [MetaMember("mFilename")]
    public string FileName { get; set; } = string.Empty;

    [MetaMember("mLength")]
    public float Length { get; set; }

    [MetaMember("mbStreamed")]
    public bool IsStreamed { get; set; }

    [MetaMember("mDataFormat")]
    public int DataFormat { get; set; }

    [MetaMember("mBytesPerSecond")]
    public int BytesPerSecond { get; set; }

    [MetaMember("mDSBufferBytes")]
    public int DsBufferBytes { get; set; }

    [MetaMember("mStreamInfo")]
    public Streamed StreamInfo { get; set; } = new();

    public byte[] OggBuffer { get; set; } = [];
    public int NumChannels { get; set; }
    public int SampleSizeBytes { get; set; }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Streamed>))]
    public struct Streamed
    {
        [MetaMember("mStreamRegionSize")]
        public int StreamRegionSize { get; set; }

        [MetaMember("mStreamBufferSecs")]
        public float StreamBufferSecs { get; set; }
    }

    public class Serializer : MetaClassSerializer<AudioData>
    {
        private static readonly DefaultClassSerializer<AudioData> DefaultSerializer = new();

        public override void Serialize(ref AudioData obj, MetaStream stream)
        {
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                if (stream.Params.StreamVersion >= 4)
                {
                    obj.NumChannels = stream.ReadInt16();
                    short bitDepth = stream.ReadInt16();
                    int sampleRate = stream.ReadInt32();
                    int bytesPerSecond = stream.ReadInt32();
                    obj.SampleSizeBytes = stream.ReadInt16();
                    short always1 = stream.ReadInt16();
                    int always328160 = stream.ReadInt32();
                    int bytesPerSecCopy = stream.ReadInt32();

                    obj.OggBuffer = stream.ReadBytes((int)stream.GetRemainingSectionBytes());
                }
            }
        }
    }
}

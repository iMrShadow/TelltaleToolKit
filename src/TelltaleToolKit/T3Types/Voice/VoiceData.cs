using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Voice;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class VoiceData
{
    [MetaMember("mbEncrypted")]
    public bool Encrypted { get; set; } = false;

    [MetaMember("mLength")]
    public float Length { get; set; } = 0.0f;

    [MetaMember("mAllPacketsSize")]
    public int AllPacketsSize => VoiceDataBuffer.Length;

    [MetaMember("mPacketSamples")]
    public int PacketSamples { get; set; } = 0;

    [MetaMember("mSampleRate")]
    public int SampleRate { get; set; } = 0;

    [MetaMember("mMode")]
    public int Mode { get; set; } = 0;

    [MetaMember("mPacketPositions")]
    public List<int> PacketPositions { get; set; } = [];

    public byte[] VoiceDataBuffer { get; set; }


    public class Serializer : MetaClassSerializer<VoiceData>
    {
        private static readonly DefaultClassSerializer<VoiceData> DefaultSerializer = new();

        public override void Serialize(ref VoiceData obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.VoiceDataBuffer);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.VoiceDataBuffer = streamReader.ReadBytes(obj.AllPacketsSize);
            }
        }
    }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Voice;

/// <summary>
/// Represents the class for .vox files.
/// </summary>
[MetaSerializer(typeof(Serializer))]
public class VoiceData
{
    [MetaMember("mbEncrypted")]
    public bool Encrypted { get; set; } = false;

    [MetaMember("mLength")]
    public float Length { get; set; } = 0.0f;

    [MetaMember("mAllPacketsSize")]
    public int AllPacketsSize { get; set; }

    [MetaMember("mPacketSamples")]
    public int PacketSamples { get; set; } = 0;

    [MetaMember("mSampleRate")]
    public int SampleRate { get; set; } = 0;

    [MetaMember("mMode")]
    public int Mode { get; set; } = 0;

    [MetaMember("mPacketPositions")]
    public List<int> PacketPositions { get; set; } = [];

    public byte[] VoiceDataBuffer { get; set; } = [];

    public class Serializer : MetaSerializer<VoiceData>
    {
        private static readonly MetaClassSerializer<VoiceData> s_metaClassSerializer = new();

        public override void PreSerialize(ref VoiceData? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new VoiceData();
        }

        public override void Serialize(ref VoiceData obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.VoiceDataBuffer);
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                obj.VoiceDataBuffer = stream.ReadBytes(obj.AllPacketsSize);
            }
        }
    }
}

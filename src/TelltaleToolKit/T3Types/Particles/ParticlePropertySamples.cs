using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Particles;

[MetaSerializer(typeof(Serializer))]
public class ParticlePropertySamples
{
    [MetaMember("mSampleCount")]
    public int SampleCount { get; set; }

    public List<Sample> Samples { get; set; } = [];

    [StructLayout(LayoutKind.Sequential)]
    public struct Sample
    {
        public Vector4 Position;
        public Vector4 Orientation;
        public Vector4 Color;
    }

    public class Serializer : MetaSerializer<ParticlePropertySamples>
    {
        private static readonly MetaClassSerializer<ParticlePropertySamples> s_metaClassSerializer = new();

        public override void PreSerialize(ref ParticlePropertySamples? obj, MetaStream stream,
            MetaClassType? type = null) =>
            obj ??= new ParticlePropertySamples();

        public override void Serialize(ref ParticlePropertySamples obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            int sampleCount = obj.SampleCount;

            if (stream.Mode is MetaStreamMode.Write)
            {
                if (sampleCount == 0)
                {
                    return;
                }

                Sample[] samplesArray = obj.Samples.ToArray();
                Span<byte> rawBytes = MemoryMarshal.AsBytes(samplesArray.AsSpan());
                stream.WriteBytes(rawBytes);
            }
            else
            {
                if (sampleCount == 0)
                {
                    obj.Samples = [];
                    return;
                }

                int byteCount = sampleCount * Unsafe.SizeOf<Sample>();
                byte[] rawBytes = new byte[byteCount];
                stream.ReadBytes(rawBytes);
                Span<Sample> samplesSpan = MemoryMarshal.Cast<byte, Sample>(rawBytes.AsSpan());
                obj.Samples = new List<Sample>(samplesSpan.ToArray());
            }
        }
    }
}

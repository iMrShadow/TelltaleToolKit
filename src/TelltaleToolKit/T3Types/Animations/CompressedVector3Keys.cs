using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class CompressedVector3Keys
{
    public string Name { get; set; }
    public byte[] Buffer { get; set; }
    public float MinTime { get; set; }
    public float MaxTime { get; set; }
    public int Flags { get; set; }
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }

    public class Serializer : MetaClassSerializer<CompressedVector3Keys>
    {
        public override void Serialize(ref CompressedVector3Keys obj, MetaStream stream)
        {
            // TODO: Test this type.

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.Name = streamReader.ReadString();
                obj.Flags = streamReader.ReadInt32();
                
                obj.Min = new Vector3() { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };
                obj.Max = new Vector3() { X = streamReader.ReadSingle(), Y = streamReader.ReadSingle(), Z = streamReader.ReadSingle() };

                obj.MinTime = streamReader.ReadSingle();
                obj.MaxTime = streamReader.ReadSingle();

                short numSamples = streamReader.ReadInt16();

                obj.Buffer = streamReader.ReadBytes(numSamples * 6);
            }
        }
    }
}
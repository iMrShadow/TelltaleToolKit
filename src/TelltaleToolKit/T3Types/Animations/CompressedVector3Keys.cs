using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class CompressedVector3Keys
{
    public string Name { get; set; }
    public byte[] Buffer { get; set; }
    public float MinTime { get; set; }
    public float MaxTime { get; set; }
    public int Flags { get; set; }
    public Vector3 Min { get; set; }
    public Vector3 Max { get; set; }

    public class Serializer : MetaSerializer<CompressedVector3Keys>
    {
        public override void Serialize(ref CompressedVector3Keys obj, MetaStream stream, MetaClassType? type = null)
        {
            // TODO: Test this type.
            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Name);
                stream.Write(obj.Flags);

                stream.Write(obj.Min.X);
                stream.Write(obj.Min.Y);
                stream.Write(obj.Min.Z);
                stream.Write(obj.Max.X);
                stream.Write(obj.Max.Y);
                stream.Write(obj.Max.Z);
                stream.Write(obj.MinTime);
                stream.Write(obj.MaxTime);
                stream.Write((short)(obj.Buffer.Length / 6));
                stream.Write(obj.Buffer);
            }
            else
            {
                obj.Name = stream.ReadString();
                obj.Flags = stream.ReadInt32();

                obj.Min = new Vector3 { X = stream.ReadSingle(), Y = stream.ReadSingle(), Z = stream.ReadSingle() };
                obj.Max = new Vector3 { X = stream.ReadSingle(), Y = stream.ReadSingle(), Z = stream.ReadSingle() };

                obj.MinTime = stream.ReadSingle();
                obj.MaxTime = stream.ReadSingle();

                short numSamples = stream.ReadInt16();

                obj.Buffer = stream.ReadBytes(numSamples * 6);
            }
        }
    }
}

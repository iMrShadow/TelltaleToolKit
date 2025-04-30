using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(Serializer))]
public struct Vector3
{
    [MetaMember("x")]
    public float X { get; set; }

    [MetaMember("y")]
    public float Y { get; set; }

    [MetaMember("z")]
    public float Z { get; set; }


    public class Serializer : MetaClassSerializer<Vector3>
    {
        public override void Serialize(ref Vector3 obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.X);
                streamWriter.Write(obj.Y);
                streamWriter.Write(obj.Z);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.X = streamReader.ReadSingle();
                obj.Y = streamReader.ReadSingle();
                obj.Z = streamReader.ReadSingle();
            }
        }
    }

    public readonly override string ToString() => $"({X}, {Y}, {Z})";
}
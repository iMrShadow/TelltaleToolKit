using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(Serializer))]
public struct Quaternion
{
    [MetaMember("x")] public float X;

    [MetaMember("y")] public float Y;

    [MetaMember("z")] public float Z;

    [MetaMember("w")] public float W;

    public class Serializer : MetaClassSerializer<Quaternion>
    {
        public override void Serialize(ref Quaternion obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.X);
                streamWriter.Write(obj.Y);
                streamWriter.Write(obj.Z);
                streamWriter.Write(obj.W);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.X = streamReader.ReadSingle();
                obj.Y = streamReader.ReadSingle();
                obj.Z = streamReader.ReadSingle();
                obj.W = streamReader.ReadSingle();
            }
        }
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {Z}, {W})";
    }
}
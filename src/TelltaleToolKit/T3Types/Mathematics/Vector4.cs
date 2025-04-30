using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Mathematics;


[MetaClassSerializerGlobal(typeof(Serializer))]
public struct Vector4
{
    [MetaMember("x")]
    public float X { get; set; }

    [MetaMember("y")]
    public float Y { get; set; }

    [MetaMember("z")]
    public float Z { get; set; }
    
    [MetaMember("W")]
    public float W { get; set; }


    public class Serializer : MetaClassSerializer<Vector4>
    {
        public override void Serialize(ref Vector4 obj, MetaStream stream)
        {
            PreSerialize(ref obj, stream, null);
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
    public readonly override string ToString() => $"({X}, {Y}, {Z}, {W})";

    //  public readonly override string ToString() => $"{this}";
}
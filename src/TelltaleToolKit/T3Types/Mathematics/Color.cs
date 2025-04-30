using System.Runtime.InteropServices;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(Serializer))]
[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct Color
{
    [MetaMember("r")]
    public float R;

    [MetaMember("g")]
    public float G;

    [MetaMember("b")]
    public float B;

    [MetaMember("a")]
    public float A;


    public class Serializer : MetaClassSerializer<Color>
    {
        public override void Serialize(ref Color obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.R);
                streamWriter.Write(obj.G);
                streamWriter.Write(obj.B);
                streamWriter.Write(obj.A);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.R = streamReader.ReadSingle();
                obj.G = streamReader.ReadSingle();
                obj.B = streamReader.ReadSingle();
                obj.A = streamReader.ReadSingle();
            }
        }
    }

    public override string ToString()
    {
        return $"Color: ({R}, {G}, {B}, {A})";
    }
}
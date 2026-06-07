using System.Runtime.InteropServices;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaSerializer(typeof(Serializer))]
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


    public class Serializer : MetaSerializer<Color>
    {
        public override void Serialize(ref Color obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                if (stream.Params.CanModifySerializedClassesList)
                {
                    MetaClass? description = stream.GetMetaClass(typeof(Color));
                    stream.AddVersionInfo(description);
                }

                stream.Write(obj.R);
                stream.Write(obj.G);
                stream.Write(obj.B);
                stream.Write(obj.A);
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                obj.R = stream.ReadSingle();
                obj.G = stream.ReadSingle();
                obj.B = stream.ReadSingle();
                obj.A = stream.ReadSingle();
            }
        }
    }

    public override string ToString() => $"Color: ({R}, {G}, {B}, {A})";
}

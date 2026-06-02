using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaSerializer(typeof(MetaClassSerializer<Polar>))]
public struct Polar
{
    [MetaMember("mR")]
    public float Radius { get; set; }

    [MetaMember("mTheta")]
    public float Theta { get; set; }

    [MetaMember("mPhi")]
    public float Phi { get; set; }

    public static void Serialize(ref object obj, MetaStream stream, MetaClass desc)
    {
        Polar instance = (Polar)obj;

        if (stream.Mode is MetaStreamMode.Write)
        {
            stream.Write(instance.Radius);
            stream.Write(instance.Theta);
            stream.Write(instance.Phi);
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            instance.Radius = stream.ReadSingle();
            instance.Theta = stream.ReadSingle();
            instance.Phi = stream.ReadSingle();
        }
    }
}

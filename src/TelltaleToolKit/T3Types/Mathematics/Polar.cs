using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Mathematics;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Polar>))]
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

        if (stream is MetaStreamWriter streamWriter)
        {
            streamWriter.Write(instance.Radius);
            streamWriter.Write(instance.Theta);
            streamWriter.Write(instance.Phi);
        }
        else if (stream is MetaStreamReader streamReader)
        {
            instance.Radius = streamReader.ReadSingle();
            instance.Theta = streamReader.ReadSingle();
            instance.Phi = streamReader.ReadSingle();
        }
    }
}
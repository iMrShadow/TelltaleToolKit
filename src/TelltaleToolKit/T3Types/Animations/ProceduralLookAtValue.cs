using System.Numerics;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class ProceduralLookAtValue
{
    [MetaMember("Baseclass_AnimatedValueInterface<Quaternion>")]
    public AnimatedValueInterface<Quaternion> Quaternion { get; set; } = new();

    public class Serializer : MetaClassSerializer<ProceduralLookAtValue>
    {
        public override void Serialize(ref ProceduralLookAtValue obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
            }
        }
    }
}
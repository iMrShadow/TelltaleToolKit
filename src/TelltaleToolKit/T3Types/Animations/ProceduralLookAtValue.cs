using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

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
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
            }
        }
    }
}

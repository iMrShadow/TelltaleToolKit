using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.T3Types.Skeletons;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class ProceduralLookAtValue : IAnimationValueInterface
{
    [MetaMember("Baseclass_AnimatedValueInterface<Transform>")]
    public AnimationValueInterface<Transform> Transform { get; set; } = new();

    public class Serializer : MetaSerializer<ProceduralLookAtValue>
    {
        public override void PreSerialize(ref ProceduralLookAtValue? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new ProceduralLookAtValue();
        }

        public override void Serialize(ref ProceduralLookAtValue obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
            }
        }
    }

    public AnimationValueInterfaceBase AnimationValueInterfaceBase
    {
        get => Transform.AnimationValueInterfaceBase;
        set => Transform.AnimationValueInterfaceBase = value;
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

// .EYES
[MetaSerializer(typeof(Serializer))]
public class ProceduralEyes
{
    [MetaMember("Baseclass_Animation")]
    public Animation BaseclassAnimation { get; set; } = new();

    public class Serializer : MetaSerializer<ProceduralEyes>
    {
        public override void PreSerialize(ref ProceduralEyes? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new ProceduralEyes();
        }

        public override void Serialize(ref ProceduralEyes obj, MetaStream stream, MetaClassType? type = null)
        {
            // Eyes are not used?!?
        }
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(MetaClassSerializer<ActingResource>))]
public class ActingResource
{
    [MetaMember("Baseclass_ActingOverridablePropOwner")]
    public ActingOverridablePropOwner BaseClassActingOverridablePropOwner { get; set; } = new();

    [MetaMember("mResource")]
    public AnimOrChore Resource { get; set; } = new();

    [MetaMember("mRuntimeFlags")]
    public Flags RuntimeFlags { get; set; } = new();

    [MetaMember("mValidIntensityRange")]
    public Range<float> ValidIntensityRange { get; set; } = new();
}

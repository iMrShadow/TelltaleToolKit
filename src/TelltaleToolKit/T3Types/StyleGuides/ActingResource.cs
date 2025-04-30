using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ActingResource>))]
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
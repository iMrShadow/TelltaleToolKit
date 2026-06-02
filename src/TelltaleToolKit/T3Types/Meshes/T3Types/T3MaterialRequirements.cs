using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MaterialRequirements>))]
public class T3MaterialRequirements
{
    [MetaMember("mPasses")]
    public BitSetBase Passes { get; set; }

    [MetaMember("mChannels")]
    public BitSetBase Channels { get; set; }

    [MetaMember("mInputs")]
    public BitSetBase Inputs { get; set; }
}

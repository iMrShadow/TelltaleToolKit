using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Enlighten;

[MetaSerializer(typeof(MetaClassSerializer<EnlightenSignature>))]
public class EnlightenSignature
{
    [MetaMember("mSignature")]
    public ulong Signature { get; set; }

    [MetaMember("mMagicNumber")]
    public uint MagicNumber { get; set; }

    [MetaMember("mVersion")]
    public uint Version { get; set; }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumT3NPRSpecularType>))]
public struct EnumT3NPRSpecularType
{
    [MetaMember("mVal")]
    public T3NPRSpecularType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<T3NPRSpecularType>))]
    public enum T3NPRSpecularType
    {
        None = 0x0,
        Isotropic = 0x1,
        Anisotropic = 0x2
    }
}

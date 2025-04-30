using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3SamplerStateBlock>))]
public class T3SamplerStateBlock
{
    [MetaMember("mData")]
    public uint Data { get; set; }
}
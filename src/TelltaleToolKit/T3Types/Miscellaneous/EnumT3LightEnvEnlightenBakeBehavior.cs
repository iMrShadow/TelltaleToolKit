using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumT3LightEnvEnlightenBakeBehavior>))]
public struct EnumT3LightEnvEnlightenBakeBehavior
{
    [MetaMember("mVal")]
    public T3LightEnvEnlightenBakeBehavior Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3LightEnvEnlightenBakeBehavior>))]
public enum T3LightEnvEnlightenBakeBehavior
{
    // TODO:
}
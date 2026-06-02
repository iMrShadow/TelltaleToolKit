using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

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

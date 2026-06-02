using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterSpriteAnimationType>))]
public struct EnumEmitterSpriteAnimationType
{
    [MetaMember("mVal")]
    public EmitterSpriteAnimationType Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterSpriteAnimationType>))]
public enum EmitterSpriteAnimationType
{
    // TODO:
}

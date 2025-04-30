using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

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
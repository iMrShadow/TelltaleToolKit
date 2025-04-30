using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumEmitterSpriteAnimationSelection>))]
public struct EnumEmitterSpriteAnimationSelection
{
    [MetaMember("mVal")]
    public EmitterSpriteAnimationSelection Val { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<EmitterSpriteAnimationSelection>))]
public enum EmitterSpriteAnimationSelection
{
// TODO:
}
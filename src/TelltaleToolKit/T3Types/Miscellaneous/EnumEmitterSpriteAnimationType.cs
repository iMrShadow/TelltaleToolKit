using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterSpriteAnimationType>))]
public struct EnumEmitterSpriteAnimationType
{
    [MetaMember("mVal")]
    public EmitterSpriteAnimationType Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EmitterSpriteAnimationType>))]
    public enum EmitterSpriteAnimationType
    {
        Linear = 0x1,
        Random = 0x2
    }
}

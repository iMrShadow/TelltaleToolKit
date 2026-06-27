using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumEmitterSpriteAnimationSelection>))]
public struct EnumEmitterSpriteAnimationSelection
{
    [MetaMember("mVal")]
    public EmitterSpriteAnimationSelection Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EmitterSpriteAnimationSelection>))]
    public enum EmitterSpriteAnimationSelection
    {
        Random = 0x1,
        LinearLoop = 0x2,
        LinearStretch = 0x3,
        KeyControl01 = 0x4
    }
}

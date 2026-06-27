using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous.PreloadPackage;

[MetaSerializer(typeof(MetaClassSerializer<ResourceKey>))]
public class ResourceKey
{
    [MetaMember("mResourceName")]
    public Symbol ResourceName { get; set; } = Symbol.Empty;

    [MetaMember("mMetaClassDescriptionCrc")]
    public ulong MetaClassDescriptionCrc { get; set; }

    //NEWER GAMES
    [MetaMember("mRenderQualities")]
    public BitSet<RenderQualityType> RenderQualities { get; set; } = new(5);

    //EVEN NEWER GAMES
    [MetaMember("mVisible")]
    public bool Visible { get; set; }

    [MetaMember("mPrefix")]
    public string Prefix { get; set; } = string.Empty;
}

[MetaSerializer(typeof(EnumSerializer<RenderQualityType>))]
public enum RenderQualityType
{
    Default = -2,
    None = -1,
    High = 0,
    Mid = 1,
    LowPlus = 2,
    Mid2_Legacy = 2,
    Low = 3,
    Low_Legacy = 3,
    Lowest = 4,
    Count = 5
}

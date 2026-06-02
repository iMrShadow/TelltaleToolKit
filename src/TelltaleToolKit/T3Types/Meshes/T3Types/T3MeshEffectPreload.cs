using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MeshEffectPreload>))]
public class T3MeshEffectPreload
{
    [MetaMember("mEffectQuality")]
    public int EffectQuality { get; set; }

    [MetaMember("mEntries")]
    public List<T3MeshEffectPreloadEntry> Entries { get; set; } = [];

    [MetaMember("mTotalEffectCount")]
    public uint TotalEffectCount { get; set; }
}

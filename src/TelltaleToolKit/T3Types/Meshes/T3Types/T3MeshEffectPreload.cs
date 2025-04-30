using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshEffectPreload>))]
public class T3MeshEffectPreload
{
    [MetaMember("mEffectQuality")]
    public int EffectQuality { get; set; }

    [MetaMember("mEntries")]
    public List<T3MeshEffectPreloadEntry> Entries { get; set; } = [];

    [MetaMember("mTotalEffectCount")]
    public uint TotalEffectCount { get; set; }
}
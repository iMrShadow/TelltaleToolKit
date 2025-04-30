using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

/// <summary>
/// The base class for .acol files.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<AssetCollection>))]
public class AssetCollection
{
    [MetaMember("mIncludeMasks")]
    public List<string> IncludeMasks { get; set; } = [];

    [MetaMember("mExcludeMasks")]
    public List<string> ExcludeMasks { get; set; }= [];

    [MetaMember("mPreFilter")]
    public string PreFilter { get; set; } = string.Empty;
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

/// <summary>
///     The base class for .acol files.
/// </summary>
[MetaSerializer(typeof(MetaClassSerializer<AssetCollection>))]
public class AssetCollection
{
    [MetaMember("mIncludeMasks")]
    public List<string> IncludeMasks { get; set; } = [];

    [MetaMember("mExcludeMasks")]
    public List<string> ExcludeMasks { get; set; } = [];

    [MetaMember("mPreFilter")]
    public string PreFilter { get; set; } = string.Empty;
}

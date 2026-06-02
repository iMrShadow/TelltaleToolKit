using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MeshPropertyEntry>))]

public class T3MeshPropertyEntry
{
    [MetaMember("mIncludeFilter")]
    public string IncludeFilter { get; set; }

    [MetaMember("mExcludeFilter")]
    public string ExcludeFilter { get; set; }

    [MetaMember("mhProperties")]
    public Handle<PropertySet> Properties { get; set; }

    [MetaMember("mPriority")]
    public int Priority { get; set; }
}

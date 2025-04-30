using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshPropertyEntry>))]

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
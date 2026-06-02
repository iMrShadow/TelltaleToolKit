using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaSerializer(typeof(MetaClassSerializer<ProjectDatabaseIdPair>))]
public class ProjectDatabaseIdPair
{
    [MetaMember("mProjectID")]
    public uint ProjectId { get; set; }

    [MetaMember("mDBID")]
    public int DatabaseId { get; set; }
}

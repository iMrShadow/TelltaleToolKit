using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Languages.Landb;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ProjectDatabaseIdPair>))]
public class ProjectDatabaseIdPair
{
    [MetaMember("mProjectID")]
    public uint ProjectId { get; set; }
    
    [MetaMember("mDBID")]
    public int DatabaseId { get; set; }
}
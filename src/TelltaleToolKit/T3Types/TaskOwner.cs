using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<TaskOwner>))]
public class TaskOwner
{
    [MetaMember("mTaskID")]
    public uint TaskId { get; set; }
}

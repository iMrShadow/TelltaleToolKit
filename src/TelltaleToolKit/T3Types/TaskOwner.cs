using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<TaskOwner>))]
public class TaskOwner
{
    [MetaMember("mTaskID")]
    public uint TaskId { get; set; }
}
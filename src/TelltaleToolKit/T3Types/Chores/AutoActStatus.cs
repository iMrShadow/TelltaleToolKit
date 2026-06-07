using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<AutoActStatus>))]
public class AutoActStatus
{
    [MetaMember("m_Status")]
    public int Status { get; set; }
}

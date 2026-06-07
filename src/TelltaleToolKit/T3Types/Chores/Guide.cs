using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<Guide>))]
public class Guide
{
    [MetaMember("m_Time")]
    public float Time { get; set; }

    [MetaMember("m_Bitfield")]
    public int Bitfield { get; set; }

    [MetaMember("m_AutoActRole")]
    public int AutoActRole { get; set; }
}

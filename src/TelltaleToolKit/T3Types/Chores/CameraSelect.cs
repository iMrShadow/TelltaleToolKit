using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Chores;

[MetaSerializer(typeof(MetaClassSerializer<CameraSelect>))]
public class CameraSelect
{
    [MetaMember("mCameraNames")]
    public List<Symbol> CameraNames { get; set; } = [];
}

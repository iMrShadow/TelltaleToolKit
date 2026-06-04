using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<CameraFacingTypes>))]
public class CameraFacingTypes
{
    [MetaSerializer(typeof(EnumSerializer<FacingTypes>))]
    public enum FacingTypes
    {
        Facing = 0,
        FacingY = 1,
        FacingLocalY = 2
    }

    [MetaMember("mCameraFacingType")]
    public FacingTypes CameraFacingType { get; set; }
}

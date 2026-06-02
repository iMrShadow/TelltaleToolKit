using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<CameraFacingTypes>))]
public class CameraFacingTypes
{
    [MetaMember("mCameraFacingType")]
    public FacingTypes CameraFacingType { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<FacingTypes>))]
public enum FacingTypes
{
    Facing = 0,
    FacingY = 1,
    FacingLocalY = 2,
}

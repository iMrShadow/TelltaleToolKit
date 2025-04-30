using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Skeletons;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshLocalTransformEntry>))]
public class T3MeshLocalTransformEntry
{
    [MetaMember("mTransform")]
    public Transform Transform { get; set; }

    [MetaMember("mCameraFacingType")]
    public T3CameraFacingType CameraFacingType { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3CameraFacingType>))]
public enum T3CameraFacingType {
    // eCameraFacing_
    None = 0,
    Enable = 1,
    EnableY = 2,
    EnableLocalY = 3
}
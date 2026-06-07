using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaSerializer(typeof(EnumSerializer<T3SurfaceMultisample>))]
public enum T3SurfaceMultisample
{
    // eSurfaceMultisample
    None = 0,
    X2 = 1, // X2
    X4 = 2, // X4
    X8 = 3, // X8
    X16 = 4 // X16
}

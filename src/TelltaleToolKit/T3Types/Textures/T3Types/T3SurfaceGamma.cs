using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

/// <summary>
///     The gamma of the surface. In Telltale Tool it is represented as eSurfaceGamma.
/// </summary>
[MetaSerializer(typeof(EnumSerializer<T3SurfaceGamma>))]
public enum T3SurfaceGamma
{
    Unknown = -1,
    Linear = 0,
    sRGB = 1
}

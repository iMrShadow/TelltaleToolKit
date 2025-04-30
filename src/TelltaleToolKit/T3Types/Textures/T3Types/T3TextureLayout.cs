using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaClassSerializerGlobal(typeof(EnumSerializer<T3TextureLayout>))]

public enum T3TextureLayout
{
    Unknown = -1,
    Texture2D = 0,
    TextureCubemap = 1,
    Texture3D = 2,
    Texture2DArray = 3,
    TextureCubemapArray = 4,
    TextureCount = 5,
}

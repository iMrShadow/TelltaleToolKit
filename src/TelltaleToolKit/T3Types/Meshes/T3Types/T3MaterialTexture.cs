using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Textures.T3Types;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaSerializer(typeof(MetaClassSerializer<T3MaterialTexture>))]
public class T3MaterialTexture
{
    [MetaMember("mName")]
    public Symbol Name { get; set; }

    [MetaMember("mTextureName")]
    public Symbol TextureName { get; set; }

    [MetaMember("mTextureNameWithoutExtension")]
    public Symbol TextureNameWithoutExtension { get; set; }

    [MetaMember("mLayout")]
    public T3TextureLayout Layout { get; set; }

    [MetaMember("mPropertyType")]
    public T3MaterialPropertyType PropertyType { get; set; }

    [MetaMember("mTextureTypes")]
    public BitSetBase TextureTypes { get; set; }

    [MetaMember("mFirstParamIndex")]
    public uint FirstParamIndex { get; set; }

    [MetaMember("mParamCount")]
    public uint ParamCount { get; set; }

    [MetaMember("mTextureIndex")]
    public int TextureIndex { get; set; }

    [MetaMember("mNestedMaterialIndex")]
    public int NestedMaterialIndex { get; set; }
}

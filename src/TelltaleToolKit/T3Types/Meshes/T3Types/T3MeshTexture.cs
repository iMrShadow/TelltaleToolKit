using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Textures;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshTexture>))]
public class T3MeshTexture
{
    [MetaMember("mTextureType")]
    public T3MeshTextureType TextureType { get; set; }

    [MetaMember("mhTexture")]
    public Handle<T3Texture> Texture { get; set; }

    [MetaMember("mNameSymbol")]
    public Symbol NameSymbol { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mMaxObjAreaPerUVArea")]
    public float MaxObjAreaPerUVArea { get; set; }

    [MetaMember("mAverageObjAreaPerUVArea")]
    public float AverageObjAreaPerUVArea { get; set; }
}

public enum T3MeshTextureType
{
    //    eMeshTexture_
    None = -1,
    LightMap = 0,
    ShadowMap = 1,
    Count = 2,
}
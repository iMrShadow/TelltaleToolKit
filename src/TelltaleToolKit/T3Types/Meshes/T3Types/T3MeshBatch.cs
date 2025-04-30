using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshBatch>))]
public class T3MeshBatch
{
    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mBatchUsage")]
    public Flags BatchUsage { get; set; }

    [MetaMember("mMinVertIndex")]
    public uint MinVertIndex { get; set; }

    [MetaMember("mMaxVertIndex")]
    public uint MaxVertIndex { get; set; }

    [MetaMember("mBaseIndex")]
    public uint BaseIndex { get; set; }

    [MetaMember("mStartIndex")]
    public uint StartIndex { get; set; }

    [MetaMember("mNumPrimitives")]
    public uint NumPrimitives { get; set; }

    [MetaMember("mNumIndices")]
    public uint NumIndices { get; set; }

    [MetaMember("mTextureIndices")]
    public T3MeshTextureIndices TextureIndices { get; set; }

    [MetaMember("mMaterialIndex")]
    public int MaterialIndex { get; set; }

    [MetaMember("mAdjacencyStartIndex")]
    public uint AdjacencyStartIndex { get; set; }

    [MetaMember("mLocalTransformIndex")]
    public uint LocalTransformIndex { get; set; }

    [MetaMember("mBonePaletteIndex")]
    public uint BonePaletteIndex { get; set; }
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<T3MeshLOD>))]
public class T3MeshLOD
{
    // Michonne for e.g. uses a DCArray of batches. In new games they only use 2.
    [MetaMember("mBatches")]
    public List<T3MeshBatch> Batches { get; set; } = [];// Default and Shadow

    [MetaMember("mBatches[0]")]
    public  List<T3MeshBatch> Batches1 { get; set; }  = [];// Default 
    
    [MetaMember("mBatches[1]")]
    public  List<T3MeshBatch> Batches2 { get; set; } = []; //  Shadow

    [MetaMember("mVertexStreams")]
    public BitSetBase VertexStreams { get; set; }

    [MetaMember("mBoundingBox")]
    public BoundingBox BoundingBox { get; set; }

    [MetaMember("mBoundingSphere")]
    public Sphere BoundingSphere { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mVertexStateIndex")]
    public uint VertexStateIndex { get; set; }

    [MetaMember("mNumPrimitives")]
    public uint NumPrimitives { get; set; }

    [MetaMember("mNumBatches")]
    public uint NumBatches { get; set; }

    [MetaMember("mVertexStart")]
    public uint VertexStart { get; set; }

    [MetaMember("mVertexCount")]
    public uint VertexCount { get; set; }

    [MetaMember("mTextureAtlasWidth")]
    public uint TextureAtlasWidth { get; set; }

    [MetaMember("mTextureAtlasHeight")]
    public uint TextureAtlasHeight { get; set; }

    [MetaMember("mPixelSize")]
    public float PixelSize { get; set; }

    [MetaMember("mDistance")]
    public float Distance { get; set; }

    [MetaMember("mBones")]
    public List<Symbol> Bones { get; set; } = [];
}
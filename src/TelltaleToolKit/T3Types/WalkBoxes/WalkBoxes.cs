using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.WalkBoxes;

/// <summary>
/// Represents the class for .wbox files.
/// </summary>
[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<WalkBoxes>))]
public class WalkBoxes
{
    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mTris")]
    public List<Tri> Tris { get; set; } = [];

    [MetaMember("mVerts")]
    public List<Vert> Verts { get; set; } = [];

    [MetaMember("mNormals")]
    public List<Vector3> Normals { get; set; } = [];

    [MetaMember("mQuads")]
    public List<Quad> Quads { get; set; } = [];

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Edge>))]
    public class Edge
    {
        [MetaMember("mV1")]
        public int V1 { get; set; }

        [MetaMember("mV2")]
        public int V2 { get; set; }

        [MetaMember("mEdgeDest")]
        public int EdgeDest { get; set; }

        [MetaMember("mEdgeDestEdge")]
        public int EdgeDestEdge { get; set; }

        [MetaMember("mEdgeDir")]
        public int EdgeDir { get; set; }

        [MetaMember("mMaxRadius")]
        public float MaxRadius { get; set; }
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Tri>))]
    public class Tri
    {
        [MetaMember("mFlags")]
        public Flags Flags { get; set; } = new();

        [MetaMember("mNormal")]
        public int Normal { get; set; }

        [MetaMember("mQuadBuddy")]
        public int QuadBuddy { get; set; }

        [MetaMember("mMaxRadius")]
        public float MaxRadius { get; set; }

        [MetaMember("mVerts")]
        public int[] Verts { get; set; } = new int[3];

        [MetaMember("mEdgeInfo")]
        public Edge[] EdgeInfo { get; set; } = new Edge[3];

        [MetaMember("mVertOffsets")]
        public int[] VertOffsets { get; set; } = new int[3];

        [MetaMember("mVertScales")]
        public float[] VertScales { get; set; } = new float[3];
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Vert>))]
    public struct Vert
    {
        [MetaMember("mFlags")]
        public Flags Flags { get; set; } 

        [MetaMember("mPos")]
        public Vector3 Position { get; set; } 
    }

    [MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Quad>))]
    public class Quad
    {
        [MetaMember("mVerts")]
        public int[] Verts { get; set; } = new int[4];
    }
}
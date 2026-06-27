using System.Numerics;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Audio;

namespace TelltaleToolKit.T3Types.WalkBoxes;

/// <summary>
/// Represents the class for .wbox (Walkbox) files.
/// </summary>
[MetaSerializer(typeof(MetaClassSerializer<WalkBoxes>))]
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

    [MetaSerializer(typeof(MetaClassSerializer<Edge>))]
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

    [MetaSerializer(typeof(MetaClassSerializer<Tri>))]
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

        [MetaMember("mFootstepMaterial")]
        public SoundFootsteps.EnumMaterial FootstepMaterial { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<Vert>))]
    public struct Vert
    {
        [MetaMember("mFlags")]
        public Flags Flags { get; set; }

        [MetaMember("mPos")]
        public Vector3 Position { get; set; }
    }

    [MetaSerializer(typeof(MetaClassSerializer<Quad>))]
    public class Quad
    {
        [MetaMember("mVerts")]
        public int[] Verts { get; set; } = new int[4];
    }
}

using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

// Right, so D3DVertexBuffer changes to T3VertexBuffer (probably because the engine became cross-platform at the time).
// Since most values are the same, I merged D3DVertexBuffer with T3VertexBuffer.

/// <summary>
/// Old name - D3DVertexBuffer
/// </summary>
[MetaSerializer(typeof(Serializer))]
public class T3VertexBuffer
{
    [MetaMember("mNumVerts")]
    public int NumVerts { get; set; }

    [MetaMember("mVertFormat")]
    public uint VertFormat { get; set; }

    [MetaMember("mbLocked")]
    public bool Locked { get; set; }

    [MetaMember("mVertSize")]
    public int VertSize { get; set; }

    [MetaMember("mType")]
    public int Type { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mUsage")]
    public int Usage { get; set; }

    [MetaMember("mVertexComponents")]
    public T3VertexComponent[] VertexComponents { get; set; } = new T3VertexComponent[14];

    [MetaMember("mComponents")]
    public T3VertexComponent[] Components { get; set; } = new T3VertexComponent[12];

    public float[] Positions { get; private set; } // length = NumVerts * 3
    public float[] Normals { get; private set; } // length = NumVerts * 3
    public float[] UVs { get; private set; } // length = NumVerts * 2 (or 3? adjust)
    public float[] Weights { get; private set; } // length = NumVerts * 3 (or 4? adjust)

    [MetaMember("mbStoreCompressed")]
    public bool StoreCompressed { get; set; }

    public float[] FBuffer = [];
    public byte[] Buffer = [];

    public class Serializer : MetaSerializer<T3VertexBuffer>
    {
        private static readonly MetaClassSerializer<T3VertexBuffer> s_metaClassSerializer = new();

        public override void PreSerialize(ref T3VertexBuffer? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new T3VertexBuffer();
        }

        public override void Serialize(ref T3VertexBuffer obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                if (obj.Buffer.Length == obj.NumVerts * obj.VertSize)
                {
                    obj.StoreCompressed = false;
                }
                else if (obj.Buffer.Length == 2 * obj.NumVerts && obj.Type is 2 or 4)
                {
                    obj.StoreCompressed = true;
                }
            }

            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Buffer);
            }
            else
            {
                if (obj.Flags.Has(1))
                {
                    return;
                }

                int totalBytes = obj.StoreCompressed switch
                {
                    true when obj.Type is 2 or 4 => 2 * obj.NumVerts,
                    _ => obj.NumVerts * obj.VertSize
                };

                obj.Buffer = stream.ReadBytes(totalBytes);
            }
        }
    }

    public void DecompressPositions(MetaStream stream)
    {
        Positions = new float[NumVerts * 3];
        uint compressedSize = stream.ReadUInt32();
        byte[] compressedData = stream.ReadBytes((int)compressedSize);

        var bitBuffer = new BitReader(compressedData);

        // ---- Header ----
        int v92 = bitBuffer.ReadBits(4); // skip bits when flag==1
        int v93 = bitBuffer.ReadBits(3); // bits for v94
        int v94 = bitBuffer.ReadBits(3); // bits for v95
        int v95 = bitBuffer.ReadBits(3); // bits for v96
        int v96 = bitBuffer.ReadBits(4); // bits for v48 (run length)

        // Two floats are read but discarded in the original code.
        // They may be scale/offset – adapt if needed.
        float Float = bitBuffer.ReadFloat();
        float v104 = bitBuffer.ReadFloat();

        float x = 0f, y = 0f, z = 0f; // accumulated position
        int vertexIndex = 0;

        while (vertexIndex < NumVerts)
        {
            // Per‑vertex flag
            bool flag = bitBuffer.ReadBit();
            float v30;
            if (flag)
            {
                uint v29 = (uint)bitBuffer.ReadBits(v92);
                uint maxVal = (1u << v92) - 1;
                if (maxVal == 0) v30 = Float;
                else
                {
                    v30 = Float + (v104 - Float) * ((float)v29 / maxVal);
                    v30 = Math.Clamp(v30, Float, v104);
                }
            }
            else
            {
                v30 = bitBuffer.ReadFloat();
            }

            // Read bit‑widths for the delta components
            int v109 = bitBuffer.ReadBits(v93);
            int v110 = bitBuffer.ReadBits(v94);
            int v111 = bitBuffer.ReadBits(v95);

            // Number of repeated deltas
            int v59 = bitBuffer.ReadBits(v96);

            for (int j = 0; j < v59; j++)
            {
                int flag2 = bitBuffer.ReadBits(1);
                if (flag2 == 0)
                {
                    // Read raw delta values (unsigned)
                    uint v106 = (uint)bitBuffer.ReadBits(v109);
                    uint v107 = (uint)bitBuffer.ReadBits(v110);
                    uint v108 = (uint)bitBuffer.ReadBits(v111);

                    float dx = 0f, dy = 0f, dz = 0f;
                    if (v109 > 0)
                    {
                        float maxVal = (1u << v109) - 1;
                        dx = (v106 / maxVal) * (2f * v30) - v30;
                    }

                    if (v110 > 0)
                    {
                        float maxVal = (1u << v110) - 1;
                        dy = (v107 / maxVal) * (2f * v30) - v30;
                    }

                    if (v111 > 0)
                    {
                        float maxVal = (1u << v111) - 1;
                        dz = (v108 / maxVal) * (2f * v30) - v30;
                    }

                    x += dx;
                    y += dy;
                    z += dz;

                    // float deltaX = DecompressDelta(rawDx, v94, 1.0f);
                    // float deltaY = DecompressDelta(rawDy, v95, 1.0f);
                    // float deltaZ = DecompressDelta(rawDz, v96, 1.0f);
                }

                if (vertexIndex >= NumVerts) break;
                int baseIdx = vertexIndex * 3;
                Positions[baseIdx + 0] = x;
                Positions[baseIdx + 1] = y;
                Positions[baseIdx + 2] = z;
                vertexIndex++;
                // flag2 == 1 : no delta (position unchanged)
            }
        }
    }

    /// <summary>
    /// Decompresses normals from the stream into a pre‑allocated float array.
    /// </summary>
    /// <param name="stream">The input stream containing compressed normal data.</param>
    /// <param name="normals">Output array (interleaved x,y,z) of length vertexCount * 3.</param>
    /// <param name="strideInBytes">Bytes between consecutive vertices in the output buffer.</param>
    /// <param name="vertexCount">Number of vertices to decompress.</param>
    public void DecompressNormals(MetaStream stream)
    {
        if (NumVerts == 0) return;

        Normals = new float[NumVerts * 3];

        // ---- Read header ----
        int w1 = stream.ReadByte(); // v75[0] (unused flag width)
        int w2 = stream.ReadByte(); // v74 – bit width for phi‑delta width
        int w3 = stream.ReadByte(); // v73 – bit width for theta‑delta width
        int w4 = stream.ReadByte(); // v72 – bit width for run length

        // The next four floats: v71, v70, v67, v68.
        // v67 and v68 are the initial spherical angles (phi, theta).
        // v71 and v70 are scaling factors used elsewhere (currently ignored).
        float _scale1 = stream.ReadSingle(); // v71
        float _scale2 = stream.ReadSingle(); // v70
        float initPhi = stream.ReadSingle(); // v67
        float initTheta = stream.ReadSingle(); // v68

        // ---- Decompress the entire bit stream ----
        // In the original code the compressed payload follows the header.
        // We assume the stream position is already at the start of the bit data.
        // The compressed size is not read here; we read until we've produced vertexCount normals.
        // You might need to read the payload size first (like in PositionDecompress).
        uint compressedSize = stream.ReadUInt32();
        byte[] compressedData = stream.ReadBytes((int)compressedSize);
        //byte[] compressedData = stream.ReadBytes( /* payload length */); // adjust as needed
        var bitBuffer = new BitReader(compressedData);

        // ---- Process vertex 0 ----
        float currentPhi = initPhi;
        float currentTheta = initTheta;
        float lastPhi = initPhi;
        float lastTheta = initTheta;

        SphericalToNormal(initPhi, initTheta, out float x, out float y, out float z);
        int strideFloats = VertSize / 4;
        Normals[0] = x;
        Normals[1] = y;
        Normals[2] = z;

        int vertexIndex = 1; // we have already written vertex 0

        // ---- Process runs ----
        while (vertexIndex < NumVerts)
        {
            // Read flag1
            int flag1 = bitBuffer.ReadBits(1);
            if (flag1 == 1)
            {
                // Read w1 bits and discard (DecompressBounds)
                bitBuffer.ReadBits(w1);
            }
            else
            {
                // Read a float and discard
                bitBuffer.ReadFloat();
            }

            // Read bit widths for this run
            int phiBitWidth = bitBuffer.ReadBits(w2); // v51
            int thetaBitWidth = bitBuffer.ReadBits(w3); // v21
            int runLength = bitBuffer.ReadBits(w4); // v25

            float deltaScale = 1.0f;
            // Process the run
            for (int j = 0; j < runLength && vertexIndex < NumVerts; j++)
            {
                int flag2 = bitBuffer.ReadBits(1); // v54

                // Read raw deltas (unsigned)
                int rawPhi = bitBuffer.ReadBits(phiBitWidth);
                int rawTheta = bitBuffer.ReadBits(thetaBitWidth);

                // Convert to floating‑point deltas.
                // The original code uses scaling factors; we use 1.0 as default.
                // If your format uses scaling, multiply by the appropriate factors.
                float phiDelta = DecompressDelta(rawPhi, phiBitWidth, deltaScale);
                float thetaDelta = DecompressDelta(rawTheta, thetaBitWidth, deltaScale);

                // Update spherical coordinates
                if (flag2 == 1)
                {
                    // Accumulate onto the current run's base
                    currentPhi += phiDelta;
                    currentTheta += thetaDelta;
                }
                else
                {
                    // Use the previous vertex's angles as base
                    currentPhi = lastPhi + phiDelta;
                    currentTheta = lastTheta + thetaDelta;
                }

                // Convert to unit normal
                SphericalToNormal(currentPhi, currentTheta, out x, out y, out z);

                // Write to output
                int baseOffset = vertexIndex * strideFloats;
                Normals[baseOffset] = x;
                Normals[baseOffset + 1] = y;
                Normals[baseOffset + 2] = z;

                // Update "last" for the next vertex
                lastPhi = currentPhi;
                lastTheta = currentTheta;

                vertexIndex++;
            }
        }
    }

    public void DecompressWeights(MetaStream stream)
    {
        if (NumVerts == 0) return;
        Weights = new float[NumVerts * 3];

        // ---- Header ----
        int w1 = stream.ReadByte(); // unused flag width
        int w2 = stream.ReadByte(); // bit width for delta1‑width
        int w3 = stream.ReadByte(); // bit width for delta2‑width
        int w4 = stream.ReadByte(); // bit width for run length

        float _scale1 = stream.ReadSingle(); // v74 (ignored)
        float _scale2 = stream.ReadSingle(); // v73 (ignored)
        float initW0 = stream.ReadSingle(); // v67
        float initW1 = stream.ReadSingle(); // v68

        float initW2 = 1.0f - initW0 - initW1; // v69

        // ---- Payload ----
        uint payloadSize = stream.ReadUInt32();
        byte[] compressedData = stream.ReadBytes((int)payloadSize);
        var bitBuffer = new BitReader(compressedData);

        // ---- Vertex 0 ----
        int strideFloats = VertSize / 4;
        Weights[0] = initW0;
        Weights[1] = initW1;
        Weights[2] = initW2;

        // ---- Loop ----
        float currentW0 = initW0;
        float currentW1 = initW1;
        float currentW2 = initW2;
        float lastW0 = initW0;
        float lastW1 = initW1;
        float lastW2 = initW2;

        int vertexIndex = 1;

        while (vertexIndex < NumVerts)
        {
            int flag1 = bitBuffer.ReadBits(1);
            if (flag1 == 1)
                bitBuffer.SkipBits(w1);
            else
                bitBuffer.ReadFloat();

            int delta0BitWidth = bitBuffer.ReadBits(w2); // v54
            int delta1BitWidth = bitBuffer.ReadBits(w3); // v23
            int runLength = bitBuffer.ReadBits(w4); // v27

            for (int j = 0; j < runLength && vertexIndex < NumVerts; j++)
            {
                int flag2 = bitBuffer.ReadBits(1); // v54 (second flag)

                int rawDelta0 = bitBuffer.ReadBits(delta0BitWidth);
                int rawDelta1 = bitBuffer.ReadBits(delta1BitWidth);

                float delta0 = DecompressDelta(rawDelta0, delta0BitWidth, 1.0f);
                float delta1 = DecompressDelta(rawDelta1, delta1BitWidth, 1.0f);

                if (flag2 == 1)
                {
                    currentW0 += delta0;
                    currentW1 += delta1;
                }
                else
                {
                    currentW0 = lastW0 + delta0;
                    currentW1 = lastW1 + delta1;
                }

                currentW2 = 1.0f - currentW0 - currentW1;

                int baseOffset = vertexIndex * strideFloats;
                Weights[baseOffset] = currentW0;
                Weights[baseOffset + 1] = currentW1;
                Weights[baseOffset + 2] = currentW2;

                lastW0 = currentW0;
                lastW1 = currentW1;
                lastW2 = currentW2;
                vertexIndex++;
            }
        }
    }

    public void DecompressUV(MetaStream stream)
    {
        if (NumVerts == 0) return;
        UVs = new float[NumVerts * 2];

        // ---- Read header ----
        int w1 = stream.ReadByte(); // unused flag width (v80[0])
        int w2 = stream.ReadByte(); // bit width for U‑delta width (v79)
        int w3 = stream.ReadByte(); // bit width for V‑delta width (v78)
        int w4 = stream.ReadByte(); // bit width for run length (v77)

        float _scaleU = stream.ReadSingle(); // v76 (ignored here)
        float _scaleV = stream.ReadSingle(); // v75 (ignored)
        float initU = stream.ReadSingle(); // v72
        float initV = stream.ReadSingle(); // v73

        // ---- Read compressed payload ----
        uint payloadSize = stream.ReadUInt32();
        byte[] compressedData = stream.ReadBytes((int)payloadSize);
        var bitBuffer = new BitReader(compressedData);

        // ---- Vertex 0 ----
        int strideFloats = VertSize / 4; // number of floats per UV pair
        UVs[0] = initU;
        UVs[1] = initV;

        // ---- Loop over remaining vertices ----
        float currentU = initU;
        float currentV = initV;

        int vertexIndex = 1;

        while (vertexIndex < NumVerts)
        {
            // Run header
            int flag1 = bitBuffer.ReadBits(1);
            if (flag1 == 1)
                bitBuffer.SkipBits(w1); // discard DecompressBounds data
            else
                bitBuffer.ReadFloat(); // discard full‑precision float

            // Read bit widths for this run (as integers)
            int uBitWidth = bitBuffer.ReadBits(w2); // v63
            int vBitWidth = bitBuffer.ReadBits(w3); // v64
            int runLength = bitBuffer.ReadBits(w4); // v35

            // Process the run
            for (int j = 0; j < runLength && vertexIndex < NumVerts; j++)
            {
                int rawU = bitBuffer.ReadBits(uBitWidth);
                int rawV = bitBuffer.ReadBits(vBitWidth);
                // TODO: Delta Scales (wtf)
                float deltaU = DecompressDelta(rawU, uBitWidth, 1.0f);
                float deltaV = DecompressDelta(rawV, vBitWidth, 1.0f);

                currentU += deltaU;
                currentV += deltaV;

                int baseOffset = vertexIndex * strideFloats;
                UVs[baseOffset] = currentU;
                UVs[baseOffset + 1] = currentV;

                vertexIndex++;
            }
        }
    }

    /// <summary>
    /// Converts spherical coordinates (phi, theta) to a unit normal.
    /// phi: azimuth angle in radians (around Y axis? adapt to your convention).
    /// theta: polar angle from Z axis (0..PI).
    /// </summary>
    private static void SphericalToNormal(float phi, float theta, out float x, out float y, out float z)
    {
        float sinTheta = (float)Math.Sin(theta);
        float cosTheta = (float)Math.Cos(theta);
        float sinPhi = (float)Math.Sin(phi);
        float cosPhi = (float)Math.Cos(phi);

        // Typical right‑handed system: X = sin(theta)*cos(phi), Y = sin(theta)*sin(phi), Z = cos(theta)
        // Adjust if your engine uses a different convention.
        x = sinTheta * cosPhi;
        y = sinTheta * sinPhi;
        z = cosTheta;
    }

    /// <summary>
    /// Converts a raw unsigned integer delta to a float in the range [-scale, scale].
    /// </summary>
    /// <param name="rawValue">The unsigned value extracted from the bitstream (only lower 'bitWidth' bits matter).</param>
    /// <param name="bitWidth">Number of bits used for this delta.</param>
    /// <param name="scale">Maximum magnitude of the delta.</param>
    /// <returns>Scaled delta as a float.</returns>
    public static float DecompressDelta(int rawValue, int bitWidth, float scale)
    {
        if (bitWidth <= 0 || scale == 0.0f)
            return 0.0f;

        int mask = (1 << bitWidth) - 1;
        int val = rawValue & mask; // ensure we only use the valid bits

        // Map val from [0, mask] to [-scale, scale]:
        //   delta = (val / mask) * 2 * scale - scale
        float normalized = (float)val / mask;
        return normalized * (2.0f * scale) - scale;
    }
}

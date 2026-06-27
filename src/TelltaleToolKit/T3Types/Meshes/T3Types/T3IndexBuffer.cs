using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Meshes.T3Types;

/// <summary>
/// Old name - D3DIndexBuffer
/// </summary>
[MetaSerializer(typeof(T3IndexBufferSerializer))]
public class T3IndexBuffer
{
    [MetaMember("mbLocked")]
    public bool Locked { get; set; }

    [MetaMember("mFormat")]
    public int Format { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mUsage")]
    public int Usage { get; set; }

    [MetaMember("mNumIndicies")]
    public int NumIndices { get; set; }

    [MetaMember("mIndexByteSize")]
    public int IndexByteSize { get; set; }

    public byte[] Buffer { get; set; } = [];

    [MetaSerializer(typeof(T3IndexBufferSerializer))]
    public class T3IndexBufferSerializer : MetaSerializer<T3IndexBuffer>
    {
        private static readonly MetaClassSerializer<T3IndexBuffer> s_metaClassSerializer = new();

        public override void Serialize(ref T3IndexBuffer obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                int bufferBytes = obj.Format switch
                {
                    101 => 2,
                    102 => 4,
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (obj.IndexByteSize == 0)
                {
                    obj.IndexByteSize = bufferBytes;
                }

                if (obj.Flags.Has(1))
                {
                    // 1. Read the base value (first index)
                    ushort baseValue = stream.ReadUInt16();
                    uint[] indices = new uint[obj.NumIndices];
                    indices[0] = baseValue;

                    // 2. Read the compressed data size and raw bytes
                    uint compressedSize = stream.ReadUInt32();
                    byte[] compressedData = stream.ReadBytes((int)compressedSize);

                    // 3. Bit reader over the compressed bytes
                    var bitReader = new BitReader(compressedData);

                    int idx = 1; // we already filled index 0

                    // 4. Decompress each run of deltas
                    while (idx < obj.NumIndices)
                    {
                        int numBits = bitReader.ReadBits(4); // magnitude bit width (0..15)
                        int runLength = bitReader.ReadBits(7); // number of deltas in this run (0..127)

                        for (int i = 0; i < runLength && idx < obj.NumIndices; i++)
                        {
                            int sign = bitReader.ReadBits(1);
                            int magnitude = bitReader.ReadBits(numBits);
                            int delta = sign == 1 ? -magnitude : magnitude;

                            baseValue = (ushort)(baseValue + delta);
                            indices[idx++] = baseValue;
                        }
                    }
                }
                else
                {
                    obj.Buffer = stream.ReadBytes(bufferBytes * obj.NumIndices);
                }
            }
        }
    }
}

public class BitReader
{
    private readonly byte[] _data;
    private int _bitPos;

    public BitReader(byte[] data)
    {
        _data = data;
        _bitPos = 0;
    }

    public int ReadBits(int count)
    {
        if (count == 0) return 0;

        int value = 0;
        for (int i = 0; i < count; i++)
        {
            int byteIndex = _bitPos / 8;
            int bitOffset = _bitPos % 8;
            int bit = (_data[byteIndex] >> bitOffset) & 1;
            value |= bit << i; // first bit read becomes LSB
            _bitPos++;
        }

        return value;
    }

    public bool ReadBit() => ReadBits(1) != 0;

    public float ReadFloat()
    {
        int bits = ReadBits(32);
        return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
    }

    /// <summary>Advances the bit pointer without reading.</summary>
    public void SkipBits(int numBits)
    {
        if (numBits < 0) throw new ArgumentOutOfRangeException(nameof(numBits));
        _bitPos += numBits;
    }
}

[MetaSerializer(typeof(MetaClassSerializer<T3VertexComponent>))]
public class T3VertexComponent
{
    [MetaMember("mOffset")]
    public uint Offset { get; set; }

    [MetaMember("mCount")]
    public uint Count { get; set; }

    [MetaMember("mType")]
    public EnumType Type { get; set; }

    [MetaSerializer(typeof(EnumSerializer<EnumType>))]
    public enum EnumType
    {
        VTypeNone = 0,
        VTypeFloat = 1,
        VTypeS8N = 2, //N meaning normalised between -127 and 127 so when read cast to float and divide by 127
        VTypeU8N = 3,
        VTypeS16N = 4,
        VTypeU16N = 5,

        //../
        VTypeS8NBones = 8,
        VTypeSF16 = 11, //signed half float
    }
}

// Right, so D3DIndexBuffer changes to T3IndexBuffer (probably because the engine became cross-platform at the time).
// Since most values are the same, I merged D3DIndexBuffer with T3IndexBuffer.

// [DataSerializerGlobal(typeof(D3DIndexBufferSerializer))]
// public class D3DIndexBuffer
// {
//     [MetaMember("mbLocked")] public bool Locked { get; set; }
//     [MetaMember("mFormat")] public int Format { get; set; }
//     [MetaMember("mNumIndicies")] public int NumIndices { get; set; }
//     [MetaMember("mIndexByteSize")] public int IndexByteSize { get; set; }
//     public byte[] IndexBufferData = [];
//
//     [DataSerializerGlobal(typeof(D3DIndexBufferSerializer))]
//     public class D3DIndexBufferSerializer : MetaSerializer<D3DIndexBuffer>
//     {
//         public override void Serialize(ref D3DIndexBuffer obj, MetaStream stream, MetaClass desc)
//         {
//             new MetaClassSerializer<D3DIndexBuffer>().Serialize(ref obj, stream, desc);
//
//             if (stream.Mode is MetaStreamMode.Write)
//             {
//             }
//             else if (stream.Mode is MetaStreamMode.Read)
//             {
//                 var bufferBytes = 2;
//                 if (obj.Format != 101)
//                 {
//                     bufferBytes = 4;
//                 }
//
//                 obj.IndexBufferData = stream.ReadBytes(obj.IndexByteSize * obj.NumIndices);
//             }
//         }
//     }
// }

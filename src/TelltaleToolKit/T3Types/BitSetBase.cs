using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(Serializer))]
public class BitSetBase
{
    public BitSetBase(int numValues) => Values = new uint[numValues];

    public uint[] Values { get; set; }

    public class Serializer : MetaSerializer<BitSetBase>
    {
        public override void PreSerialize(ref BitSetBase? obj, MetaStream stream, MetaClassType? type = null)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type), "BitSetBase requires a metaclass type!");
            }

            int bitSetSize = type.Symbol.DebugString switch
            {
                "BitSetBase<1>" => 1,
                "BitSetBase<2>" => 2,
                "BitSetBase<3>" => 3,
                "BitSetBase<4>" => 4,
                "BitSetBase<5>" => 5,
                "BitSetBase<6>" => 6,
                "BitSetBase<7>" => 7,
                "BitSetBase<8>" => 8,
                "BitSetBase<9>" => 9,
                _ => throw new InvalidDataException($"Unexpected BitSetBase type: {type.Symbol.DebugString}")
            };

            obj ??= new BitSetBase(bitSetSize);

            if (obj.Values.Length != bitSetSize)
            {
                BitSetBase newBitSet = new(bitSetSize);

                int copyLength = Math.Min(obj.Values.Length, bitSetSize);
                Array.Copy(obj.Values, 0, newBitSet.Values, 0, copyLength);

                obj = newBitSet;
            }
        }

        public override void Serialize(ref BitSetBase obj, MetaStream stream, MetaClassType? type = null)
        {
            for (int i = 0; i < obj.Values.Length; i++)
            {
                stream.Serialize(ref obj.Values[i]);
            }
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class BitSetBase
{
    public uint[] Values { get; set; }

    public BitSetBase(int numValues)
    {
        Values = new uint[numValues];
    }

    public class Serializer : MetaClassSerializer<BitSetBase>
    {
        public override void PreSerialize(ref BitSetBase obj, MetaStream stream, MetaClassType? type = null)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type), "BitSetBase requires a metaclass type!");
            }

            int? bitSetSize = type.Symbol.DebugString switch
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

            if (obj?.Values == null || obj.Values.Length != bitSetSize.Value)
            {
                obj = new BitSetBase(bitSetSize.Value);
            }
        }

        public override void Serialize(ref BitSetBase obj, MetaStream stream)
        {
            for (var i = 0; i < obj.Values.Length; i++)
            {
                stream.Serialize(ref obj.Values[i]);
            }
        }
    }
}

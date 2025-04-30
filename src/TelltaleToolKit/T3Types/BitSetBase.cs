using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class BitSetBase
{
    public uint[] Values;

    public BitSetBase(int numValues)
    {
        Values = new uint[numValues];
    }

    public class Serializer : MetaClassSerializer<BitSetBase>
    {
        public override void PreSerialize(ref BitSetBase obj, MetaStream stream, MetaClassType? type = null)
        {
            ArgumentNullException.ThrowIfNull(type, "BitSetBase requires a metaclass type!");

            obj = type.Symbol.SymbolName switch
            {
                "BitSetBase<1>" => new BitSetBase(1),
                "BitSetBase<2>" => new BitSetBase(2),
                "BitSetBase<3>" => new BitSetBase(3),
                "BitSetBase<4>" => new BitSetBase(4),
                "BitSetBase<5>" => new BitSetBase(5),
                "BitSetBase<6>" => new BitSetBase(6),
                "BitSetBase<7>" => new BitSetBase(7),
                "BitSetBase<8>" => new BitSetBase(8),
                "BitSetBase<9>" => new BitSetBase(9),
                _ => obj
            };
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
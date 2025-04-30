using TelltaleToolKit.Serialization;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(BitSetBase.Serializer))]
public class BitSet<T> : BitSetBase where T : Enum
{
    // A rare case where I actually inherit, and it makes sense to do so.
    private readonly int _first;
    private readonly int _count;

    public int First => _first;
    public int Count => _count;


    public BitSet(int count, int first = 0) : base((count + 31) / 32)
    {
        _count = count;
        _first = first;
    }
}
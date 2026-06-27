using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types;

[MetaSerializer(typeof(BitSetSerializer<>), typeof(BitSet<>))]
public class BitSet<T> : BitSetBase where T : Enum
{
    // A rare case where I actually inherit, and it makes sense to do so.


    public BitSet(int count, int first = 0) : base((count + 31) / 32)
    {
        Count = count;
        First = first;
    }

    public int First { get; }

    public int Count { get; }

    public bool this[T enumValue]
    {
        get => GetBit(ConvertEnumToIndex(enumValue));
        set => SetBit(ConvertEnumToIndex(enumValue), value);
    }

    private int ConvertEnumToIndex(T enumValue)
    {
        int rawValue = Convert.ToInt32(enumValue);
        int index = rawValue - First;

        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(enumValue),
                $"Enum value {rawValue} is outside range [{First}, {First + Count - 1}]");
        }

        return index;
    }

    public void Set(T enumValue) => this[enumValue] = true;

    public void Clear(T enumValue) => this[enumValue] = false;

    public bool IsSet(T enumValue) => this[enumValue];

    public void SetRange(params T[] enumValues)
    {
        foreach (T value in enumValues)
        {
            Set(value);
        }
    }

    public void ClearRange(params T[] enumValues)
    {
        foreach (T value in enumValues)
        {
            Clear(value);
        }
    }

    public IEnumerable<T> GetSetValues()
    {
        for (int i = 0; i < Count; i++)
        {
            if (GetBit(i))
            {
                int enumValue = First + i;
                yield return (T)Enum.ToObject(typeof(T), enumValue);
            }
        }
    }

    private static int CalculateRequiredWords(int bitCount) => (bitCount + 31) / 32;

    private bool GetBit(int index)
    {
        int wordIndex = index / 32;
        int bitIndex = index % 32;
        return (Values[wordIndex] & (1u << bitIndex)) != 0;
    }

    private void SetBit(int index, bool value)
    {
        int wordIndex = index / 32;
        int bitIndex = index % 32;

        if (value)
        {
            Values[wordIndex] |= 1u << bitIndex;
        }
        else
        {
            Values[wordIndex] &= ~(1u << bitIndex);
        }
    }
}

public class BitSetSerializer<T> : MetaSerializer<BitSet<T>> where T : Enum
{
    public override void PreSerialize(ref BitSet<T>? obj, MetaStream stream, MetaClassType? type = null)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type), "BitSet<T> requires a metaclass type!");
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

        obj ??= new BitSet<T>(bitSetSize);

        if (obj.Values.Length != bitSetSize)
        {
            BitSet<T> newBitSet = new(bitSetSize);

            int copyLength = Math.Min(obj.Values.Length, bitSetSize);
            Array.Copy(obj.Values, 0, newBitSet.Values, 0, copyLength);

            obj = newBitSet;
        }
    }

    public override void Serialize(ref BitSet<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        for (int i = 0; i < obj.Values.Length; i++)
        {
            stream.Serialize(ref obj.Values[i]);
        }
    }
}

using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Flags>))]
public struct Flags
{
    // TODO: Potentially migrate to C# enum flags.
    // Some issues arise with the serializing process. For e.g., determining how the `class Flags` should be added to the serialized classes list.
    // Also, it will make serializers slower (but not by much). The reason why is because the type's property search complexity goes to O(N) instead of O(1).

    [MetaMember("mFlags")]
    public int Data { get; set; }

    public Flags(int data) => Data = data;

    public bool Has(int flag) => (Data & flag) == flag;
    public bool HasAny(int mask) => (Data & mask) != 0;

    public Flags Set(int flag) => new Flags(Data | flag);
    public Flags Clear(int flag) => new Flags(Data & ~flag);
    public Flags Toggle(int flag) => new Flags(Data ^ flag);

    public override string ToString() => $"0x{Data:X8}";

    public bool Equals(Flags other) => Data == other.Data;
    public override bool Equals(object? obj) => obj is Flags f && Equals(f);
    public override int GetHashCode() => Data;

    public static implicit operator int(Flags f) => f.Data;
    public static implicit operator Flags(int i) => new Flags(i);

    public static Flags operator |(Flags a, Flags b) => new Flags(a.Data | b.Data);
    public static Flags operator &(Flags a, Flags b) => new Flags(a.Data & b.Data);
    public static Flags operator ^(Flags a, Flags b) => new Flags(a.Data ^ b.Data);
    public static Flags operator ~(Flags a) => new Flags(~a.Data);
}
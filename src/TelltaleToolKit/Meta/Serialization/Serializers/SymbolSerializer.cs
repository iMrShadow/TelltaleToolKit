using TelltaleToolKit.Hashing;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(SymbolSerializer))]
public class SymbolSerializer : MetaSerializer<Symbol>
{
    public override void PreSerialize(ref Symbol? obj, MetaStream stream, MetaClassType? type = null)
    {
        obj ??= Symbol.Empty;
    }

    public override void Serialize(ref Symbol obj, MetaStream stream, MetaClassType? type = null)
    {
        MetaClass? description = stream.GetMetaClass(typeof(Symbol));

        if (description is null)
        {
            throw new InvalidOperationException("Symbol description not found.");
        }

        if (stream.Mode == MetaStreamMode.Write)
        {
            stream.AddVersionInfo(description);
        }

        ulong crc64 = stream.ReadUInt64();
        obj = Symbol.FromCrc64(crc64);
        stream.Params.SerializedSymbols.Add(obj);

        // What the helly, there's actually debug (although empty) string for symbols.
        // I am doing a hard unverified check, I haven't seen any symbols with debug string with MSV5 or MSV6.
        // Poker Night 2 might be different, but I doubt it
        if (stream.Params.StreamVersion <= 3)
        {
            string debug = stream.ReadString();
            if (!string.IsNullOrEmpty(debug) && Crc64.Compute(debug) == obj.Crc64)
            {
                obj = Symbol.FromExplicit(debug, obj.Crc64);
            }
            else if (Crc64.Compute(debug) != obj.Crc64 && debug.Length > 0)
            {
                Toolkit.Instance.Logger.LogWarning($"Symbol debug string mismatch: {debug} != {obj.Crc64}");
            }
        }
    }
}

using System.Text;

namespace TelltaleToolKit.T3Types;

public class Symbol
{
    public ulong Crc64 { get; }
    public string? SymbolName { get; set; } = string.Empty;
    public bool HasString() => !string.IsNullOrEmpty(SymbolName);

    public Symbol(string name)
    {
        SymbolName = name;
        Crc64 = GetCrc64(name);
    }

    public Symbol(ulong crc64)
    {
        Crc64 = crc64;
    }

    public static readonly Symbol DefaultSymbol = new(0);

    /// <summary>
    /// Get the CRC64 of the type name. This is used to identify the type in the metaclass header. This is consistent throughout all Telltale Tool games.
    /// </summary>
    /// <returns></returns>
    public static ulong GetCrc64(string symbol) =>
        System.IO.Hashing.Crc64.HashToUInt64(Encoding.UTF8.GetBytes(symbol.ToLowerInvariant()));

    public override string ToString()
        => HasString() ? $"{SymbolName}" : $"{Crc64}";
}
using System.Diagnostics;

namespace TelltaleToolKit.T3Types;

/// <summary>
///     Represents a Telltale Tool symbol, identified by a CRC64 hash of its lowercase name.
///     Symbols can be in one of three states:
///     <list type="bullet">
///         <item>
///             <description>Resolved: Both CRC64 and DebugString are known. DebugString can be overwritten.</description>
///         </item>
///         <item>
///             <description>Unresolved: Only CRC64 is known (DebugString is null).</description>
///         </item>
///         <item>
///             <description>Empty: CRC64 is 0, DebugString is empty.</description>
///         </item>
///     </list>
/// </summary>
public sealed class Symbol : IEquatable<Symbol>
{
    private Symbol(string name)
    {
        DebugString = name;
        Crc64 = Hashing.Crc64.Compute(name);
    }

    private Symbol(ulong crc64)
    {
        Crc64 = crc64;
        DebugString = null;
    }

    private Symbol(string name, ulong crc64)
    {
        DebugString = name;
        Crc64 = crc64;
    }

    public Symbol() : this(string.Empty)
    {

    }

    /// <summary>
    ///     Gets the empty symbol (CRC64 = 0, empty debug string).
    /// </summary>
    public static Symbol Empty { get; } = new(string.Empty, 0);

    /// <summary>
    ///     Gets the CRC64 hash that uniquely identifies this symbol.
    ///     This value never changes after construction.
    /// </summary>
    public ulong Crc64 { get; }

    /// <summary>
    ///     Gets the human-readable name of this symbol, or <c>null</c> if unresolved.
    /// </summary>
    public string? DebugString { get; private set; }

    /// <summary>Gets whether this symbol has a debug string attached.</summary>
    public bool IsResolved => DebugString is not null;

    /// <summary>Gets whether this symbol lacks a debug string.</summary>
    public bool IsUnresolved => DebugString is null;

    /// <summary>
    ///     Gets whether this is the canonical empty symbol (CRC64 = 0).
    /// </summary>
    /// <remarks>
    ///     Both <see cref="Empty" /> and any symbol created via <see cref="FromCrc64" />(0)
    ///     are considered empty, because CRC64 alone defines symbol identity.
    /// </remarks>
    public bool IsEmpty => Crc64 == 0;

    /// <summary>
    ///     Gets whether the attached <see cref="DebugString" /> (if any) is consistent with <see cref="Crc64" />.
    ///     Always returns <c>false</c> for unresolved symbols.
    /// </summary>
    public bool IsDebugStringConsistent =>
        DebugString is not null && Crc64 == Hashing.Crc64.Compute(DebugString);

    /// <inheritdoc />
    public bool Equals(Symbol? other) => other is not null && Crc64 == other.Crc64;

    /// <summary>Creates a resolved symbol by computing CRC64 from <paramref name="name" />.</summary>
    /// <remarks>The name is lowercased before hashing, making symbols case-insensitive.</remarks>
    public static Symbol FromName(string name)
        => new(name);

    /// <summary>Creates an unresolved symbol from a raw CRC64 value.</summary>
    /// <remarks>Use <see cref="Resolve" /> later to attach a debug string.</remarks>
    public static Symbol FromCrc64(ulong crc64)
        => crc64 == 0 ? Empty : new Symbol(crc64);

    /// <summary>
    ///     Creates a symbol with an explicitly provided name and CRC64, bypassing normal calculation.
    /// </summary>
    /// <remarks>
    ///     Intended for existing data that may have inconsistencies.
    ///     Use <see cref="IsDebugStringConsistent" /> to verify consistency after construction.
    /// </remarks>
    public static Symbol FromExplicit(string name, ulong crc64)
    {
        Symbol symbol = new(name, crc64);
        Debug.Assert(
            symbol.IsDebugStringConsistent,
            $"Symbol CRC mismatch: '{name}' hashes to 0x{Hashing.Crc64.Compute(name):X16}, but 0x{crc64:X16} was provided.");
        return symbol;
    }

    /// <summary>
    ///     Returns <c>true</c> if <paramref name="name" /> would hash to this symbol's <see cref="Crc64" />.
    /// </summary>
    public bool MatchesName(string name)
        => Crc64 == Hashing.Crc64.Compute(name);

    /// <summary>
    ///     Attaches a debug string to an unresolved symbol.
    /// </summary>
    /// <param name="name">The resolved name.</param>
    public void Resolve(string name)
    {
        Debug.Assert(Hashing.Crc64.Compute(name) == Crc64,
            $"Resolve mismatch: '{name}' hashes to 0x{Hashing.Crc64.Compute(name):X16}, expected 0x{Crc64:X16}.");
        DebugString = name;
    }

    /// <summary>
    ///     Returns the debug string if resolved; otherwise the CRC64 in hex (e.g. <c>1A2B3C4D5E6F7890</c>).
    /// </summary>
    public override string ToString() => DebugString ?? $"0x{Crc64:X16}";

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Symbol other && Crc64 == other.Crc64;

    /// <inheritdoc />
    /// <remarks>Based solely on <see cref="Crc64" />, which is immutable.</remarks>
    public override int GetHashCode() => Crc64.GetHashCode();

    public static bool operator ==(Symbol? left, Symbol? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Crc64 == right.Crc64;
    }

    public static bool operator !=(Symbol? left, Symbol? right) => !(left == right);
}

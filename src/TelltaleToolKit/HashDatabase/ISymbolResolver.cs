using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.HashDatabase;

/// <summary>
/// Defines a contract for resolving symbol names from symbol objects.
/// Implementers provide a mechanism to assign a string name to a <see cref="Symbol"/> based on its properties (such as CRC).
/// </summary>
public interface ISymbolResolver
{
    /// <summary>
    /// Attempts to resolve a <see cref="Symbol"/> by looking up its CRC64 hash if it has no string value.
    /// </summary>
    /// <param name="symbol">The symbol to resolve.</param>
    public void ResolveSymbol(Symbol symbol);
}
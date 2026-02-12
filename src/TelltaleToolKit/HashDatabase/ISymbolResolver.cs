using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.HashDatabase;

/// <summary>
/// Interface for resolving <see cref="Symbol"/> names from CRC64 values.
/// </summary>
public interface ISymbolResolver
{
    /// <summary>
    /// Resolves a single symbol.
    /// </summary>
    bool ResolveSymbol(Symbol symbol);
        
    /// <summary>
    /// Resolves multiple symbols in batch.
    /// </summary>
    int ResolveSymbols(IEnumerable<Symbol> symbols);
        
    /// <summary>
    /// Adds a symbol to the resolver.
    /// </summary>
    void AddSymbol(string symbolName);

    /// <summary>
    /// Adds a symbol with its CRC64.
    /// </summary>
    void AddSymbol(ulong crc64, string symbolName);
    
    bool RemoveSymbol(ulong crc64);
    bool RemoveSymbol(string symbolName);
        
    /// <summary>
    /// Clears all symbols.
    /// </summary>
    void Clear();
    
    bool Contains(ulong crc64);
    bool Contains(string symbolName);
    bool TryGetValue(ulong crc64, out string? symbolName);
        
    /// <summary>
    /// Gets the number of symbols in the resolver.
    /// </summary>
    int Count { get; }
}
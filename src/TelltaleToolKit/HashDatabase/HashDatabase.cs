using System.Collections.Concurrent;
using System.Text;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.HashDatabase;

/// <summary>
/// An in-memory, thread-safe hash database for storing and resolving <see cref="Symbol"/> entries.
/// Supports loading from text files and automatic caching.
/// </summary>
public class HashDatabase : ISymbolResolver
{
    private readonly ConcurrentDictionary<ulong, string> _symbols = new();

    /// <summary>
    /// Gets the number of symbols in the database.
    /// </summary>
    public int Count => _symbols.Count;
    
    public bool IsReadOnly { get; init; }

    /// <summary>
    /// Gets all symbols as CRC64-name pairs.
    /// </summary>
    public IEnumerable<KeyValuePair<ulong, string>> AllSymbols => _symbols;

    /// <summary>
    /// Creates an empty HashDatabase.
    /// </summary>
    public HashDatabase()
    {
    }

    /// <summary>
    /// Creates a HashDatabase preloaded from a directory.
    /// </summary>
    public HashDatabase(string directoryPath, string searchPattern = "*.txt", bool recursive = false)
    {
        ImportFromDirectory(directoryPath, searchPattern, recursive);
    }

    /// <summary>
    /// Creates a HashDatabase preloaded from specific files.
    /// </summary>
    public HashDatabase(IEnumerable<string> filePaths)
    {
        ImportFromFiles(filePaths);
    }

    /// <summary>
    /// Resolves a symbol by setting its SymbolName if found.
    /// </summary>
    public bool ResolveSymbol(Symbol symbol)
    {
        if (symbol == null) throw new ArgumentNullException(nameof(symbol));

        if (symbol.HasString())
            return true;

        if (_symbols.TryGetValue(symbol.Crc64, out string? name))
        {
            symbol.SymbolName = name;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Resolves multiple symbols efficiently.
    /// </summary>
    public int ResolveSymbols(IEnumerable<Symbol> symbols)
    {
        if (symbols == null) throw new ArgumentNullException(nameof(symbols));

        var resolved = 0;

        foreach (Symbol? symbol in symbols)
        {
            if (symbol == null || symbol.HasString())
                continue;

            if (_symbols.TryGetValue(symbol.Crc64, out string? name))
            {
                symbol.SymbolName = name;
                resolved++;
            }
        }

        return resolved;
    }

    /// <summary>
    /// Adds a symbol from its string representation.
    /// </summary>
    public void AddSymbol(Symbol symbol)
    {
        if (symbol.HasString())
        {
            AddSymbol(symbol.Crc64, symbol.SymbolName);
        }
    }
    
    /// <summary>
    /// Adds a symbol from its string representation.
    /// </summary>
    public void AddSymbol(string symbolName)
    {
        if (string.IsNullOrWhiteSpace(symbolName))
            throw new ArgumentException("Symbol name cannot be null or empty", nameof(symbolName));

        var symbol = new Symbol(symbolName);
        AddSymbol(symbol.Crc64, symbol.SymbolName);
    }

    /// <summary>
    /// Adds a symbol with its CRC64.
    /// </summary>
    public void AddSymbol(ulong crc64, string symbolName)
    {
        if (string.IsNullOrWhiteSpace(symbolName))
            throw new ArgumentException("Symbol name cannot be null or empty", nameof(symbolName));

        _symbols[crc64] = symbolName;
    }
    
    private void AddSymbolInternal(ulong crc64, string symbolName)
    {
        if (IsReadOnly)
            throw new InvalidOperationException("Cannot add symbols to a read-only database");
        
        _symbols[crc64] = symbolName;
    }

    /// <summary>
    /// Adds multiple symbols in batch.
    /// </summary>
    public void AddSymbols(IEnumerable<(ulong crc64, string name)> symbols)
    {
        foreach ((ulong crc64, string name) in symbols)
        {
            AddSymbol(crc64, name);
        }
    }

    /// <summary>
    /// Adds multiple symbols from their string representations.
    /// </summary>
    public void AddSymbols(IEnumerable<string> symbolNames)
    {
        foreach (string? name in symbolNames)
        {
            AddSymbol(name);
        }
    }
    
    /// <summary>
    /// Adds multiple symbols from their string representations.
    /// </summary>
    public void AddSymbols(IEnumerable<Symbol> symbols)
    {
        foreach (Symbol? symbol in symbols)
        {
            AddSymbol(symbol);
        }
    }

    /// <summary>
    /// Removes a symbol by CRC64.
    /// </summary>
    public bool RemoveSymbol(ulong crc64)
    {
        return _symbols.TryRemove(crc64, out _);
    }

    /// <summary>
    /// Removes a symbol by name.
    /// </summary>
    public bool RemoveSymbol(string symbolName)
    {
        var symbol = new Symbol(symbolName);
        return RemoveSymbol(symbol.Crc64);
    }

    /// <summary>
    /// Clears all symbols.
    /// </summary>
    public void Clear()
    {
        _symbols.Clear();
    }

    /// <summary>
    /// Checks if a symbol exists by CRC64.
    /// </summary>
    public bool Contains(ulong crc64)
    {
        return _symbols.ContainsKey(crc64);
    }

    /// <summary>
    /// Checks if a symbol exists by name.
    /// </summary>
    public bool Contains(string symbolName)
    {
        var symbol = new Symbol(symbolName);
        return Contains(symbol.Crc64);
    }

    /// <summary>
    /// Tries to get a symbol name by CRC64.
    /// </summary>
    public bool TryGetValue(ulong crc64, out string symbolName)
    {
        return _symbols.TryGetValue(crc64, out symbolName);
    }

    /// <summary>
    /// Scans a directory for text files and imports each non-empty, non-comment line as a symbol.
    /// </summary>
    public int ImportFromDirectory(string directoryPath, string searchPattern = "*.txt", bool recursive = false)
    {
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");

        string[] files = Directory.GetFiles(directoryPath, searchPattern,
            recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        return ImportFromFiles(files);
    }

    /// <summary>
    /// Imports symbols from specific files.
    /// </summary>
    public int ImportFromFiles(IEnumerable<string> filePaths)
    {
        var imported = 0;

        foreach (string? filePath in filePaths)
        {
            if (!File.Exists(filePath))
                continue;

            imported += ImportFromFile(filePath);
        }

        return imported;
    }

    /// <summary>
    /// Imports symbols from a single file.
    /// </summary>
    public int ImportFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"File not found: {filePath}");

        var imported = 0;

        foreach (string? line in File.ReadLines(filePath))
        {
            string trimmed = line.Trim();

            // Skip empty lines and comments
            if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#") || trimmed.StartsWith("//"))
                continue;

            // Handle tab-separated CRC64 and name format
            if (trimmed.Contains('\t'))
            {
                string[]? parts = trimmed.Split('\t');
                if (parts.Length >= 2 && ulong.TryParse(parts[0].Trim(), System.Globalization.NumberStyles.HexNumber,
                        null, out ulong crc64))
                {
                    AddSymbol(crc64, parts[1].Trim());
                    imported++;
                }
            }
            else
            {
                // Just a symbol name
                AddSymbol(trimmed);
                imported++;
            }
        }

        return imported;
    }

    /// <summary>
    /// Exports all symbols to a directory as text files.
    /// </summary>
    public void ExportToDirectory(string directoryPath, int maxPerFile = 10000)
    {
        Directory.CreateDirectory(directoryPath);

        List<KeyValuePair<ulong, string>> symbols = _symbols.ToList();
        var fileCount = 0;

        for (var i = 0; i < symbols.Count; i += maxPerFile)
        {
            IEnumerable<KeyValuePair<ulong, string>> chunk = symbols.Skip(i).Take(maxPerFile);
            string filePath = Path.Combine(directoryPath, $"symbols_{fileCount++:D4}.txt");

            ExportToFile(filePath, chunk);
        }
    }

    /// <summary>
    /// Exports symbols to a file.
    /// </summary>
    public void ExportToFile(string filePath, IEnumerable<KeyValuePair<ulong, string>> symbols = null)
    {
        IEnumerable<KeyValuePair<ulong, string>> targetSymbols = symbols ?? _symbols;

        using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

        writer.WriteLine("# CRC64 (hex) \\t Symbol Name");
        writer.WriteLine("# Exported from HashDatabase");
        writer.WriteLine();

        foreach (KeyValuePair<ulong, string> kvp in targetSymbols.OrderBy(kvp => kvp.Key))
        {
            writer.WriteLine($"{kvp.Key:X16}\t{kvp.Value}");
        }
    }

    /// <summary>
    /// Merges another hash database into this one.
    /// </summary>
    public void Merge(ISymbolResolver other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        if (other is HashDatabase memoryDb)
        {
            foreach (KeyValuePair<ulong, string> kvp in memoryDb._symbols)
            {
                AddSymbol(kvp.Key, kvp.Value);
            }
        }
        else
        {
            throw new NotImplementedException("Merging with non-HashDatabase not yet implemented");
        }
    }
}
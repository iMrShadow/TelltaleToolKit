using System.Collections.Concurrent;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.HashDatabase;

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

/// <summary>
/// Provides a SQLite-backed database for storing and resolving <see cref="Symbol"/> entries.
/// Implements <see cref="IDisposable"/> and <see cref="ISymbolResolver"/> for resource management and symbol resolution.
/// </summary>
public class HashDatabase : IDisposable, ISymbolResolver
{
    private readonly SqliteConnection _conn;
    private readonly object _dbLock = new();
    private readonly ConcurrentDictionary<ulong, string> _cache = new();
    private const string TableName = "Symbols";

    /// <summary>
    /// Initializes a new instance of the <see cref="HashDatabase"/> class and opens a connection to the specified database file.
    /// Ensures the schema is created if it does not exist.
    /// </summary>
    /// <param name="dbPath">Path to the SQLite database file.</param>
    /// <param name="enableWriteOptimizations">If true, sets WAL mode and turns synchronous off for faster bulk inserts.</param>
    public HashDatabase(string dbPath, bool enableWriteOptimizations = true)
    {
        if (string.IsNullOrWhiteSpace(dbPath))
            throw new ArgumentException("dbPath cannot be null or empty", nameof(dbPath));

        var csb = new SqliteConnectionStringBuilder { DataSource = dbPath };
        _conn = new SqliteConnection(csb.ConnectionString);
        _conn.Open();

        if (enableWriteOptimizations)
        {
            // These PRAGMAs are helpful for bulk imports; keep in mind they change durability guarantees.
            //  using SqliteCommand pragmaCmd = _conn.CreateCommand();
            // pragmaCmd.CommandText = "PRAGMA journal_mode=WAL; PRAGMA synchronous=NORMAL;";
            // pragmaCmd.ExecuteNonQuery();
        }

        InitializeSchema();
    }

    /// <summary>
    /// Ensures the schema (Bones, Properties tables) exists.
    /// </summary>
    private void InitializeSchema()
    {
        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $@"
                    CREATE TABLE IF NOT EXISTS {TableName} (
                        Crc   INTEGER PRIMARY KEY,
                        Value TEXT NOT NULL
                    );";
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Gets the number of entries in the hash database.
    /// </summary>
    /// <returns>The entry count.</returns>
    public int NumEntries()
    {
        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT COUNT(*) FROM {TableName};";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }

    /// <summary>
    /// Finds a symbol by CRC.
    /// </summary>
    public string? FindSymbol(ulong crc)
    {
        long key = CrcToKey(crc);

        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT Value FROM {TableName} WHERE Crc = @crc LIMIT 1;";
            cmd.Parameters.AddWithValue("@crc", key);
            object? res = cmd.ExecuteScalar();
            return res?.ToString();
        }
    }

    /// <summary>
    /// Finds a symbol using the in-memory cache, with automatic caching of lookups.
    /// </summary>
    public string? FindSymbolCached(ulong crc)
    {
        if (_cache.TryGetValue(crc, out string? found))
            return found;

        string? value = FindSymbol(crc);
        if (value is null)
            return null;

        _cache[crc] = value;
        return value;
    }

    /// <summary>
    /// Adds or updates a symbol entry.
    /// </summary>
    public void AddEntry(ulong crc, string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        long key = CrcToKey(crc);

        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"INSERT OR REPLACE INTO {TableName} (Crc, Value) VALUES (@crc, @val);";
            cmd.Parameters.AddWithValue("@crc", key);
            cmd.Parameters.AddWithValue("@val", value);
            cmd.ExecuteNonQuery();
        }

        _cache[crc] = value;
    }

    /// <summary>
    /// Adds a symbol from its original string.
    /// </summary>
    public void AddSymbol(string originalString)
    {
        ArgumentNullException.ThrowIfNull(originalString);
        var symbol = new Symbol(originalString);
        AddEntry(symbol.Crc64, symbol.SymbolName);
    }

    /// <summary>
    /// Bulk inserts CRC-value pairs for performance.
    /// </summary>
    public void BulkInsert(IEnumerable<(ulong CRC, string Value)> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        lock (_dbLock)
        {
            using SqliteTransaction trans = _conn.BeginTransaction();
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"INSERT OR REPLACE INTO {TableName} (Crc, Value) VALUES (@crc, @val);";
            var crcParam = cmd.Parameters.Add("@crc", SqliteType.Integer);
            var valParam = cmd.Parameters.Add("@val", SqliteType.Text);

            foreach ((ulong crc, string value) in entries)
            {
                crcParam.Value = CrcToKey(crc);
                valParam.Value = value;
                cmd.ExecuteNonQuery();

                _cache[crc] = value;
            }

            trans.Commit();
        }
    }

    /// <summary>
    /// Removes a single entry (returns true if deleted).
    /// </summary>
    public bool RemoveEntry(ulong crc)
    {
        long key = CrcToKey(crc);

        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"DELETE FROM {TableName} WHERE Crc = @crc;";
            cmd.Parameters.AddWithValue("@crc", key);
            int affected = cmd.ExecuteNonQuery();
            if (affected > 0)
                _cache.TryRemove(crc, out _);

            return affected > 0;
        }
    }

    /// <summary>
    /// Remove all entries (clears DB table and cache).
    /// </summary>
    public void Clear()
    {
        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"DELETE FROM {TableName};";
            cmd.ExecuteNonQuery();
        }

        _cache.Clear();
    }

    /// <summary>
    /// Dumps all entries from the table.
    /// </summary>
    public List<(ulong CRC, string Value)> DumpAll()
    {
        var results = new List<(ulong CRC, string Value)>();
        lock (_dbLock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = $"SELECT Crc, Value FROM {TableName};";
            using SqliteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                long signed = rdr.GetInt64(0);
                ulong crc = KeyToCrc(signed);
                string val = rdr.GetString(1);
                results.Add((crc, val));
            }
        }

        return results;
    }

    private static long CrcToKey(ulong crc) => unchecked((long)crc);
    private static ulong KeyToCrc(long key) => unchecked((ulong)key);

    /// <inheritdoc/>
    public void ResolveSymbol(Symbol symbol)
    {
        ArgumentNullException.ThrowIfNull(symbol);
        if (symbol.HasString()) return;

        string? val = FindSymbolCached(symbol.Crc64);
        if (val is not null)
            symbol.SymbolName = val;
    }

    /// <summary>
    /// Scans a directory for text files and imports each non-empty, non-comment line as a symbol string.
    /// Lines starting with '#' are treated as comments. Returns number of symbols imported.
    /// If a line already exists in DB (by CRC) it will be replaced.
    /// </summary>
    /// <param name="directoryPath">Directory to scan.</param>
    /// <param name="searchPattern">File search pattern, default "*.txt".</param>
    /// <param name="recursive">Search subdirectories if true.</param>
    public int ImportFromDirectory(string directoryPath, string searchPattern = "*.txt", bool recursive = false)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
            throw new ArgumentException("directoryPath cannot be null or empty", nameof(directoryPath));
        if (!Directory.Exists(directoryPath))
            throw new DirectoryNotFoundException(directoryPath);

        var files = Directory.EnumerateFiles(directoryPath, searchPattern,
            recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        var toInsert = new List<(ulong CRC, string Value)>();

        foreach (string file in files)
        {
            foreach (string rawLine in File.ReadLines(file))
            {
                if (string.IsNullOrWhiteSpace(rawLine)) continue;
                string line = rawLine.Trim();
                if (line.Length == 0) continue;
                if (line.StartsWith("#")) continue; // allow comment lines

                // treat the line as the original string to create a Symbol
                var s = new Symbol(line);
                toInsert.Add((s.Crc64, s.SymbolName));
            }
        }

        if (toInsert.Count > 0)
            BulkInsert(toInsert);

        return toInsert.Count;
    }

    /// <summary>
    /// Resolve a collection of symbols in bulk. This method:
    /// - uses the in-memory cache first,
    /// - batches unresolved CRCs into IN (...) queries to the DB for efficiency,
    /// - updates the cache and sets symbol.SymbolName for each resolved symbol.
    /// Returns the number of symbols that were resolved by this call (including those resolved from cache).
    /// </summary>
    /// <param name="symbols">Collection of symbols to resolve (can be List&lt;Symbol&gt; or any IEnumerable&lt;Symbol&gt;).</param>
    public int ResolveSymbols(IEnumerable<Symbol> symbols)
    {
        ArgumentNullException.ThrowIfNull(symbols);

        // Map of CRC -> list of symbols that need resolution (not resolved by cache)
        var unresolved = new Dictionary<ulong, List<Symbol>>();
        int resolvedCount = 0;

        // First pass: try cache and collect unresolved symbols
        foreach (var s in symbols)
        {
            if (s is null) continue;
            if (s.HasString()) continue;

            ulong crc = s.Crc64;
            if (_cache.TryGetValue(crc, out var cached))
            {
                s.SymbolName = cached;
                resolvedCount++;
                continue;
            }

            if (!unresolved.TryGetValue(crc, out var list))
            {
                list = [];
                unresolved[crc] = list;
            }

            list.Add(s);
        }

        if (unresolved.Count == 0)
            return resolvedCount;

        // Query DB in batches (avoid building huge IN clauses)
        const int BatchSize = 500;
        List<ulong> crcKeys = unresolved.Keys.ToList();

        for (var i = 0; i < crcKeys.Count; i += BatchSize)
        {
            List<ulong> chunk = crcKeys.Skip(i).Take(BatchSize).ToList();

            lock (_dbLock)
            {
                using SqliteCommand cmd = _conn.CreateCommand();
                var paramNames = new List<string>(chunk.Count);
                for (var p = 0; p < chunk.Count; p++)
                {
                    string param = "@p" + p;
                    paramNames.Add(param);
                    cmd.Parameters.AddWithValue(param, CrcToKey(chunk[p]));
                }

                cmd.CommandText = $"SELECT Crc, Value FROM {TableName} WHERE Crc IN ({string.Join(",", paramNames)});";

                using SqliteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    long signed = rdr.GetInt64(0);
                    ulong crc = KeyToCrc(signed);
                    string val = rdr.GetString(1);

                    // update cache
                    _cache[crc] = val;

                    // set all symbols that were waiting on this crc
                    if (unresolved.TryGetValue(crc, out List<Symbol>? waiting))
                    {
                        foreach (Symbol sym in waiting)
                        {
                            sym.SymbolName = val;
                            resolvedCount++;
                        }

                        unresolved.Remove(crc);
                    }
                }
            }
        }

        return resolvedCount;
    }


    private bool _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        if (disposing)
        {
            lock (_dbLock)
            {
                _conn?.Dispose();
            }
        }

        _disposed = true;
    }
}
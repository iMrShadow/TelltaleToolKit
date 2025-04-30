using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.HashDatabase;

using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

// TODO: Rework for v0.2.0
// Each game snapshot will have its own database with the following tables:
// Files
// Properties
// Bones

/// <summary>
/// Provides a SQLite-backed database for storing and resolving <see cref="Symbol"/> entries.
/// Implements <see cref="IDisposable"/> and <see cref="ISymbolResolver"/> for resource management and symbol resolution.
/// </summary>
public class HashDatabase : IDisposable, ISymbolResolver
{
    /// <summary>
    /// The underlying SQLite connection.
    /// </summary>
    private readonly SqliteConnection _conn;

    /// <summary>
    /// Synchronization lock for thread-safe operations.
    /// </summary>
    private readonly object _lock = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="HashDatabase"/> class and opens a connection to the specified database file.
    /// Ensures the schema is created if it does not exist.
    /// </summary>
    /// <param name="dbPath">Path to the SQLite database file.</param>
    public HashDatabase(string dbPath)
    {
        var connectionString = $"Data Source={dbPath}";
        _conn = new SqliteConnection(connectionString);
        _conn.Open();
        InitializeSchema();
    }

    /// <summary>
    /// Disposes the database connection.
    /// </summary>
    public void Dispose()
    {
        _conn.Dispose();
    }

    /// <summary>
    /// Ensures the entries table exists in the database.
    /// </summary>
    private void InitializeSchema()
    {
        using SqliteCommand cmd = _conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Entries (
                CRC INTEGER PRIMARY KEY,
                Value TEXT
            );";
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Gets the number of entries in the hash database.
    /// </summary>
    /// <returns>The entry count.</returns>
    public int NumEntries()
    {
        using SqliteCommand cmd = _conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Entries";
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    /// <summary>
    /// Finds the symbol value for the specified CRC hash.
    /// </summary>
    /// <param name="crc">The CRC64 hash to look up.</param>
    /// <returns>The symbol name if found; otherwise, <c>null</c>.</returns>
    public string? FindSymbol(ulong crc)
    {
        lock (_lock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT Value FROM Entries WHERE CRC = @crc LIMIT 1";
            cmd.Parameters.AddWithValue("@crc", crc);
            object? result = cmd.ExecuteScalar();
            return result?.ToString();
        }
    }

    /// <summary>
    /// Adds a symbol to the database from its original string.
    /// </summary>
    /// <param name="originalString">The original symbol string.</param>
    public void AddSymbol(string originalString)
    {
        var symbol = new Symbol(originalString);
        AddEntry(symbol.Crc64, symbol.SymbolName);
    }

    /// <summary>
    /// Adds or updates a CRC-value entry in the database.
    /// </summary>
    /// <param name="crc">The CRC64 hash.</param>
    /// <param name="value">The symbol value.</param>
    public void AddEntry(ulong crc, string value)
    {
        lock (_lock)
        {
            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = @"
                INSERT OR REPLACE INTO Entries (CRC, Value) VALUES (@crc, @value)";
            cmd.Parameters.AddWithValue("@crc", crc);
            cmd.Parameters.AddWithValue("@value", value);
            cmd.ExecuteNonQuery();
        }
    }

    /// <summary>
    /// Performs a bulk insert of CRC-value pairs using a transaction and prepared statement for performance.
    /// </summary>
    /// <param name="entries">The collection of entries to insert.</param>
    public void BulkInsert(IEnumerable<(ulong CRC, string Value)> entries)
    {
        lock (_lock)
        {
            using SqliteTransaction transaction = _conn.BeginTransaction();

            using SqliteCommand cmd = _conn.CreateCommand();
            cmd.CommandText = "INSERT OR REPLACE INTO Entries (CRC, Value) VALUES (@crc, @value)";

            SqliteParameter crcParam = cmd.Parameters.Add("@crc", SqliteType.Integer);
            SqliteParameter valueParam = cmd.Parameters.Add("@value", SqliteType.Text);

            foreach ((ulong crc, string value) in entries)
            {
                crcParam.Value = crc;
                valueParam.Value = value;
                cmd.ExecuteNonQuery();
            }

            transaction.Commit();
        }
    }

    /// <summary>
    /// Dumps all CRC-value entries from the database.
    /// </summary>
    /// <returns>A list of all entries as (CRC, Value) pairs.</returns>
    public List<(long CRC, string Value)> DumpAll()
    {
        var results = new List<(long, string)>();
        using SqliteCommand cmd = _conn.CreateCommand();
        cmd.CommandText = "SELECT CRC, Value FROM Entries";
        using SqliteDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            results.Add((reader.GetInt64(0), reader.GetString(1)));
        }

        return results;
    }

    /// <inheritdoc/>
    public void ResolveSymbol(Symbol symbol)
    {
        if (!symbol.HasString())
        {
            symbol.SymbolName = FindSymbol(symbol.Crc64);
        }
    }
}
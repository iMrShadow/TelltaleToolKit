namespace TelltaleToolKit.Utility.Caching;

/// <summary>
///     Defines the contract for a chunk-level cache.
/// </summary>
/// <remarks>
///     Implementations differ in memory usage and access-pattern suitability:
///     <list type="bullet">
///         <item><see cref="SinglePageCache" /> — one chunk hot at a time; ideal for linear scans.</item>
///         <item><see cref="LruChunkCache" />  — N most-recently-used chunks; best for clustered random access.</item>
///     </list>
/// </remarks>
public interface IChunkCache
{
    /// <summary>
    ///     Returns the decoded bytes for chunk <paramref name="chunkIndex" />, decoding
    ///     if not already cached. The caller must not mutate the returned array.
    /// </summary>
    /// <param name="chunkIndex">Zero-based index into the archive's chunk table.</param>
    /// <param name="decode">
    ///     Factory that performs the actual decompress+decrypt for the chunk.
    ///     Called only on a cache miss.
    /// </param>
    byte[] GetOrDecode(int chunkIndex, Func<int, byte[]> decode);

    /// <summary>Discards all cached state. Called between unrelated extraction operations.</summary>
    void Clear();
}

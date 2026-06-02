namespace TelltaleToolKit.Utility.Caching;

/// <summary>
///     Cache strategy that keeps exactly one decoded chunk in memory at a time.
/// </summary>
/// <remarks>
///     <para>
///         Memory overhead: one chunk (~64 KiB constant).
///     </para>
///     <para>
///         Best for: sequential / linear access patterns such.
///     </para>
///     <para>
///         Worst for: random or backward access — any access to a different chunk evicts
///         the current one, potentially causing re-decodes for clustered small files that
///         straddle two chunks.
///     </para>
///     <para>
///         Not thread-safe. Each concurrent extraction path should use its own instance.
///     </para>
/// </remarks>
public sealed class SinglePageCache : IChunkCache
{
    private byte[] _cachedData = [];
    private int _cachedIndex = -1;

    /// <inheritdoc />
    public byte[] GetOrDecode(int chunkIndex, Func<int, byte[]> decode)
    {
        if (_cachedIndex == chunkIndex)
        {
            return _cachedData;
        }

        _cachedData = decode(chunkIndex);
        _cachedIndex = chunkIndex;
        return _cachedData;
    }

    /// <inheritdoc />
    public void Clear()
    {
        _cachedIndex = -1;
        _cachedData = [];
    }
}

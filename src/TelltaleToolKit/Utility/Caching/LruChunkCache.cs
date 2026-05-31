namespace TelltaleToolKit.Utility.Caching;

/// <summary>
///     Cache strategy that retains the <em>N</em> most-recently-used decoded chunks.
/// </summary>
/// <remarks>
///     <para>
///         Memory overhead: up to <c>N * WindowSize</c> bytes (e.g., N=8, WindowSize=64 KiB -> 512 KiB).
///     </para>
///     <para>
///         Not thread-safe. Each concurrent extraction path should use its own instance.
///     </para>
/// </remarks>
public sealed class LruChunkCache : IChunkCache
{
    private readonly int _capacity;

    // Doubly linked list of (chunkIndex, data), most-recent at front.
    private readonly LinkedList<(int index, byte[] data)> _list = new();
    private readonly Dictionary<int, LinkedListNode<(int, byte[])>> _map = new();

    /// <summary>
    ///     Initializes a new <see cref="LruChunkCache" /> with the given capacity.
    /// </summary>
    /// <param name="capacity">
    ///     Maximum number of decoded chunks to retain simultaneously.
    ///     Typical values: 4 (conservative), 8 (balanced), 16 (aggressive).
    ///     Must be >= 1.
    /// </param>
    public LruChunkCache(int capacity = 8) =>
        // ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 1);
        _capacity = capacity;

    /// <inheritdoc />
    public byte[] GetOrDecode(int chunkIndex, Func<int, byte[]> decode)
    {
        // Cache hit — move to front (most recently used) and return.
        if (_map.TryGetValue(chunkIndex, out LinkedListNode<(int, byte[] data)>? node))
        {
            _list.Remove(node);
            _list.AddFirst(node);
            return node.Value.data;
        }

        // Cache miss — decode the chunk.
        byte[] data = decode(chunkIndex);

        // Evict the least-recently-used entry if at capacity.
        if (_list.Count == _capacity)
        {
            LinkedListNode<(int index, byte[] data)> lru = _list.Last!;
            _map.Remove(lru.Value.index);
            _list.RemoveLast();
        }

        // Insert at front.
        LinkedListNode<(int, byte[])> newNode = _list.AddFirst((chunkIndex, data));
        _map[chunkIndex] = newNode;

        return data;
    }

    /// <inheritdoc />
    public void Clear()
    {
        _list.Clear();
        _map.Clear();
    }
}

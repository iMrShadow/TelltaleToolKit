using TelltaleToolKit.IO.Archives;
using TelltaleToolKit.Utility.Hashing;

namespace TelltaleToolKit.IO.Resources;

/// <summary>
/// Represents a named, prioritized group of <see cref="IFileProvider"/> instances.
/// </summary>
/// <remarks>
/// <para>
/// A <see cref="ResourceContext"/> is the primary unit of resource organization inside a
/// <see cref="Workspace"/>.  Typical groupings are: one context per game-data archive set
/// (base game, DLC, patch), one context per mod, or one context per mounted folder.
/// </para>
/// <para>
/// Providers within a context are searched in insertion order.  The first provider that
/// contains the requested file wins, so add higher-priority providers first (e.g., loose
/// override files before packed archives).
/// </para>
/// <para>
/// When a context is <see cref="IsEnabled">disabled</see>, all lookup and extraction
/// methods return <see langword="null"/> / <see langword="false"/> immediately, letting
/// the workspace skip the context without removing it.
/// </para>
/// </remarks>
public class ResourceContext : IFileProvider
{
    private readonly List<IFileProvider> _providers = new();

    /// <summary>
    /// Creates a new resource context.
    /// </summary>
    /// <param name="name">Human-readable name used for lookup.</param>
    /// <param name="priority">
    /// Numeric priority relative to other contexts in the same workspace.
    /// Higher values are searched first during asset loading.
    /// </param>
    public ResourceContext(string name, int priority)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Priority = priority;
        IsEnabled = true;
    }

    /// <summary>Gets the human-readable name of this context.</summary>
    public string Name { get; }

    /// <summary>
    /// Gets the numeric priority of this context within its owning workspace.
    /// Higher values take precedence over lower values when searching for a file.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets whether this context participates in file lookups.
    /// The workspace skips disabled contexts entirely.
    /// </summary>
    public bool IsEnabled { get; private set; }

    /// <summary>Gets a read-only view of the providers registered with this context.</summary>
    public IReadOnlyList<IFileProvider> Providers => _providers;

    /// <summary>
    /// Appends <paramref name="provider"/> to the end of the provider list.
    /// Providers added later have lower priority within this context.
    /// </summary>
    /// <param name="provider">The provider to add; must not be <see langword="null"/>.</param>
    public void AddProvider(IFileProvider provider) => _providers.Add(provider);

    /// <summary>
    /// Removes <paramref name="provider"/> from the provider list.
    /// If <paramref name="provider"/> is an <see cref="IDisposable"/>, it is
    /// <em>not</em> disposed automatically — call <see cref="Unload"/> if you
    /// want disposal of all providers.
    /// </summary>
    /// <param name="provider">The provider to remove.</param>
    /// <returns>
    /// <see langword="true"/> if the provider was found and removed;
    /// <see langword="false"/> if it was not registered.
    /// </returns>
    public void RemoveProvider(IFileProvider provider) => _providers.Remove(provider);

    /// <summary>Enables this context so that its providers participate in file lookups.</summary>
    public void Enable() => IsEnabled = true;

    /// <summary>
    /// Disables this context so that all lookup and extraction methods return
    /// <see langword="null"/> / <see langword="false"/> without consulting providers.
    /// </summary>
    public void Disable() => IsEnabled = false;

    /// <summary>
    /// Disposes all <see cref="IDisposable"/> providers and clears the provider list.
    /// The context remains in memory but is effectively empty afterward.
    /// </summary>
    public void Unload()
    {
        foreach (IDisposable? provider in _providers.OfType<IDisposable>())
            provider.Dispose();
        _providers.Clear();
    }

    /// <inheritdoc/>
    /// <remarks>Returns <see langword="null"/> immediately when the context is disabled.</remarks>
    public Stream? ExtractFile(ulong crc64)
    {
        if (!IsEnabled) return null;

        foreach (IFileProvider? provider in _providers)
        {
            Stream? stream = provider.ExtractFile(crc64);
            if (stream != null) return stream;
        }

        return null;
    }

    /// <inheritdoc/>
    /// <remarks>Returns <see langword="false"/> immediately when the context is disabled.</remarks>
    public bool ContainsFile(ulong crc64)
        => IsEnabled && _providers.Any(p => p.ContainsFile(crc64));

    /// <inheritdoc/>
    /// <remarks>Returns <see langword="null"/> immediately when the context is disabled.</remarks>
    public ResourceEntry? GetFileEntry(ulong crc64)
    {
        if (!IsEnabled) return null;

        foreach (IFileProvider provider in _providers)
        {
            ResourceEntry? entry = provider.GetFileEntry(crc64);
            if (entry != null) return entry;
        }

        return null;
    }

    /// <inheritdoc/>
    public Stream? ExtractFile(string fileName)
        => ExtractFile(Crc64.Compute(fileName));

    /// <inheritdoc/>
    public bool ContainsFile(string fileName)
        => ContainsFile(Crc64.Compute(fileName));

    /// <inheritdoc/>
    public ResourceEntry? GetFileEntry(string fileName)
        => GetFileEntry(Crc64.Compute(fileName));

    /// <inheritdoc/>
    public IEnumerable<ResourceEntry> GetAllEntries()
        => _providers.SelectMany(p => p.GetAllEntries());
}

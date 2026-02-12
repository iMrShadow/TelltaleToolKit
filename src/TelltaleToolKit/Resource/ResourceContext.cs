using TelltaleToolKit.T3Types;
using TelltaleToolKit.TelltaleArchives;

namespace TelltaleToolKit.Resource;

// Core resource context - manages archives + loose files
public class ResourceContext : IFileProvider
{
    public string Name { get; }
    public int Priority { get; } // Explicit priority (Version 2)
    public bool IsEnabled { get; private set; }
    
    private readonly Workspace _game;
    private readonly List<IFileProvider> _providers = new();
    
    public ResourceContext(string name, int priority, Workspace workspace)
    {
        Name = name;
        Priority = priority;
        _game = workspace;
        IsEnabled = true;
    }
    
    // Add any type of provider
    public void AddProvider(IFileProvider provider) => _providers.Add(provider);
    public void RemoveProvider(IFileProvider provider) => _providers.Remove(provider);
    public IReadOnlyList<IFileProvider> Providers => _providers;
    
    public Stream? ExtractFile(ulong crc64)
    {
        foreach (IFileProvider? provider in _providers)
        {
            Stream? stream = provider.ExtractFile(crc64);
            if (stream != null) return stream;
        }
        return null;
    }
    
    public bool ContainsFile(ulong crc64)
        => _providers.Any(p => p.ContainsFile(crc64));
        
    public TelltaleFileEntry? GetFileEntry(ulong crc64)
    {
        foreach (IFileProvider? provider in _providers)
        {
            TelltaleFileEntry? entry = provider.GetFileEntry(crc64);
            if (entry != null) return entry;
        }
        return null;
    }
    
    public Stream? ExtractFile(string fileName)
        => ExtractFile(Symbol.GetCrc64(fileName));
    
    public bool ContainsFile(string fileName)
        => ContainsFile(Symbol.GetCrc64(fileName));
    
    public TelltaleFileEntry? GetFileEntry(string fileName)
        => GetFileEntry(Symbol.GetCrc64(fileName));
    
    public void Enable() => IsEnabled = true;
    public void Disable() => IsEnabled = false;
    
    public void Unload()
    {
        foreach (IDisposable? provider in _providers.OfType<IDisposable>())
            provider.Dispose();
        _providers.Clear();
    }
}

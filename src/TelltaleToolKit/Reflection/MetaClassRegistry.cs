using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Reflection;

/// <summary>
/// Registry for metaclass descriptions, allowing lookup and management by type hash and CRC32.
/// </summary>
public class MetaClassRegistry
{
    /// <summary>
    /// Dictionary for registered classes indexed by (CRC64 type hash, CRC32 version crc).
    /// </summary>
    public Dictionary<(ulong TypeHash, uint Crc32), MetaClass> Classes { get; } = [];

    /// <summary>
    /// Retrieves a class by MetaClassType and CRC32.
    /// </summary>
    public MetaClass? GetClass(MetaClassType type, uint crc32)
        => GetClass(type.Symbol.Crc64, crc32);

    /// <summary>
    /// Retrieves a class by type hash (CRC64) and CRC32.
    /// </summary>
    private MetaClass? GetClass(ulong typeHash, uint crc32)
        => GetClass((typeHash, crc32));

    /// <summary>
    /// Retrieves a class by symbol and CRC32.
    /// </summary>
    public MetaClass? GetClass(Symbol symbol, uint crc32)
        => GetClass((symbol.Crc64, crc32));

    /// <summary>
    /// Retrieves a class by key (type hash and CRC32).
    /// </summary>
    private MetaClass? GetClass((ulong TypeHash, uint Crc32) key)
    {
        return Classes.TryGetValue(key, out MetaClass? result) ? result : null;
    }

    /// <summary>
    /// Returns true if the given class is registered.
    /// </summary>
    public bool ContainsClass(MetaClass metaClass)
        => Classes.ContainsKey((metaClass.ClassType.Symbol.Crc64, metaClass.Crc32));

    /// <summary>
    /// Registers a single class if it is not already registered.
    /// </summary>
    public void RegisterClass(MetaClass metaClass)
    {
        if (!ContainsClass(metaClass))
            Classes[(metaClass.ClassType.Symbol.Crc64, metaClass.Crc32)] = metaClass;
    }

    /// <summary>
    /// Registers multiple classes.
    /// </summary>
    public void Register(IEnumerable<MetaClass> metaClasses)
    {
        foreach (MetaClass metaClass in metaClasses)
            RegisterClass(metaClass);
    }

    /// <summary>
    /// Prints all registered classes to the console.
    /// </summary>
    public void PrintRegisteredClasses()
    {
        foreach (MetaClass cls in Classes.Values)
            Console.WriteLine(
                $"{cls.ClassType.Symbol.SymbolName} => CRC64: {cls.ClassType.Symbol.Crc64:X} CRC32: {cls.Crc32:X}");
    }
}
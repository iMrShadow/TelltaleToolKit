namespace TelltaleToolKit.Reflection;

/// <summary>
/// Builder pattern for constructing <see cref="MetaClass"/> instances.
/// </summary>
public class MetaClassBuilder
{
    private readonly Func<MetaClass, uint> _crc32Function;
    private MetaClass _metaClass;

    /// <summary>
    /// Initializes a new instance of the builder with a CRC32 calculation function.
    /// </summary>
    /// <param name="crc32Function">A function that computes CRC32 for a <see cref="MetaClass"/>.</param>
    [Obsolete("This constructor is deprecated. Prefer to directly initialize the class's CRC32 with `Initialize`.")]
    public MetaClassBuilder(Func<MetaClass, uint> crc32Function)
    {
        _crc32Function = crc32Function ?? throw new ArgumentNullException(nameof(crc32Function));
        _metaClass = new MetaClass();
    }

    /// <summary>
    /// Resets the builder to a new <see cref="MetaClass"/>.
    /// </summary>
    private void Reset() => _metaClass = new MetaClass();

    /// <summary>
    /// Initializes the builder with a class name and optional CRC32.
    /// </summary>
    /// <param name="className">The name of the class.</param>
    /// <param name="crc32">Optional CRC32 value.</param>
    /// <returns>The builder instance (for chaining).</returns>
    public MetaClassBuilder Initialize(string className, uint crc32 = 0)
    {
        if (string.IsNullOrWhiteSpace(className))
            throw new ArgumentException("Class name cannot be null or whitespace.", nameof(className));

        _metaClass = new MetaClass
        {
            ClassType = MetaClassTypeRegistry.GetByName(className),
            Crc32 = crc32
        };
        return this;
    }

    /// <summary>
    /// Adds a member to the class being built.
    /// </summary>
    /// <param name="member">The member to add.</param>
    /// <returns>The builder instance (for chaining).</returns>
    private MetaClassBuilder AddMember(MetaMember member)
    {
        _metaClass.Members.Add(member ?? throw new ArgumentNullException(nameof(member)));
        return this;
    }
    
    /// <summary>
    /// Adds a member to the class by name, type, and optional flags.
    /// </summary>
    /// <param name="memberName">The member's name.</param>
    /// <param name="typeName">The member's type name.</param>
    /// <param name="flags">Optional flags for the member.</param>
    /// <returns>The builder instance (for chaining).</returns>
    public MetaClassBuilder AddMember(string memberName, string typeName, MetaFlags flags = MetaFlags.None)
    {
        if (string.IsNullOrWhiteSpace(memberName))
            throw new ArgumentException("Member name cannot be null or whitespace.", nameof(memberName));
        if (string.IsNullOrWhiteSpace(typeName))
            throw new ArgumentException("Type name cannot be null or whitespace.", nameof(typeName));

        var member = new MetaMember(memberName, MetaClassTypeRegistry.GetByName(typeName), flags);
        return AddMember(member);
    }

    /// <summary>
    /// Calculates and sets the CRC32 for the class.
    /// </summary>
    [Obsolete("This function is deprecated. Prefer to directly initialize the class's CRC32 in the constructor.")]
    private void CalculateCrc32() => _metaClass.Crc32 = _crc32Function(_metaClass);

    /// <summary>
    /// Finalizes the build, calculates CRC32, resets the builder, and returns the built class.
    /// </summary>
    /// <returns>The constructed <see cref="MetaClass"/>.</returns>
    public MetaClass Build()
    {
        CalculateCrc32();
        MetaClass result = _metaClass;
        Reset();
        return result;
    }
}
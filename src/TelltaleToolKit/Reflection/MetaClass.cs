namespace TelltaleToolKit.Reflection;

// TODO: Maybe Add enum descriptors. Currently we assume that the enums are fixed. It may not be worth it for the extra memory usage.

/// <summary>
/// Represents a description of a metaclass.
/// This includes type information, versioning, and a list of properties (members).
/// </summary>
public sealed class MetaClass
{
    /// <summary>
    /// Class information, including the name, original type name, and the linking type.
    /// </summary>
    public MetaClassType ClassType { get; init; } = null!;

    /// <summary>
    /// The CRC32 version number, which is calculated per game.
    /// Used to identify the version of the class.
    /// </summary>
    public uint Crc32 { get; set; }

    /// <summary>
    /// The members (properties) of this class.
    /// This is a list of MetaMember objects, each describing a property.
    /// </summary>
    public List<MetaMember> Members { get; init; } = [];

    /// <summary>
    /// Registers the current MetaClass instance with the toolkit context.
    /// </summary>
    [Obsolete(
        "The register function is deprecated. You can register the class using the context class. This function was previously used when I originally registered from code only.")]
    public void Register() => TTKContext.Instance().RegisterClass(this);

    /// <inheritdoc cref="MetaClassType.IsBlocked"/>
    public bool IsBlocked()
        => ClassType.IsBlocked();

    /// <inheritdoc cref="MetaClassType.IsSerialized"/>
    public bool IsSerialized()
        => ClassType.IsSerialized();

    /// <summary>
    /// Checks whether a member with the specified name exists in the class properties.
    /// </summary>
    /// <param name="name">The member name to check for.</param>
    /// <returns>True if the member exists; otherwise, false.</returns>
    public bool ContainsMember(string name)
        => Members.Any(member => member.MemberName.Equals(name));
}
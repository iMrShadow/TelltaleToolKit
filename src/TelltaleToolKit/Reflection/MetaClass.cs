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
    public List<MetaMember> Members { get; set; } = [];

    /// <summary>
    /// Checks whether a member with the specified name exists in the class properties.
    /// </summary>
    /// <param name="name">The member name to check for.</param>
    /// <returns>True if the member exists; otherwise, false.</returns>
    public bool ContainsMember(string name)
        => Members.Any(member => member.MemberName.Equals(name));
}
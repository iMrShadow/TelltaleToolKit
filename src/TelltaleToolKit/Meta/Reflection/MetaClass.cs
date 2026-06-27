using System.IO.Hashing;
using System.Text;

namespace TelltaleToolKit.Meta.Reflection;
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

    /// <summary>
    /// Calculate the version Crc for the given meta stream version and workspace.
    /// </summary>
    public static uint CalculateVersionCrc(MetaClass metaClass, uint streamVersion, Workspace? workspace = null)
    {
        var crc32 = new Crc32();

        // Step 1: Initial value depends on version
        if (streamVersion <= 2)
        {
            uint initial = metaClass.ClassType.IsBlocked() ? 0xFFFFu : 0u;
            crc32.Append(BitConverter.GetBytes(initial));
        }
        else // MTRE, MSV5, MSV6 all use the same int initializer
        {
            int initial = metaClass.ClassType.IsBlocked() ? -1 : 0;
            crc32.Append(BitConverter.GetBytes(initial));
        }

        // Step 2: Process members
        foreach (MetaMember member in metaClass.Members)
        {
            if (member.Flags.HasFlag(MetaFlags.MetaSerializeDisable))
                continue;

            // Member name is always appended first
            crc32.Append(Encoding.UTF8.GetBytes(member.MemberName));

            var metaclass = workspace?.GetMetaClassDescription(member.Type.LinkingType);

            if (metaclass == null)
                throw new InvalidOperationException($"MetaClass not found for type: {member.Type.LinkingType}");

            // Version specific - this is only a guess
            switch (streamVersion)
            {
                case 1:
                case 2:
                    string typeName = member.Type.FullTypeName;
                    // Hack for inherited classes in early MBIN builds
                    if (member.MemberName.StartsWith("Baseclass_"))
                        typeName += " *";
                    crc32.Append(Encoding.UTF8.GetBytes(typeName));
                    break;
                case 3:
                    crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64));
                    crc32.Append(
                        BitConverter.GetBytes(metaclass
                            .Crc32)); // TODO: This breaks when the type is does not have a metaclass description.
                    break;
                case 4:
                case 5:
                    crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64));
                    crc32.Append(new[] { Convert.ToByte(member.Type.IsBlocked()) });
                    crc32.Append(
                        BitConverter.GetBytes(metaclass
                            .Crc32)); // TODO: This breaks when the type is does not have a metaclass description.
                    break;
                case 6:
                    crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64));
                    crc32.Append(new[] { Convert.ToByte(member.Type.IsBlocked()) });
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(streamVersion),
                        $"Unsupported stream version: {streamVersion}");
            }
        }

        return crc32.GetCurrentHashAsUInt32();
    }

    public bool IsCompiledForWorkspace(Workspace workspace)
        => Crc32 == CalculateVersionCrc(this, workspace.Profile.StreamVersion, workspace);
}

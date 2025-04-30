using System.Text;
using System.Text.Json.Serialization;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Reflection;

/// <summary>
/// Represents metadata and reflection information for a class/type, including its name, symbol, linking .NET type, and metadata flags.
/// </summary>
public class MetaClassType
{
    public static readonly MetaClassType UninitializedClassType = new(string.Empty, typeof(int));

    /// <summary>
    /// Constructs a new <see cref="MetaClassType"/>.
    /// </summary>
    /// <param name="name">The full type name.</param>
    /// <param name="linkingType">The .NET type it links to.</param>
    /// <param name="flags">Optional metadata flags for this type.</param>
    public MetaClassType(string name, Type linkingType, MetaFlags flags = MetaFlags.None)
    {
        FullTypeName = name;
        Symbol = new Symbol(GetStrippedTypeName(name));
        LinkingType = linkingType;
        Flags = flags;
    }

    /// /// <summary>
    /// Gets the full type name of the class as defined in the original data.
    /// </summary>
    public string FullTypeName { get; }

    /// <summary>
    /// Gets the symbol representing this class/type.
    /// <para>
    /// The underlying string and its CRC64 are derived from the stripped type name in lower case.
    /// <br/>
    /// Example: <c>class DCArray&lt;struct Font::GlyphInfo&gt;</c> becomes <c>DCArray&lt;Font::GlyphInfo&gt;</c> for <see cref="T3Types.Symbol.SymbolName"/>,
    /// and <c>dcarray&lt;font::glyphinfo&gt;</c> for <see cref="T3Types.Symbol.Crc64"/>.
    /// </para>
    /// </summary>
    /// <seealso cref="T3Types.Symbol"/>
    public Symbol Symbol { get; }

    /// <summary>
    /// Gets the actual .NET type that this metaclass links to.
    /// </summary>
    public Type LinkingType { get; }

    /// <summary>
    /// Gets the flags describing this type's metadata. See <see cref="MetaFlags"/>.
    /// </summary>
    public MetaFlags Flags { get; }

    /// <summary>
    /// Determines whether this type is serialized in a blocked format.
    /// Blocked serialization is disabled if <see cref="MetaFlags.MetaSerializeBlockingDisabled"/> is present in <see cref="Flags"/>.
    /// </summary>
    /// <returns><c>true</c> if blocked serialization is enabled; otherwise, <c>false</c>.</returns>
    public bool IsBlocked() => !Flags.HasFlag(MetaFlags.MetaSerializeBlockingDisabled);

    /// <summary>
    /// Determines whether this type is not serialized at all.
    /// </summary>
    /// <returns><c>true</c> if serialization is enabled; otherwise, <c>false</c>.</returns>
    public bool IsSerialized() => !Flags.HasFlag(MetaFlags.MetaSerializeDisable);

    /// <summary>
    /// Removes common C++ type prefixes and whitespace from a type name, producing a canonical type name.
    /// </summary>
    /// <param name="name">The type name to strip.</param>
    /// <returns>The stripped type name.</returns>
    public static string GetStrippedTypeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        return new StringBuilder(name)
            .Replace("class ", string.Empty)
            .Replace("struct ", string.Empty)
            .Replace("enum ", string.Empty)
            .Replace("std::", string.Empty)
            .Replace(" ", string.Empty)
            .ToString();
    }
}
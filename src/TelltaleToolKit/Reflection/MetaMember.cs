namespace TelltaleToolKit.Reflection;

public record MetaMember(
    string MemberName,
    MetaClassType Type,
    MetaFlags Flags = MetaFlags.None)
{
    public bool IsFlagType() => Flags.HasFlag(MetaFlags.FlagType);
    public bool IsEnumType() => Flags.HasFlag(MetaFlags.EnumIntType) || Flags.HasFlag(MetaFlags.EnumStringType);
    
    /// <inheritdoc cref="MetaClassType.IsSerialized"/>
    public bool IsSerialized() => !Flags.HasFlag(MetaFlags.MetaSerializeDisable) && Type.IsSerialized();
}
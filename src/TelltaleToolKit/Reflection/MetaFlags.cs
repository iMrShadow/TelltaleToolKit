namespace TelltaleToolKit.Reflection;

/// <summary>
/// Flags to describe metadata properties for classes/types.
/// </summary>
[Flags]
public enum MetaFlags
{
    /// <summary>
    /// The type has no flags.
    /// </summary>
    None = 0,

    /// <summary>
    /// The type cannot be serialized.
    /// </summary>
    MetaSerializeDisable = 0x1,

    /// <summary>
    /// The type is not blocked in serialization.
    /// If this flag is not applied, a block size will be added.
    /// </summary>
    MetaSerializeBlockingDisabled = 0x2,

    /// <summary>
    /// The class be displayed in the property menu of the game engine's editor.
    /// </summary>
    PlaceInAddPropMenu = 0x4,

    /// <summary>
    /// No caption in the panel it is in.
    /// </summary>
    NoPanelCaption = 0x8,

    /// <summary>
    /// This type is a base class. Base classes are inherited types.
    /// </summary>
    BaseClass = 0x10,

    /// <summary>
    /// Do not show this in the game engine editor.
    /// </summary>
    EditorHide = 0x20,

    /// <summary>
    /// The type is an enum of type int.
    /// </summary>
    EnumIntType = 0x40,

    /// <summary>
    /// The type is an enum of type string.
    /// </summary>
    EnumStringType = 0x80,

    /// <summary>
    /// This type is a container type (map/set/array/etc).
    /// </summary>
    ContainerType = 0x100,

    /// <summary>
    /// Is a script enum type (enum used in lua scripts).
    /// </summary>
    ScriptEnum = 0x200,

    /// <summary>
    /// The name of this type (in meta member descriptions) is allocated on the heap.
    /// </summary>
    Heap = 0x400,

    /// <summary>
    /// Serialized or created from lua scripts.
    /// </summary>
    ScriptTransient = 0x800,

    /// <summary>
    /// Unknown. Related to select agent type.
    /// </summary>
    SelectAgentType = 0x1000,

    /// <summary>
    /// Unknown. Just object state is a meta operation.
    /// </summary>
    SkipObjectState = 0x2000,

    /// <summary>
    /// Unknown. Indicates object is not cacheable.
    /// </summary>
    NotCacheable = 0x4000,

    /// <summary>
    /// This type wraps an enum. Eg, this type is a struct EnumPlatformType{PlatformType mType}, where PlatformType is an enum.
    /// The reason it's like this is that the `EnumPlatformType` struct inherits from EnumBase and that has a separate description.
    /// </summary>
    EnumWrapperClass = 0x8000,

    /// <summary>
    /// Unknown. Temporary description.
    /// </summary>
    TempDescription = 0x10000,

    /// <summary>
    /// This type is a handle (reference to a file). If serialized this is a CRC.
    /// </summary>
    Handle = 0x20000,

    /// <summary>
    /// This type has a list of flags present with it (eg FlagsT3LightEnvGroupSet).
    /// </summary>
    FlagType = 0x40000,

    /// <summary>
    /// Unknown. Related to select folder type.
    /// </summary>
    SelectFolderType = 0x80000,

    /// <summary>
    /// This type does not have any members.
    /// </summary>
    Memberless = 0x100000,

    /// <summary>
    /// This type is a renderable resource (eg a texture or font).
    /// </summary>
    RenderResource = 0x200000,

    /// <summary>
    /// If this type has a block around it but the size of the serialized version is not always one value.
    /// The value is <c>0x400000 | MetaSerializeBlockingDisabled</c>, meaning it also includes <see cref="MetaSerializeBlockingDisabled"/>.
    /// </summary>
    MetaSerializeNonBlockedVariableSize = 0x400000 | MetaSerializeBlockingDisabled,

    /// <summary>
    /// Unknown. Embedded cacheable resource.
    /// </summary>
    EmbeddedCacheableResource = 0x800000,

    /// <summary>
    /// Unknown. Virtual resource.
    /// </summary>
    VirtualResource = 0x1000000,

    /// <summary>
    /// This class is not allowed to be serialized asynchronously, i.e., it's a very large class.
    /// Usually used for textures and meshes.
    /// </summary>
    DontAsyncLoad = 0x2000000,

    /// <summary>
    /// This type is not a meta file.
    /// </summary>
    IsNotMetaFile = 0x4000000,
}
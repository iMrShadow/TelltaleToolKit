namespace TelltaleToolKit.Serialization;

// TODO: Maybe use this only for custom serializers and automatically generate serializers.
// The downside is (if it's true) that there will be serializers for types that aren't needed.
/// <summary>
/// Declares a serializer as a global serializer for a specific type,
/// allowing it to be automatically discovered and registered at runtime.
/// Can be applied to assemblies, classes, structs, or enums.
/// </summary>
/// <remarks>
/// If <see cref="MetaClassSerializerGlobalAttribute.SerializedType"/> is a generic type definition, this attribute
/// can be used to register open generic serializers (e.g., for <c>List&lt;&gt;</c>).
/// </remarks>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class MetaClassSerializerGlobalAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MetaClassSerializerGlobalAttribute"/> class.
    /// </summary>
    /// <param name="serializerType">
    /// The type of the serializer (must inherit from <see cref="MetaClassSerializer"/>).
    /// </param>
    /// <param name="serializedType">
    /// The target type this serializer handles. If null, the serializer will be queried for its supported type.
    /// </param>
    public MetaClassSerializerGlobalAttribute(Type? serializerType, Type? serializedType = null)
    {
        SerializerType = serializerType;
        SerializedType = serializedType;
    }

    /// <summary>
    /// Gets the serializer type to be registered.
    /// </summary>
    public Type? SerializerType { get; }
    
    /// <summary>
    /// Gets the type that will be serialized/deserialized by the serializer.
    /// </summary>
    public Type? SerializedType { get; }
}
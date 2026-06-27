using System.Runtime.CompilerServices;
using TelltaleToolKit.Meta.Reflection;

namespace TelltaleToolKit.Meta.Serialization;

/// <summary>
/// Describes how to serialize and deserialize an object without knowing its type.
/// Used as a common base class for all data serializers.
/// </summary>
public abstract class MetaSerializer
{
    /// <summary>
    /// The type of the object that can be serialized or deserialized.
    /// </summary>
    public abstract Type SerializationType { get; }

    /// <summary>
    /// Initializes the specified serializer.
    /// </summary>
    /// <remarks>This method should be thread-safe and OK to call multiple times.</remarks>
    /// <param name="serializerSelector">The serializer.</param>
    public virtual void Initialize(MetaSerializerSelector serializerSelector)
    {
    }

    /// <summary>
    /// Serializes or deserializes the given object <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">The object to serialize or deserialize.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    /// <param name="type"></param>
    public abstract void Serialize(ref object obj, MetaStream stream, MetaClassType? type = null);

    /// <summary>
    /// Performs the first step of serialization or deserialization.
    /// </summary>
    /// <remarks>
    /// Typically, it will instantiate the object if [null], and if it's a collection, clear it.
    /// </remarks>
    /// <param name="obj">The object to process.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    /// <param name="type"></param>
    public abstract void PreSerialize(ref object? obj, MetaStream stream, MetaClassType? type = null);
}

/// <summary>
/// Describes how to serialize and deserialize an object of a given type.
/// </summary>
/// <typeparam name="T">The type of object to serialize or deserialize.</typeparam>
public abstract class MetaSerializer<T> : MetaSerializer
{
    /// <inheritdoc/>
    public override Type SerializationType => typeof(T);

    /// <inheritdoc/>
    public override void Serialize(ref object obj, MetaStream stream, MetaClassType? type = null)
    {
        T objT = (T)obj;
        Serialize(ref objT, stream);
        obj = objT!;
    }

    /// <summary>
    /// Serializes the given object <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">The object to serialize or deserialize.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Serialize(T obj, MetaStream stream)
    {
        Serialize(ref obj, stream);
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref object? obj, MetaStream stream, MetaClassType? type = null)
    {
        T? objT = obj == null ? default : (T)obj;
        PreSerialize(ref objT, stream, type);
        obj = objT;
    }

    /// <summary>
    /// Performs the first step of serialization or deserialization.
    /// </summary>
    /// <remarks>
    /// Typically, it will instantiate the object if [null], and if it's a collection, clear it.
    /// </remarks>
    /// <param name="obj">The object to process.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    /// <param name="type"></param>
    public virtual void PreSerialize(ref T? obj, MetaStream stream, MetaClassType? type = null)
    {
    }

    /// <summary>
    /// Serializes or deserializes the given object <paramref name="obj"/>.
    /// </summary>
    /// <param name="obj">The object to serialize or deserialize.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    /// <param name="type"></param>
    public abstract void Serialize(ref T obj, MetaStream stream, MetaClassType? type = null);
}

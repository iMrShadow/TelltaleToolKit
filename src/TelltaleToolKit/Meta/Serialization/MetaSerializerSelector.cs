using System.Reflection;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.Meta.Serialization;

/// <summary>
/// Responsible for selecting and providing the appropriate <see cref="MetaSerializer"/>
/// instance for a given .NET type, including support for open generics and arrays.
/// </summary>
public class MetaSerializerSelector
{
    private readonly Dictionary<Type, MetaSerializer> _serializers = new();

    // Open generics (List<>, Dictionary<,>, etc.)
    private readonly Dictionary<Type, Type> _openGenericSerializers = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="MetaSerializerSelector"/> class,
    /// scanning loaded assemblies for serializer registrations.
    /// </summary>
    public MetaSerializerSelector()
    {
        Initialize();
    }

    /// <summary>
    /// Registers a serializer instance for the specified type key.
    /// </summary>
    /// <param name="key">Type of the object to be (de)serialized.</param>
    /// <param name="serializer">Serializer instance for that type.</param>
    public void Register(Type? key, MetaSerializer serializer)
    {
        if (key != null)
            _serializers[key] = serializer;
    }

    /// <summary>
    /// Registers a generic serializer instance for the specified type key.
    /// </summary>
    /// <typeparam name="T">The type being serialized.</typeparam>
    /// <param name="key">Type of the object to be (de)serialized.</param>
    /// <param name="serializer">Serializer instance for that type.</param>
    public void Register<T>(Type? key, MetaSerializer<T> serializer)
    {
        if (key != null)
            _serializers[key] = serializer;
    }

    /// <summary>
    /// Registers a serializer instance by inferring its serialization type.
    /// </summary>
    /// <param name="serializer">Serializer instance.</param>
    public void Register(object serializer)
    {
        var type = (MetaSerializer)serializer;
        _serializers[type.SerializationType] = (MetaSerializer)serializer;
    }

    /// <summary>
    /// Gets the serializer for the specified type parameter.
    /// </summary>
    /// <typeparam name="T">The type to (de)serialize.</typeparam>
    /// <returns>The serializer instance, or null if not found.</returns>
    public MetaSerializer<T> GetSerializer<T>()
        => GetSerializer(typeof(T)) as MetaSerializer<T>;


    /// <summary>
    /// Gets the serializer for the given .NET type.
    /// </summary>
    /// <param name="type">The type that you want to (de)serialize.</param>
    /// <returns>
    /// The <see cref="MetaSerializer"/> for this type if it exists or can be created; otherwise, throws.
    /// </returns>
    /// <exception cref="NotSupportedException">Thrown when a serializer cannot be found for the provided type.</exception>
    public MetaSerializer GetSerializer(Type type)
    {
        if (_serializers.TryGetValue(type, out MetaSerializer? serializer))
        {
            return serializer;
        }

        // Handle 1D arrays
        if (type.IsArray && type.GetArrayRank() == 1)
        {
            Type? elementType = type.GetElementType();
            Type serializerType = typeof(ArraySerializer<>).MakeGenericType(elementType);
            var arraySerializer = (MetaSerializer)Activator.CreateInstance(serializerType)!;
            arraySerializer?.Initialize(this);
            _serializers[type] = arraySerializer; // Cache for later
            return arraySerializer;
        }

        // Handle open generic types
        if (type.IsGenericType)
        {
            Type genericDef = type.GetGenericTypeDefinition();

            if (_openGenericSerializers.TryGetValue(genericDef, out Type? serializerGenericType))
            {
                // Instantiate the open generic serializer for the type arguments
                Type[] args = type.GetGenericArguments();
                Type closedSerializerType = serializerGenericType.MakeGenericType(args);
                var closedSerializer = (MetaSerializer)Activator.CreateInstance(closedSerializerType)!;

                closedSerializer?.Initialize(this);

                if (closedSerializer != null)
                {
                    _serializers[type] = closedSerializer;
                    return closedSerializer;
                }
            }
        }

        throw new NotSupportedException($"No serializer was found for {type}!");
    }

    /// <summary>
    /// Scans all loaded assemblies for types decorated with <see cref="MetaSerializerAttribute"/>
    /// and registers the corresponding serializers.
    /// </summary>
    public void Initialize()
    {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                var globalAttr = type.GetCustomAttribute<MetaSerializerAttribute>();

                if (globalAttr == null) continue;

                Type? serializerType = globalAttr.SerializerType;
                Type? serializedType = globalAttr.SerializedType;

                if (serializedType is { IsGenericTypeDefinition: true })
                {
                    // Register the open generic serializer type for later instantiation
                    _openGenericSerializers[serializedType] = serializerType;
                }
                else if (serializedType != null)
                {
                    // Register a closed serializer instance
                    var serializer = (MetaSerializer)Activator.CreateInstance(serializerType);
                    serializer?.Initialize(this);
                    _serializers[serializedType] = serializer;
                }
                else
                {
                    var serializer = (MetaSerializer)Activator.CreateInstance(serializerType);
                    serializer?.Initialize(this);
                    _serializers[serializer.SerializationType] = serializer;
                }
            }
        }
    }
}

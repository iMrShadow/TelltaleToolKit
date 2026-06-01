using System.Reflection;
using TelltaleToolKit.Meta;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;
using TransitionMap = TelltaleToolKit.T3Types.TransitionMap;

namespace TelltaleToolKit.Serialization.Serializers;

/// <summary>
/// Provides a default implementation of <see cref="MetaClassSerializer{T}"/> for class serialization,
/// using reflection and attribute caching to efficiently map class properties to meta members.
/// <para>
/// This serializer builds a cache of properties with <see cref="MetaMemberAttribute"/>, linking them to their
/// serialization metadata and delegates for fast access.
/// </para>
/// <para>
/// This is the equivalent operation of 'MetaOp_SerializeMain` and 'MetaOp_SerializeAsync' in Telltale Tool.
/// </para>
/// </summary>
/// <typeparam name="T">The type of the class being serialized.</typeparam>
public class DefaultClassSerializer<T> : MetaClassSerializer<T> where T : new()
{
    private static readonly Dictionary<(string, Type), CachedMember> MemberCache;

    // Initialize the mapping dictionary.
    static DefaultClassSerializer()
    {
        MemberCache = typeof(T).GetProperties()
            .Select(p => new { Property = p, Attribute = p.GetCustomAttribute<MetaMemberAttribute>() })
            .Where(p => p.Attribute is not null)
            .ToDictionary(
                p => (p.Attribute!.Name, p.Property.PropertyType),
                p => new CachedMember
                {
                    Property = p.Property,
                    Getter = CreateGetterDelegate(p.Property),
                    Setter = CreateSetterDelegate(p.Property),
                }
            );
    }


    /// <inheritdoc/>
    public override void Serialize(ref T obj, MetaStream stream)
    {
        MetaClass? description = stream.GetMetaClass(typeof(T));

        Toolkit.Instance.Logger.LogInfo($"Reading {typeof(T).Name}.");
        if (description is null || !description.ClassType.IsSerialized())
        {
            if (description is null)
                Toolkit.Instance.Logger.LogWarning(
                    $"[DefaultClassSerializer] No description available for {typeof(T).Name}.");

            return;
        }

        stream.BeginObject(typeof(T).Name);
        // Add this class to the metaclass header.
        if (stream is { Mode: MetaStreamMode.Write, Params.CanModifySerializedClassesList: true })
        {
            stream.AddVersionInfo(description);
        }

        // Loop through the metaclass's properties/members
        foreach (MetaMember propDesc in description.Members.Where(propDesc => propDesc.IsSerialized()))
        {
            CachedMember? cached = ResolveMember(propDesc);

            if (propDesc.Type.IsBlocked())
                stream.BeginBlock();


            stream.Key(propDesc.MemberName);

            if (propDesc.Type.LinkingType.IsPrimitive || (propDesc.Type.LinkingType == typeof(string) ||
                                                          propDesc.Type.LinkingType == typeof(byte[]) ||
                                                          propDesc.Type.LinkingType == typeof(Symbol)))
            {

            }
            else
            {
                stream.BeginObject(propDesc.MemberName);
            }

            object? value = cached?.Getter(ref obj);

            MetaClassSerializer serializer =
                Toolkit.Instance.GetSerializer(cached?.Property.PropertyType ?? propDesc.Type.LinkingType);
            serializer.PreSerialize(ref value, stream, propDesc.Type);
            serializer.Serialize(ref value, stream);

            Toolkit.Instance.Logger.LogInfo($"{propDesc.MemberName} - {value}");

            if (stream.Mode is MetaStreamMode.Read)
                cached?.Setter(ref obj, value);

            if (propDesc.Type.LinkingType.IsPrimitive || propDesc.Type.LinkingType == typeof(string) ||
                propDesc.Type.LinkingType == typeof(byte[]) ||
                propDesc.Type.LinkingType == typeof(Symbol))
            {

            }
            else
            {
                stream.EndObject(propDesc.MemberName);
            }

            if (propDesc.Type.IsBlocked())
                stream.EndBlock();
        }

        stream.EndObject(typeof(T).Name);
        Toolkit.Instance.Logger.LogInfo($"Ending {typeof(T).Name}.");
    }

    public override void PreSerialize(ref T obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Read && obj == null)
        {
            obj = new T();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="propDesc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private static CachedMember? ResolveMember(MetaMember propDesc)
    {
        // 1. Try direct match
        if (MemberCache.TryGetValue((propDesc.MemberName, propDesc.Type.LinkingType), out CachedMember? cached))
            return cached;

        // 2. Enum/flag fallback: the member's type is an int, but also an enum
        if ((propDesc.IsEnumType() || propDesc.IsFlagType()) && propDesc.Type.LinkingType.IsPrimitive)
        {
            CachedMember? cachedAlt = MemberCache
                .FirstOrDefault(kvp => kvp.Key.Item1 == propDesc.MemberName && kvp.Value.Property.PropertyType.IsEnum)
                .Value;
            if (cachedAlt != null)
                return cachedAlt;
        }

        // 5. int<->Flags
        if (propDesc.Type.LinkingType == typeof(int))
        {
            if (MemberCache.TryGetValue((propDesc.MemberName, typeof(Flags)), out CachedMember? cachedAlt))
                return cachedAlt;
        }

        // Final: First available
        if (propDesc.Type.LinkingType == typeof(int) || propDesc.Type.LinkingType == typeof(uint))
        {
            CachedMember? cachedAltFinal = MemberCache
                .FirstOrDefault(kvp => kvp.Key.Item1 == propDesc.MemberName)
                .Value;
            if (cachedAltFinal != null)
                return cachedAltFinal;
        }

        Toolkit.Instance.Logger.LogError(
            $"Property {propDesc.MemberName} with type {propDesc.Type.LinkingType} not found in class {typeof(T)}");
        return null;
    }

    // Delegate signatures for ref struct support
    private delegate object? FuncRefGetter<TVal>(ref TVal obj);

    private delegate void ActionRefSetter<TVal>(ref TVal obj, object? value);

    // Getter delegate, uses ref for correct struct handling
    private static FuncRefGetter<T> CreateGetterDelegate(PropertyInfo prop)
    {
        return (ref T instance) => prop.GetValue(instance);
    }

    // Setter delegate, uses ref for correct struct handling
    private static ActionRefSetter<T> CreateSetterDelegate(PropertyInfo prop)
    {
        if (!prop.CanWrite)
            return (ref T instance, object? value) => { };

        if (typeof(T).IsValueType)
        {
            // For structs, use boxing: unbox, set, re-assign
            return (ref T instance, object? value) =>
            {
                object boxed = instance!;
                prop.SetValue(boxed, value);
                instance = (T)boxed;
            };
        }

        // For classes, direct set
        return (ref T instance, object? value) => { prop.SetValue(instance, value); };
    }

    private class CachedMember
    {
        public PropertyInfo Property = null!;
        public FuncRefGetter<T> Getter = null!;
        public ActionRefSetter<T> Setter = null!;
    }
}

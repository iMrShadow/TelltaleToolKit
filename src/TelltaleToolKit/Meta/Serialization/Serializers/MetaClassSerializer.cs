using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

/// <summary>
/// Provides a default implementation of <see cref="MetaSerializer{T}"/> for class serialization,
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
public class MetaClassSerializer<T> : MetaSerializer<T> where T : new()
{
    private static readonly Dictionary<(string, Type), CachedMember> MemberCache;

    // Initialize the mapping dictionary.
    static MetaClassSerializer()
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

        var fallbackEntries = new List<((string, Type), CachedMember)>();

        foreach (var kvp in MemberCache)
        {
            var (name, propType) = kvp.Key;
            var member = kvp.Value;

            // Enum properties -> also register under int/uint
            if (propType.IsEnum)
            {
                AddIfMissing((name, typeof(int)), member);
                AddIfMissing((name, typeof(uint)), member);
                // If Flags is a base type of this enum, register that too
                if (typeof(Flags).IsAssignableFrom(propType))
                    AddIfMissing((name, typeof(Flags)), member);
            }
            // Flags type -> register as int/uint
            else if (propType == typeof(Flags))
            {
                AddIfMissing((name, typeof(int)), member);
                AddIfMissing((name, typeof(uint)), member);
            }
            // int/uint -> optionally register enum/Flags lookups? You know your schema best.
            // Also add any inheritance‑based matches you’d otherwise search for.
        }

        foreach (var entry in fallbackEntries)
            if (!MemberCache.ContainsKey(entry.Item1))
                MemberCache[entry.Item1] = entry.Item2;

        foreach (var cached in MemberCache.Values)
        {
            cached.Serializer = new Lazy<MetaSerializer>(
                () => Toolkit.Instance.GetSerializer(cached.Property.PropertyType),
                LazyThreadSafetyMode.ExecutionAndPublication
            );
        }
        void AddIfMissing((string, Type) key, CachedMember member)
            => fallbackEntries.Add((key, member));
    }

    /// <inheritdoc/>
    public override void Serialize(ref T obj, MetaStream stream, MetaClassType? type = null)
    {
        MetaClass? description = stream.GetMetaClass(typeof(T));

        if (description is null || !description.ClassType.IsSerialized())
        {
            if (description is null)
                Toolkit.Instance.Logger.LogWarning(
                    $"[DefaultClassSerializer] No description available for {type?.FullTypeName} / {typeof(T).Name}.");

            return;
        }

        stream.BeginObject(typeof(T).Name);
        // Add this class to the metaclass header.
        if (stream is { Mode: MetaStreamMode.Write, Params.CanModifySerializedClassesList: true })
        {
            stream.AddVersionInfo(description);
        }

        stream.DefaultSectionDepth++;

        // Loop through the metaclass's properties/members
        foreach (MetaMember propDesc in description.Members.Where(propDesc => propDesc.IsSerialized()))
        {
            CachedMember? cached = ResolveMember(propDesc);

            if (propDesc.Type.IsBlocked())
                stream.BeginBlock();

            stream.Key(propDesc.MemberName);
            string pad = new(' ',  stream.DefaultSectionDepth);

            if (propDesc.Type.LinkingType.IsPrimitive || (propDesc.Type.LinkingType == typeof(string) ||
                                                          propDesc.Type.LinkingType == typeof(byte[]) ||
                                                          propDesc.Type.LinkingType == typeof(Symbol)))
            {
            }
            else
            {
                stream.BeginObject(propDesc.MemberName);
                Toolkit.Instance.Logger.LogInfo($"{pad}{propDesc.MemberName} {propDesc.Type.LinkingType} - [");
            }

            object? value = cached?.Getter(ref obj);

            MetaSerializer serializer = cached?.Serializer.Value ?? Toolkit.Instance.GetSerializer(cached?.Property.PropertyType ?? propDesc.Type.LinkingType);

            serializer.PreSerialize(ref value, stream, propDesc.Type);
            serializer.Serialize(ref value, stream, propDesc.Type);


            if (propDesc.Type.LinkingType.IsPrimitive || (propDesc.Type.LinkingType == typeof(string) ||
                                                          propDesc.Type.LinkingType == typeof(byte[]) ||
                                                          propDesc.Type.LinkingType == typeof(Symbol)))
            {
                Toolkit.Instance.Logger.LogInfo($"{pad}{propDesc.MemberName} {propDesc.Type.LinkingType} - {value}");
            }


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
                Toolkit.Instance.Logger.LogInfo($"{pad}]");
            }

            if (propDesc.Type.IsBlocked())
            {
                try
                {
                    stream.EndBlock();
                }
                catch
                {
                    //throw;
                }
            }
        }

        stream.DefaultSectionDepth--;
        stream.EndObject(typeof(T).Name);
    }

    public override void PreSerialize(ref T? obj, MetaStream stream, MetaClassType? type = null)
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

        // // 2. Enum/flag fallback: the member's type is an int, but also an enum
        // if ((propDesc.IsEnumType() || propDesc.IsFlagType()) && propDesc.Type.LinkingType.IsPrimitive)
        // {
        //     CachedMember? cachedAlt = MemberCache
        //         .FirstOrDefault(kvp => kvp.Key.Item1 == propDesc.MemberName && kvp.Value.Property.PropertyType.IsEnum)
        //         .Value;
        //     if (cachedAlt != null)
        //         return cachedAlt;
        // }
        //
        // // 5. int<->Flags
        // if (propDesc.Type.LinkingType == typeof(int))
        // {
        //     if (MemberCache.TryGetValue((propDesc.MemberName, typeof(Flags)), out CachedMember? cachedAlt))
        //         return cachedAlt;
        // }
        //
        // // Final: First available
        // if (propDesc.Type.LinkingType == typeof(int) || propDesc.Type.LinkingType == typeof(uint))
        // {
        //     CachedMember? cachedAltFinal = MemberCache
        //         .FirstOrDefault(kvp => kvp.Key.Item1 == propDesc.MemberName)
        //         .Value;
        //     if (cachedAltFinal != null)
        //         return cachedAltFinal;
        // }

        var inheritanceMatch = MemberCache
            .FirstOrDefault(kvp =>
                kvp.Key.Item1 == propDesc.MemberName && propDesc.Type.LinkingType.IsAssignableFrom(kvp.Value.Property.PropertyType)
                );

        if (inheritanceMatch.Value != null)
            return inheritanceMatch.Value;

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
        var instance = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
        var propertyAccess = Expression.Property(instance, prop);
        var convert = Expression.Convert(propertyAccess, typeof(object));
        return Expression.Lambda<FuncRefGetter<T>>(convert, instance).Compile();
    }

    private static ActionRefSetter<T> CreateSetterDelegate(PropertyInfo prop)
    {
        if (!prop.CanWrite)
            return (ref T obj, object? value) => { };

        var instance = Expression.Parameter(typeof(T).MakeByRefType(), "obj");
        var value = Expression.Parameter(typeof(object), "val");
        var castValue = Expression.Convert(value, prop.PropertyType);
        var assign = Expression.Assign(Expression.Property(instance, prop), castValue);
        return Expression.Lambda<ActionRefSetter<T>>(assign, instance, value).Compile();
    }

    private class CachedMember
    {
        public PropertyInfo Property = null!;
        public FuncRefGetter<T> Getter = null!;
        public ActionRefSetter<T> Setter = null!;
        public Lazy<MetaSerializer> Serializer = null!;
    }
}

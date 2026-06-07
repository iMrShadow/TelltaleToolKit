using TelltaleToolKit.Meta.Reflection;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

// Collection serializers are lazily initialized during runtime.
[MetaSerializer(typeof(ListSerializer<>), typeof(List<>))]
public sealed class ListSerializer<T> : MetaSerializer<List<T>>
{
    private MetaSerializer<T> _itemDataSerializer = null!;

    public override void Initialize(MetaSerializerSelector selector)
    {
        _itemDataSerializer = selector.GetSerializer<T>();
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref List<T>? obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Read)
        {
            if (obj is null)
                obj = [];
            else
                obj.Clear();
        }
    }

    /// <inheritdoc/>
    public override void Serialize(ref List<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            stream.Write(obj.Count);

            foreach (T entry in obj)
            {
                _itemDataSerializer.Serialize(entry, stream);
            }
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            int count = stream.ReadInt32();
            obj.Capacity = count;

            for (var i = 0; i < count; i++)
            {
                var value = default(T);
                _itemDataSerializer.PreSerialize(ref value, stream, type);
                _itemDataSerializer.Serialize(ref value, stream, type);

                obj.Add(value);
            }
        }
    }
}

[MetaSerializer(typeof(HashSetSerializer<>), typeof(HashSet<>))]
public sealed class HashSetSerializer<T> : MetaSerializer<HashSet<T>>
{
    private MetaSerializer<T> _itemDataSerializer = null!;

    public override void Initialize(MetaSerializerSelector selector)
    {
        _itemDataSerializer = selector.GetSerializer<T>();
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref HashSet<T>? obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Read)
        {
            if (obj is null)
                obj = [];
            else
                obj.Clear();
        }
    }

    /// <inheritdoc/>
    public override void Serialize(ref HashSet<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            stream.Write(obj.Count);

            foreach (T entry in obj)
            {
                _itemDataSerializer.Serialize(entry, stream);
            }
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            int count = stream.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var value = default(T);
                _itemDataSerializer.PreSerialize(ref value, stream);
                _itemDataSerializer.Serialize(ref value, stream);

                obj.Add(value);
            }
        }
    }
}

[MetaSerializer(typeof(DictionarySerializer<,>), typeof(Dictionary<,>))]
public sealed class DictionarySerializer<TKey, TValue> : MetaSerializer<Dictionary<TKey, TValue>>
    where TKey : notnull
{
    private MetaSerializer<TKey> _keySerializer = null!;
    private MetaSerializer<TValue> _valueSerializer = null!;

    /// <inheritdoc/>
    public override void PreSerialize(ref Dictionary<TKey, TValue>? obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Read)
        {
            if (obj is null)
                obj = new Dictionary<TKey, TValue>();
            else
                obj.Clear();
        }
    }

    public override void Initialize(MetaSerializerSelector selector)
    {
        _keySerializer = selector.GetSerializer<TKey>();
        _valueSerializer = selector.GetSerializer<TValue>();
    }

    /// <inheritdoc/>
    public override void Serialize(ref Dictionary<TKey, TValue> obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            stream.Write(obj.Count);

            foreach (KeyValuePair<TKey, TValue> item in obj)
            {
                _keySerializer.Serialize(item.Key, stream);
                _valueSerializer.Serialize(item.Value, stream);
            }
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            int count = stream.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var key = default(TKey);
                var value = default(TValue);
                _keySerializer.PreSerialize(ref key, stream, type);
                _keySerializer.Serialize(ref key, stream, type);
                _valueSerializer.PreSerialize(ref value, stream, type);
                _valueSerializer.Serialize(ref value, stream, type);
                obj.TryAdd(key!, value!);
            }
        }
    }
}

public sealed class ArraySerializer<T> : MetaSerializer<T[]>
{
    private MetaSerializer<T> _itemDataSerializer = null!;

    public override void PreSerialize(ref T[]? obj, MetaStream stream, MetaClassType? type = null)
    {
        if (type is null) return;

        int required = type.ArgNum;

        if (obj is null)
        {
            obj = new T[required];
        }
        else if (obj.Length != required)
        {
            var newArray = new T[required];
            // Copy as many as fit, preserving existing data when possible
            int copyCount = Math.Min(obj.Length, required);
            Array.Copy(obj, newArray, copyCount);
            obj = newArray;
        }
    }

    public override void Initialize(MetaSerializerSelector selector)
    {
        _itemDataSerializer = selector.GetSerializer<T>();
    }

    public override void Serialize(ref T[] obj, MetaStream stream, MetaClassType? type = null)
    {
        int length = type?.ArgNum ?? obj.Length;
        for (int i = 0; i < length; ++i)
        {
            _itemDataSerializer.PreSerialize(ref obj[i], stream);
            _itemDataSerializer.Serialize(ref obj[i], stream);
        }
    }
}

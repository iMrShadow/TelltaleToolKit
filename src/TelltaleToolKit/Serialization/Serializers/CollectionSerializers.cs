using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.Serialization.Serializers;

// Collection serializers are lazily initialized during runtime.
[MetaClassSerializerGlobal(typeof(ListSerializer<>), typeof(List<>))]
public sealed class ListSerializer<T> : MetaClassSerializer<List<T>>
{
    private MetaClassSerializer<T> _itemDataClassSerializer = null!;

    public override void Initialize(MetaClassSerializerSelector selector)
    {
        _itemDataClassSerializer = selector.GetSerializer<T>();
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref List<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream is MetaStreamReader streamReader)
        {
            if (obj is null)
                obj = [];
            else
                obj.Clear();
        }
    }

    /// <inheritdoc/>
    public override void Serialize(ref List<T> obj, MetaStream stream)
    {
        if (stream is MetaStreamWriter streamWriter)
        {
            streamWriter.Write(obj.Count);

            foreach (T entry in obj)
            {
                _itemDataClassSerializer.Serialize(entry, stream);
            }
        }
        else if (stream is MetaStreamReader streamReader)
        {
            int count = streamReader.ReadInt32();
            obj.Capacity = count;

            for (var i = 0; i < count; i++)
            {
                var value = default(T);
                _itemDataClassSerializer.PreSerialize(ref value, stream);
                _itemDataClassSerializer.Serialize(ref value, stream);

                obj.Add(value);
            }
        }
    }
}

[MetaClassSerializerGlobal(typeof(HashSetSerializer<>), typeof(HashSet<>))]
public sealed class HashSetSerializer<T> : MetaClassSerializer<HashSet<T>>
{
    private MetaClassSerializer<T> _itemDataClassSerializer = null!;

    public override void Initialize(MetaClassSerializerSelector selector)
    {
        _itemDataClassSerializer = selector.GetSerializer<T>();
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref HashSet<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream is MetaStreamReader streamReader)
        {
            if (obj is null)
                obj = [];
            else
                obj.Clear();
        }
    }


    /// <inheritdoc/>
    public override void Serialize(ref HashSet<T> obj, MetaStream stream)
    {
        if (stream is MetaStreamWriter streamWriter)
        {
            streamWriter.Write(obj.Count);

            foreach (T entry in obj)
            {
                _itemDataClassSerializer.Serialize(entry, stream);
            }
        }
        else if (stream is MetaStreamReader streamReader)
        {
            int count = streamReader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var value = default(T);
                _itemDataClassSerializer.PreSerialize(ref value, stream);
                _itemDataClassSerializer.Serialize(ref value, stream);

                obj.Add(value);
            }
        }
    }
}

[MetaClassSerializerGlobal(typeof(DictionarySerializer<,>), typeof(Dictionary<,>))]
public sealed class DictionarySerializer<TKey, TValue> : MetaClassSerializer<Dictionary<TKey, TValue>>
    where TKey : notnull
{
    private MetaClassSerializer<TKey> _keyClassSerializer = null!;
    private MetaClassSerializer<TValue> _valueClassSerializer = null!;

    /// <inheritdoc/>
    public override void PreSerialize(ref Dictionary<TKey, TValue> obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream is MetaStreamReader streamReader)
        {
            if (obj is null)
                obj = [];
            else
                obj.Clear();
        }
    }

    public override void Initialize(MetaClassSerializerSelector selector)
    {
        _keyClassSerializer = selector.GetSerializer<TKey>();
        _valueClassSerializer = selector.GetSerializer<TValue>();
    }

    /// <inheritdoc/>
    public override void Serialize(ref Dictionary<TKey, TValue> obj, MetaStream stream)
    {
        PreSerialize(ref obj, stream, null);

        if (stream is MetaStreamWriter streamWriter)
        {
            streamWriter.Write(obj.Count);

            foreach (KeyValuePair<TKey, TValue> item in obj)
            {
                _keyClassSerializer.Serialize(item.Key, stream);
                _valueClassSerializer.Serialize(item.Value, stream);
            }
        }
        else if (stream is MetaStreamReader streamReader)
        {
            int count = streamReader.ReadInt32();

            for (var i = 0; i < count; i++)
            {
                var key = default(TKey);
                var value = default(TValue);
                _keyClassSerializer.PreSerialize(ref key, stream);
                _keyClassSerializer.Serialize(ref key, stream);
                _valueClassSerializer.PreSerialize(ref value, stream);
                _valueClassSerializer.Serialize(ref value, stream);
                obj.Add(key, value);
            }
        }
    }
}

public sealed class ArraySerializer<T> : MetaClassSerializer<T[]>
{
    private MetaClassSerializer<T> _itemDataClassSerializer = null!;

    public override void Initialize(MetaClassSerializerSelector selector)
    {
        _itemDataClassSerializer = selector.GetSerializer<T>();
    }

    /// <inheritdoc/>
    public override void Serialize(ref T[] obj, MetaStream stream)
    {
        for (var i = 0; i < obj.Length; ++i)
        {
            _itemDataClassSerializer.Serialize(ref obj[i], stream);
        }
    }
}
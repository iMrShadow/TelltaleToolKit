using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Properties;

[MetaSerializer(typeof(Serializer))]
public class PropertySet
{
    // Starts appearing later
    [MetaMember("mPropVersion")]
    public int PropVersion { get; set; }

    [MetaMember("mPropertyFlags")]
    public Flags PropertyFlags { get; set; }

    // Disappears
    [MetaMember("mParentList")]
    public List<Handle<PropertySet>> ParentList { get; set; } = new();

    public Dictionary<Symbol, PropertyEntry> Properties = new();

    // New: store a small wrapper per-property so we can remember the MetaClassType used for each value
    public struct PropertyEntry
    {
        public object? Value;
        public MetaClassType? MetaType;

        public PropertyEntry(object? value, MetaClassType? metaType = null)
        {
            Value = value;
            MetaType = metaType;
        }
    }

    public PropertySet ParentProperties;

    public class Serializer : MetaSerializer<PropertySet>
    {
        private static readonly MetaClassSerializer<PropertySet> s_metaClassSerializer = new();

        public override void PreSerialize(ref PropertySet? obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new PropertySet();
            }
        }

        public override void Serialize(ref PropertySet obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            stream.BeginBlock();

            // Attempt 1
            if (obj.PropertyFlags.Has(0x4000000))
            {
                stream.EndBlock();
                return;
            }

            if (stream.Mode is MetaStreamMode.Write)
            {
                // Write ParentList when PropVersion > 0
                if (obj.PropVersion > 0)
                {
                    int parentCount = obj.ParentList.Count;
                    stream.Write(parentCount);

                    MetaSerializer<Handle<PropertySet>> parentSerializer =
                        Toolkit.Instance.GetSerializer<Handle<PropertySet>>();
                    for (var i = 0; i < parentCount; i++)
                    {
                        // Use a local variable so we can pass by ref into serializer
                        Handle<PropertySet> parent = obj.ParentList[i];
                        parentSerializer.PreSerialize(ref parent, stream);
                        parentSerializer.Serialize(ref parent, stream);
                        // Update back in case the serializer modified the handle (consistent with reader logic)
                        obj.ParentList[i] = parent;
                    }
                }

                // Maintain backwards-compat for PropVersion == 1: reader does EndBlock(); BeginBlock();
                if (obj.PropVersion == 1)
                {
                    stream.EndBlock();
                    stream.BeginBlock();
                }

                // Group properties by their MetaClassType so we can write number-of-types block
                // Determine MetaClassType for each property's value
                var groups =
                    new Dictionary<MetaClassType, List<KeyValuePair<Symbol, PropertyEntry>>>();

                foreach ((Symbol key, PropertyEntry entry) in obj.Properties)
                {
                    MetaClassType? typeSymbol = entry.MetaType;

                    if (typeSymbol is null)
                    {
                        continue;
                    }

                    if (!groups.TryGetValue(typeSymbol, out List<KeyValuePair<Symbol, PropertyEntry>>? list))
                    {
                        list = new List<KeyValuePair<Symbol, PropertyEntry>>();
                        groups[typeSymbol] = list;
                    }

                    list.Add(new KeyValuePair<Symbol, PropertyEntry>(key,
                        new PropertyEntry(entry.Value, typeSymbol)));
                }

                // Write number of distinct meta types
                stream.Write(groups.Count);

                // Stable order: order by the MetaClassType's identity marker (e.g., its symbol or linking-type name)
                // Use MetaClassType.Symbol if present (prefer preserving the meta-symbol ordering), otherwise LinkingType.FullName
                IOrderedEnumerable<KeyValuePair<MetaClassType, List<KeyValuePair<Symbol, PropertyEntry>>>> ordered =
                    groups.OrderBy(g =>
                    {
                        // Prefer meta-symbol if available (so ordering depends on meta identity)
                        Symbol? sym = g.Key.Symbol; // assume MetaClassType has .Symbol
                        if (sym != null)
                            return sym.ToString();
                        Type? lt = g.Key.LinkingType;
                        return lt?.FullName ?? string.Empty;
                    }, StringComparer.Ordinal);

                foreach ((MetaClassType typeSymbol, List<KeyValuePair<Symbol, PropertyEntry>> entries) in ordered)
                {
                    stream.Write(typeSymbol);
                    stream.Write(entries.Count);

                    MetaSerializer typeSerializer =
                        Toolkit.Instance.GetSerializer(typeSymbol.LinkingType);

                    foreach ((Symbol key, PropertyEntry value) in entries)
                    {
                        stream.Write(key);

                        object? propertyValue = value.Value;

                        typeSerializer.PreSerialize(ref propertyValue, stream, typeSymbol);
                        typeSerializer.Serialize(ref propertyValue, stream);

                        // Note: we intentionally don't mutate the stored PropertyEntry.Value here.
                        // If serializer changes the in-memory object and you want that persisted, set it before calling Serialize.
                    }
                }
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                if (obj.PropVersion > 1)
                {
                    obj.ParentList.Capacity = stream.ReadInt32();

                    for (var i = 0; i < obj.ParentList.Capacity; i++)
                    {
                        var parent = new Handle<PropertySet>();
                        Toolkit.Instance.GetSerializer<Handle<PropertySet>>().Serialize(ref parent, stream);
                        obj.ParentList.Add(parent);
                    }
                }

                if (obj.PropVersion == 1 &&
                    !stream.GetMetaClass(typeof(PropertySet))!.ContainsMember("mParentList"))
                {
                    stream.EndBlock();
                    stream.BeginBlock();
                }

                int numTypes = stream.ReadInt32();
                for (var i = 0; i < numTypes; i++)
                {
                    // The type of the class
                    MetaClassType? typeSymbol = stream.ReadMetaClassType();
                    if (typeSymbol?.Symbol.Crc64 == 0)
                        continue;
                    // The number of times that type has been serialized
                    int numOfType = stream.ReadInt32();

                    if (typeSymbol is null)
                        throw new InvalidOperationException("[PropertySet] Type symbol is null");

                    MetaSerializer typeSerializer = Toolkit.Instance.GetSerializer(typeSymbol.LinkingType);

                    for (var j = 0; j < numOfType; j++)
                    {
                        // The property key
                        Symbol propertyKey = stream.ReadSymbol();

                        object? propertyValue = null;

                        typeSerializer.PreSerialize(ref propertyValue, stream, typeSymbol);
                        typeSerializer.Serialize(ref propertyValue, stream, typeSymbol);
                        obj.Properties[propertyKey] = new PropertyEntry(propertyValue, typeSymbol);
                    }
                }
            }

            bool embeddedParentProps = obj.PropertyFlags.Has(1024);

            if (embeddedParentProps)
            {
                stream.Serialize(ref obj.ParentProperties);
            }

            stream.EndBlock();
        }
    }

    public void EmbedParentProperties()
    {
        PropertyFlags |= 1024;
    }

    public object? GetProperty(string propertyName)
    {
        var symbol = Symbol.FromName(propertyName);

        Properties.TryGetValue(symbol, out PropertyEntry obj);
        {
            return obj.Value;
        }
    }

    public object? GetProperty(ulong crc64)
    {
        Properties.TryGetValue(Symbol.FromCrc64(crc64), out PropertyEntry obj);
        {
            return obj.Value;
        }
    }

    public T? GetProperty<T>(string propertyName)
    {
        var symbol = Symbol.FromName(propertyName);

        Properties.TryGetValue(symbol, out PropertyEntry obj);
        {
            return obj.Value is T value ? value : default;
        }
    }

    public T? GetProperty<T>(ulong crc64)
    {
        Properties.TryGetValue(Symbol.FromCrc64(crc64), out PropertyEntry obj);
        {
            return obj.Value is T value ? value : default;
        }
    }

    public object? GetProperty(Symbol symbol)
    {
        return GetProperty(symbol.Crc64);
    }

    public T? GetProperty<T>(Symbol symbol)
    {
        return GetProperty<T>(symbol.Crc64);
    }

    internal class KeyInfo
    {
    }
}

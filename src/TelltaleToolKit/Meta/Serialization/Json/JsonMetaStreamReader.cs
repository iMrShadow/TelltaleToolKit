using System.Text.Json;

namespace TelltaleToolKit.Meta.Serialization.Json;

/// <summary>
///     Reads a JSON file written by <see cref="JsonMetaStreamWriter"/>.
///     The file must be an array with two elements: header object and payload object.
/// </summary>
public sealed class JsonMetaStreamReader : MetaStream
{
    private readonly JsonDocument? _document;
    private readonly JsonElement _current;
    private readonly Stack<ReaderFrame> _frames = new();
    private bool _closed;

    // Tracks whether the next value read should be preceded by a key.
    // In JSON, keys are consumed by Key(), values by Serialize().
    private bool _expectKey;

    public JsonMetaStreamReader(Stream inputStream, MetaStreamParams @params)
    {
        Stream inputStream1 = inputStream ?? throw new ArgumentNullException(nameof(inputStream));
        Params = @params;

        // Load the entire JSON document (we need random access for header & payload)
        _document = JsonDocument.Parse(inputStream1);
        JsonElement root = _document.RootElement;

        // Expect an array with exactly two elements
        if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() != 2)
            throw new InvalidDataException("JSON root must be an array of [header, payload].");

        JsonElement header = root[0];
        JsonElement payload = root[1];

        // Parse header to populate MetaStreamParams.VersionInfo
        ParseHeader(header);

        // Start reading the payload
        _current = payload;
        _frames.Clear();
        _expectKey = false;
    }

    private void ParseHeader(JsonElement header)
    {
        if (!header.TryGetProperty("_metaVersionInfo", out JsonElement versionInfoElement))
            return; // no version info, ignore

        if (!versionInfoElement.TryGetProperty("mVersionInfo", out JsonElement arrayElement))
            return;

        if (arrayElement.ValueKind != JsonValueKind.Array)
            return;

        var versionInfoList = new List<MetaVersionInfo>();
        foreach (JsonElement item in arrayElement.EnumerateArray())
        {
            if (!item.TryGetProperty("mTypeSymbolCrc", out JsonElement crcElem) ||
                !item.TryGetProperty("mVersionCrc", out JsonElement versionCrcElem))
                continue;

            ulong typeCrc = crcElem.GetUInt64();
            uint versionCrc = versionCrcElem.GetUInt32();

            versionInfoList.Add(new MetaVersionInfo { TypeSymbolCrc = typeCrc, VersionCrc = versionCrc });
        }

        Params.VersionInfo.Clear();
        Params.VersionInfo.AddRange(versionInfoList);
    }

    public override MetaStreamMode Mode => MetaStreamMode.Read;

    protected override bool SetSection(SectionType section)
    {
        return false;
    }

    public override bool BeginAsyncSection()
    {
        return false;
    }

    public override void EndAsyncSection()
    {
    }

    public override bool BeginDebugSection()
    {
        return false;
    }

    public override void EndDebugSection()
    {
    }

    public override void BeginObject(string name, bool isArray = false)
    {
        if (_frames.Count == 0)
        {
            // Root – we are already at the payload object, just push it
            _frames.Push(new ReaderFrame(_current, isArray));
            _expectKey = false;
            return;
        }

        ReaderFrame currentFrame = _frames.Peek();

        if (currentFrame.IsArray)
        {
            // Inside an array: read the next element (the array itself must be an object)
            JsonElement child = currentFrame.GetNextArrayElement();
            if (isArray)
            {
                if (child.ValueKind != JsonValueKind.Array)
                    throw new InvalidDataException("Expected JSON array.");
            }
            else
            {
                if (child.ValueKind != JsonValueKind.Object)
                    throw new InvalidDataException("Expected JSON object.");
            }

            _frames.Push(new ReaderFrame(child, isArray));
            _expectKey = false;
        }
        else
        {
            // Inside an object: consume a property with the given name
            if (!currentFrame.TryGetProperty(name, out JsonElement child))
                throw new InvalidDataException($"Property '{name}' not found in JSON object.");

            if (isArray)
            {
                if (child.ValueKind != JsonValueKind.Array)
                    throw new InvalidDataException($"Property '{name}' is not a JSON array.");
            }
            else
            {
                if (child.ValueKind != JsonValueKind.Object)
                    throw new InvalidDataException($"Property '{name}' is not a JSON object.");
            }

            _frames.Push(new ReaderFrame(child, isArray));
            _expectKey = false;
        }
    }

    public override void EndObject(string name)
    {
        if (_frames.Count == 0)
            throw new InvalidOperationException("EndObject called without matching BeginObject.");
        _frames.Pop();
        _expectKey = false;
    }

    public override void Key(string name)
    {
        if (_frames.Count == 0)
            throw new InvalidOperationException("Key called outside of an object.");
        ReaderFrame frame = _frames.Peek();
        if (frame.IsArray)
            return; // keys are ignored inside arrays (writer never calls Key there)

        // Verify that the next property name matches the expected name
        if (!frame.TryGetNextProperty(out string actualName))
            throw new InvalidDataException("No more properties in current object.");

        if (actualName != name)
            throw new InvalidDataException($"Expected property '{name}' but found '{actualName}'.");

        _expectKey = true;
    }

    private void EnsureValue()
    {
        if (!_expectKey)
            throw new InvalidOperationException("Key must be called before reading a value.");
        _expectKey = false;
    }

    private JsonElement GetCurrentValue()
    {
        ReaderFrame frame = _frames.Peek();
        if (frame.IsArray)
            return frame.GetNextArrayElement();
        else
            return frame.GetCurrentValue(); // after Key, we already advanced to the value
    }

    public override void Serialize(ref bool value)
    {
        EnsureValue();
        value = GetCurrentValue().GetBoolean();
    }

    public override void Serialize(ref float value)
    {
        EnsureValue();
        value = (float)GetCurrentValue().GetDouble();
    }

    public override void Serialize(ref double value)
    {
        EnsureValue();
        value = GetCurrentValue().GetDouble();
    }

    public override void Serialize(ref short value)
    {
        EnsureValue();
        value = GetCurrentValue().GetInt16();
    }

    public override void Serialize(ref int value)
    {
        EnsureValue();
        value = GetCurrentValue().GetInt32();
    }

    public override void Serialize(ref long value)
    {
        EnsureValue();
        value = GetCurrentValue().GetInt64();
    }

    public override void Serialize(ref ushort value)
    {
        EnsureValue();
        value = GetCurrentValue().GetUInt16();
    }

    public override void Serialize(ref uint value)
    {
        EnsureValue();
        value = GetCurrentValue().GetUInt32();
    }

    public override void Serialize(ref ulong value)
    {
        EnsureValue();
        value = GetCurrentValue().GetUInt64();
    }

    public override void Serialize(ref string value)
    {
        EnsureValue();
        value = GetCurrentValue().GetString() ?? string.Empty;
    }

    public override void Serialize(ref char value)
    {
        EnsureValue();
        string s = GetCurrentValue().GetString() ?? string.Empty;
        value = s.Length > 0 ? s[0] : '\0';
    }

    public override void Serialize(ref byte value)
    {
        EnsureValue();
        value = GetCurrentValue().GetByte();
    }

    public override void Serialize(ref sbyte value)
    {
        EnsureValue();
        value = GetCurrentValue().GetSByte();
    }

    public override void Serialize(byte[] values, int offset, int count)
    {
        EnsureValue();
        byte[] base64 = GetCurrentValue().GetBytesFromBase64();
        if (base64.Length < offset + count)
            throw new InvalidDataException("Base64 data too short.");
        Array.Copy(base64, offset, values, 0, count);
    }


    public override void BeginBlock()
    {
    }

    public override void EndBlock()
    {
    }

    public override void Close()
    {
        if (_closed) return;
        _document?.Dispose();
        _closed = true;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && !_closed)
            Close();
        base.Dispose(disposing);
    }


    private class ReaderFrame
    {
        private JsonElement Element { get; }
        public bool IsArray { get; }
        private readonly IEnumerator<JsonProperty>? _propertyEnumerator;
        private readonly IEnumerator<JsonElement>? _arrayEnumerator;
        private JsonElement _currentValue; // after Key() we store the value

        public ReaderFrame(JsonElement element, bool isArray)
        {
            Element = element;
            IsArray = isArray;
            if (IsArray)
                _arrayEnumerator = element.EnumerateArray().GetEnumerator();
            else
                _propertyEnumerator = element.EnumerateObject().GetEnumerator();
        }

        public bool TryGetProperty(string name, out JsonElement child)
        {
            if (IsArray)
                throw new InvalidOperationException("Cannot get property by name from an array.");
            return Element.TryGetProperty(name, out child);
        }

        public bool TryGetNextProperty(out string name)
        {
            if (IsArray || _propertyEnumerator == null)
                throw new InvalidOperationException("Not in an object.");
            if (_propertyEnumerator.MoveNext())
            {
                name = _propertyEnumerator.Current.Name;
                _currentValue = _propertyEnumerator.Current.Value;
                return true;
            }

            name = null!;
            return false;
        }

        public JsonElement GetCurrentValue() => _currentValue;

        public JsonElement GetNextArrayElement()
        {
            if (!IsArray || _arrayEnumerator == null)
                throw new InvalidOperationException("Not in an array.");
            if (!_arrayEnumerator.MoveNext())
                throw new InvalidDataException("Unexpected end of JSON array.");
            return _arrayEnumerator.Current;
        }
    }
}

using System.Text.Json;

namespace TelltaleToolKit.Meta.Serialization.Json;

/// <summary>
///     Writes a MetaStream as human-readable JSON, following the same two-part layout
///     as Telltale Tool Lib's <c>MetaStream_JSON with minor improvements to make visualization better.</c>:
///     <list type="number">
///         <item>A header object (watermark, game ID, version info).</item>
///         <item>A payload object containing the serialized asset data.</item>
///     </list>
/// </summary>
public sealed class JsonMetaStreamWriter : MetaStream
{
    private readonly Stream _outputStream;
    private bool _closed;

    // The payload writer — accumulates the asset data between Open and Close.
    private readonly MemoryStream _payloadBuffer = new(0x4000);
    private readonly Utf8JsonWriter _payload;
    private bool _keyPending; // true after Key() until a value is writte

    // Tracks whether the current JSON container is an array (true) or object (false).
    // Mirrors C++ mbObjectIsArrayStack.
    private readonly Stack<bool> _objectIsArrayStack = new();

    public JsonMetaStreamWriter(Stream outputStream, MetaStreamParams @params)
    {
        _outputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
        Params = @params;

        _payload = new Utf8JsonWriter(_payloadBuffer, new JsonWriterOptions { Indented = true });
        _payload.WriteStartObject(); // root payload object — closed in Close()
        _keyPending = false;
        // JSON has no real sections; we still need a non-null stream so base-class
        // helpers (GetRemainingSectionBytes, etc.) don't throw.
        Sections[(int)SectionType.Default].Stream = _payloadBuffer;
        _currentSection = SectionType.Default;
    }

    public override MetaStreamMode Mode => MetaStreamMode.Write;

    protected override bool SetSection(SectionType section)
    {
        // Intentional no-op: JSON folds all sections into the payload object via
        // BeginAsyncSection / BeginDebugSection named sub-objects.
        _currentSection = section;
        return true;
    }

    public override bool BeginAsyncSection()
    {
        if (Params.StreamVersion < 4) return false;
        if (_currentSection is SectionType.Async) return false;

        _payload.WritePropertyName("Async Data");
        _payload.WriteStartObject();
        _currentSection = SectionType.Async;

        return true;
    }

    public override void EndAsyncSection()
    {
        if (_currentSection is not SectionType.Async) return;

        _payload.WriteEndObject();
        _currentSection = SectionType.Default;
    }

    public override bool BeginDebugSection()
    {
        if (Params.StreamVersion < 4) return false;
        if (_currentSection is SectionType.Debug) return false;

        _payload.WritePropertyName("Debug Data");
        _payload.WriteStartObject();
        _currentSection = SectionType.Debug;

        return true;
    }

    public override void EndDebugSection()
    {
        if (_currentSection is not SectionType.Debug) return;

        _payload.WriteEndObject();
        _currentSection = SectionType.Default;
    }

    public override void BeginObject(string name, bool isArray = false)
    {
        EnsureKey();
        _payload.WritePropertyName(name);
        if (isArray)
            _payload.WriteStartArray();
        else
            _payload.WriteStartObject();
        _objectIsArrayStack.Push(isArray);
        _keyPending = false; // the key has been consumed
    }

    public override void EndObject(string name)
    {
        if (_objectIsArrayStack.Count == 0)
            throw new InvalidOperationException("EndObject called without a matching BeginObject.");
        bool wasArray = _objectIsArrayStack.Pop();
        if (wasArray)
            _payload.WriteEndArray();
        else
            _payload.WriteEndObject();
    }

    /// <summary>
    ///     Emits a JSON property key. Only has effect when the current container is an object.
    ///     Mirrors <c>MetaStream_JSON::Key</c>.
    /// </summary>
    public override void Key(string name)
    {
        // Inside an array context a key makes no sense — silently ignore,
        // matching the C++ NeedsKey() guard.
        bool insideArray = _objectIsArrayStack.Count > 0 && _objectIsArrayStack.Peek();
        if (!insideArray)
        {
            _payload.WritePropertyName(name);
            _keyPending = true;
        }
    }

    public override void BeginBlock()
    {
    }

    public override void EndBlock()
    {
    }

    private void EnsureKey()
    {
        // Only need a key if we're inside an object (not an array)
        if (_objectIsArrayStack.Count > 0 && !_objectIsArrayStack.Peek())
        {
            if (!_keyPending)
            {
                // No key provided – write a default property name
                _payload.WritePropertyName("__default");
            }

            // Key is consumed (either the real one or the default)
            _keyPending = false;
        }
    }

    public override void Serialize(ref bool value)
    {
        EnsureKey();
        _payload.WriteBooleanValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref float value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref double value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref short value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref int value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref long value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref ushort value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref uint value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref ulong value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref string value)
    {
        EnsureKey();
        _payload.WriteStringValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref char value)
    {
        EnsureKey();
        _payload.WriteStringValue(value.ToString());
        _keyPending = false;
    }

    public override void Serialize(ref byte value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    public override void Serialize(ref sbyte value)
    {
        EnsureKey();
        _payload.WriteNumberValue(value);
        _keyPending = false;
    }

    /// <summary>
    ///     Binary blobs are Base64-encoded, matching C++ <c>WriteData</c>.
    /// </summary>
    public override void Serialize(byte[] values, int offset, int count)
    {
        EnsureKey();
        _payload.WriteBase64StringValue(values.AsSpan(offset, count));
        _keyPending = false;
    }

    public override void Close()
    {
        if (_closed) return;

        // Finish the root payload object opened in the constructor.
        _payload.WriteEndObject();
        _payload.Flush();
        _payloadBuffer.Seek(0, SeekOrigin.Begin);

        using JsonDocument payloadDoc = JsonDocument.Parse(_payloadBuffer);
        using MemoryStream finalBuffer = new(0x1000);

        using (Utf8JsonWriter final = new(finalBuffer, new JsonWriterOptions { Indented = true }))
        {
            final.WriteStartArray();

            final.WriteStartObject();
            // Version info — type CRC, human-readable type name, version CRC.
            final.WriteString("Watermark",
                $"Converted to JSON by TelltaleToolKit v{typeof(MetaStream).Assembly.GetName().Version}");
            final.WritePropertyName("_metaVersionInfo");
            final.WriteStartObject();
            {
                // Skip entries whose type cannot be resolved, matching C++ behaviour.
                List<MetaVersionInfo> resolvable = Params.VersionInfo
                    .Where(v => v.IsTypeRegistered())
                    .ToList();

                final.WriteNumber("mSize", resolvable.Count);
                final.WritePropertyName("mVersionInfo");
                final.WriteStartArray();
                foreach (MetaVersionInfo v in resolvable)
                {
                    final.WriteStartObject();
                    final.WriteNumber("mTypeSymbolCrc", v.TypeSymbolCrc); // ulong → JSON number
                    final.WriteString("Type Name", v.GetMetaClassType()!.FullTypeName);
                    final.WriteNumber("mVersionCrc", v.VersionCrc);
                    final.WriteEndObject();
                }

                final.WriteEndArray();
            }
            final.WriteEndObject();
            final.WriteEndObject();

            payloadDoc.RootElement.WriteTo(final);

            final.WriteEndArray(); // end header root
        }

        if (_outputStream.CanSeek)
        {
            _outputStream.SetLength(0); // remove any existing content
            _outputStream.Seek(0, SeekOrigin.Begin);
        }

        finalBuffer.Seek(0, SeekOrigin.Begin);
        finalBuffer.CopyTo(_outputStream);

        _closed = true;
        Dispose(true);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _payload.Dispose();
            _payloadBuffer.Dispose();
        }

        base.Dispose(disposing);
    }
}

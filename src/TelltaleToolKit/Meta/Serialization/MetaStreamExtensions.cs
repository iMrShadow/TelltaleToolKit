using System.Runtime.CompilerServices;
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Meta.Serialization;

public static class MetaStreamExtensions
{
    /// <summary>
    /// Reads a boolean value from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A boolean value read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBoolean(this MetaStream stream)
    {
        bool value = false;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 4-byte floating point value read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadSingle(this MetaStream stream)
    {
        float value = 0.0f;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads an 8-byte floating point value from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>An 8-byte floating point value read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble(this MetaStream stream)
    {
        double value = 0.0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 2-byte signed integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 2-byte signed integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ReadInt16(this MetaStream stream)
    {
        short value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 4-byte signed integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 4-byte signed integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int ReadInt32(this MetaStream stream)
    {
        int value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads an 8-byte signed integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>An 8-byte signed integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadInt64(this MetaStream stream)
    {
        long value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 2-byte unsigned integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 2-byte unsigned integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUInt16(this MetaStream stream)
    {
        ushort value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 4-byte unsigned integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 4-byte unsigned integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt32(this MetaStream stream)
    {
        uint value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads an 8-byte unsigned integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>An 8-byte unsigned integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadUInt64(this MetaStream stream)
    {
        ulong value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a string.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A string read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ReadString(this MetaStream stream)
    {
        string value = null!;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a Unicode character from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A Unicode character read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ReadChar(this MetaStream stream)
    {
        char value = '\0';
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads an unsigned byte integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>An unsigned byte read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(this MetaStream stream)
    {
        byte value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a signed byte from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A signed byte read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static sbyte ReadSByte(this MetaStream stream)
    {
        sbyte value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads the specified number of bytes.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="count"></param>
    /// <returns>A byte array containing the data read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte[] ReadBytes(this MetaStream stream, int count)
    {
        byte[] value = new byte[count];
        stream.Serialize(value, 0, count);
        return value;
    }

    /// <summary>
    /// Reads a string.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A string read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Symbol ReadSymbol(this MetaStream stream)
    {
        var value = Symbol.Empty;
        stream.Serialize(ref value);
        return value;
    }

    // TODO: Fix documentation
    /// <summary>
    /// Reads a MetaClassType.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A string read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaClassType? ReadMetaClassType(this MetaStream stream)
    {
        MetaClassType? value;
        if (stream.Params.StreamVersion >= 3) // mStreamVersion >= 3 uses hashed types
        {
            Symbol def = stream.ReadSymbol();
            value = MetaClassTypeRegistry.GetByHash(def.Crc64);

            if (value == null)
                Toolkit.Instance.Logger.LogError($"Unknown type symbol CRC64: {def.Crc64}");
        }
        else
        {
            string typeName = stream.ReadString();
            value = MetaClassTypeRegistry.GetByName(typeName);
        }

        return value;
    }

    /// <summary>
    /// Writes a boolean value to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The boolean value to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, bool value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 4-byte floating point value to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 4-byte floating point value to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, float value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes an 8-byte floating point value to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 8-byte floating point value to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, double value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 2-byte signed integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 2-byte signed integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, short value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 4-byte signed integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 4-byte signed integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, int value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes an 8-byte signed integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 8-byte signed integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, long value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 2-byte unsigned integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 2-byte unsigned integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, ushort value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 4-byte unsigned integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 4-byte unsigned integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, uint value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes an 8-byte unsigned integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 8-byte unsigned integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, ulong value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a string to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The string to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, string value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a Unicode character to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The Unicode character to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, char value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes an unsigned byte to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The unsigned byte to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, byte value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a signed byte to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The signed byte to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, sbyte value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a signed byte to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The signed byte to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, Symbol value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a byte array region to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="values">The byte array to write.</param>
    /// <param name="offset">The starting offset in values to write.</param>
    /// <param name="count">The number of bytes to write.</param>
    /// <returns>
    /// The stream.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, byte[] values, int offset, int count)
    {
        stream.Serialize(values, offset, count);
        return stream;
    }

    /// <summary>
    /// Writes a byte array region to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="values">The byte array to write.</param>
    /// <returns>
    /// The stream.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, byte[] values)
    {
        stream.Serialize(values, 0, values.Length);
        return stream;
    }

    // TODO: Fix documentation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStream Write(this MetaStream stream, MetaClassType value)
    {
        if (stream.Params.StreamVersion >= 3) // mStreamVersion >= 3 uses hashed types
        {
            stream.Write(value.Symbol.Crc64);
        }
        else
        {
            stream.Write(value.FullTypeName);
        }

        return stream;
    }

    /// <summary>
    /// Serializes or deserializes the given object <paramref name="obj"/> using the Telltale ToolKit context.
    /// </summary>
    /// <param name="obj">The object to serialize or deserialize.</param>
    /// <param name="stream">The stream to serialize or deserialize to.</param>
    /// <param name="type"></param>
    public static void Serialize<T>(this MetaStream stream, ref T obj, MetaClassType? type = null)
    {
        var serializer = Toolkit.Instance.GetSerializer<T>();

        serializer.PreSerialize(ref obj, stream, type);
        serializer.Serialize(ref obj, stream);
    }

    /// <summary>
    /// Reads exactly <paramref name="destination"/>.<see cref="Span{T}.Length"/> bytes from the stream into the span.
    /// </summary>
    public static void ReadBytes(this MetaStream stream, Span<byte> destination)
    {
        // Fallback: use a temporary array (one allocation per call)
        byte[] temp = new byte[destination.Length];
        stream.Serialize(temp, 0, temp.Length);
        temp.AsSpan().CopyTo(destination);
    }

    /// <summary>
    /// Writes a read‑only span of bytes to the stream.
    /// </summary>
    public static void WriteBytes(this MetaStream stream, ReadOnlySpan<byte> values)
    {
        // Fallback: copy to array and write
        byte[] temp = values.ToArray();
        stream.Serialize(temp, 0, temp.Length);
    }
}

using System.Runtime.CompilerServices;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization.Binary;

public static class MetaStreamExtensions
{
    /// <summary>
    /// Reads a boolean value from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A boolean value read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReadBoolean(this MetaStreamReader stream)
    {
        var value = false;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 4-byte floating point value from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 4-byte floating point value read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ReadSingle(this MetaStreamReader stream)
    {
        var value = 0.0f;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 8-byte floating point value from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 8-byte floating point value read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble(this MetaStreamReader stream)
    {
        var value = 0.0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 2-byte signed integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 2-byte signed integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static short ReadInt16(this MetaStreamReader stream)
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
    public static int ReadInt32(this MetaStreamReader stream)
    {
        var value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 8-byte signed integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 8-byte signed integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long ReadInt64(this MetaStreamReader stream)
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
    public static ushort ReadUInt16(this MetaStreamReader stream)
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
    public static uint ReadUInt32(this MetaStreamReader stream)
    {
        uint value = 0;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a 8-byte unsigned integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A 8-byte unsigned integer read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong ReadUInt64(this MetaStreamReader stream)
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
    public static string ReadString(this MetaStreamReader stream)
    {
        string value = null!;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a unicode character from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A unicode character read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char ReadChar(this MetaStreamReader stream)
    {
        var value = '\0';
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Reads a unsigned byte integer from the stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>An unsigned byte read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ReadByte(this MetaStreamReader stream)
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
    public static sbyte ReadSByte(this MetaStreamReader stream)
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
    public static byte[] ReadBytes(this MetaStreamReader stream, int count)
    {
        var value = new byte[count];
        stream.Serialize(value, 0, count);
        return value;
    }

    /// <summary>
    /// Reads a string.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <returns>A string read from the stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Symbol ReadSymbol(this MetaStreamReader stream)
    {
        var value = Symbol.DefaultSymbol;
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
    public static MetaClassType ReadMetaClassType(this MetaStreamReader stream)
    {
        MetaClassType value = MetaClassType.UninitializedClassType;
        stream.Serialize(ref value);
        return value;
    }

    /// <summary>
    /// Writes a boolean value to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The boolean value to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStreamWriter Write(this MetaStreamWriter stream, bool value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, float value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 8-byte floating point value to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 8-byte floating point value to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStreamWriter Write(this MetaStreamWriter stream, double value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, short value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, int value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 8-byte signed integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 8-byte signed integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStreamWriter Write(this MetaStreamWriter stream, long value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, ushort value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, uint value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a 8-byte unsigned integer to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The 8-byte unsigned integer to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStreamWriter Write(this MetaStreamWriter stream, ulong value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, string value)
    {
        stream.Serialize(ref value);
        return stream;
    }

    /// <summary>
    /// Writes a unicode character to the specified stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="value">The unicode character to write.</param>
    /// <returns>The stream.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStreamWriter Write(this MetaStreamWriter stream, char value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, byte value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, sbyte value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, Symbol value)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, byte[] values, int offset, int count)
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
    public static MetaStreamWriter Write(this MetaStreamWriter stream, byte[] values)
    {
        stream.Serialize(values, 0, values.Length);
        return stream;
    }
    
    // TODO: Fix documentation
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MetaStreamWriter Write(this MetaStreamWriter stream, MetaClassType value)
    {
        stream.Serialize(ref value);
        return stream;
    }
}
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace TelltaleToolKit.IO;

public static class Compression
{
    /// <summary>Compression algorithm to use when writing archive chunks.</summary>
    public enum Mode
    {
        /// <summary>No compression.</summary>
        None,

        /// <summary>Raw DEFLATE (no zlib header). Used by most Telltale games.</summary>
        Deflate,

        /// <summary>Zlib-wrapped DEFLATE. Used by some CSI and earlier titles.</summary>
        Zlib,

        /// <summary>
        ///     Oodle compression.
        /// </summary>
        Oodle
    }

    /// <summary>Reads exactly <c>buffer.Length</c> bytes from <paramref name="stream" />.</summary>
    internal static void ReadExact(Stream stream, byte[] buffer)
    {
        int total = 0;
        while (total < buffer.Length)
        {
            int read = stream.Read(buffer, total, buffer.Length - total);
            if (read == 0)
            {
                throw new EndOfStreamException(
                    $"Unexpected end of stream after {total} / {buffer.Length} bytes.");
            }

            total += read;
        }
    }

    public static byte[] Decompress(byte[] compressedData, Mode mode, int expectedSize = 0)
    {
        // This is a hack? In TWD:DE, 401_txmesh, there's a page which is the same size as the expected size (65535)
        // C#'s raw deflate fails.
        // ttarchext returns the same data:
        // https://github.com/infernokun/TelltaleGamesExtractionGUI/blob/fec9fc1a70b545bcc67062fedc3fe3f7cd0d3e1b/bin/Debug/net6.0-windows/ttarchext/ttarchext.c#L1512
        // If the data isn't actually compressed (e.g., same size as expected)
        if (expectedSize > 0 && compressedData.Length == expectedSize)
            return compressedData; // Already uncompressed

        using var input = new MemoryStream(compressedData, writable: false);
        using var output = expectedSize > 0 ? new MemoryStream(expectedSize) : new MemoryStream();

        try
        {
            Stream decompressor = mode switch
            {
                Mode.None => input, // No compression – just copy
                Mode.Deflate => new DeflateStream(input, CompressionMode.Decompress),
                Mode.Zlib => new InflaterInputStream(input), // from SharpZipLib
                Mode.Oodle => throw new NotSupportedException("Oodle not implemented"), // TODO: Add Oodle compression.
                _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
            };

            using (decompressor)
            {
                decompressor.CopyTo(output);
            }

            return output.ToArray();
        }
        catch (Exception ex)
        {
            throw new InvalidDataException($"Decompression failed using {mode}.", ex);
        }
    }

    /// <summary>
    ///     Compresses a single chunk using the specified algorithm.
    /// </summary>
    /// <returns>Number of compressed bytes written into <paramref name="outputBuffer" />.</returns>
    public static int Compress(ReadOnlySpan<byte> input, Mode algorithm, byte[] outputBuffer)
    {
        using MemoryStream ms = new(outputBuffer, true);
        if (algorithm == Mode.Zlib)
        {
            using DeflaterOutputStream zlib = new(ms);
            zlib.Write(input);
            zlib.Finish();
        }
        else // Deflate (raw)
        {
            using DeflateStream deflate = new(ms, CompressionLevel.Optimal, true);
            deflate.Write(input);
        }

        return (int)ms.Position;
    }

    public static Mode DetectMode(ReadOnlySpan<byte> data)
    {
        // TODO: Investigate ttarch
        // CSI games use zlib compression instead of the regular raw deflate.
        // According to TTG Tools, version 8 archives use zlib (presumably for the files themselves)

        if (data.Length < 2)
            return Mode.None; // Not enough data to decide

        // Zlib header: first byte = 0x78, second byte's high nibble = 0xC
        // Common values: 0x78 0x9C, 0x78 0xDA, 0x78 0x01, 0x78 0x5E
        if (data[0] is 0x78 && data[1] is 0x9C or 0xDA or 0x01 or 0x5E)
            return Mode.Zlib;

        // Default: raw DEFLATE has no fixed header
        return Mode.Deflate;
    }
}

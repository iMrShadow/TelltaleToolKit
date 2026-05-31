using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Utility.Blowfish;
using BlockConfiguration = (int blockSize, int cryptInterval, int cleanInterval);

namespace TelltaleToolKit.TelltaleArchives;

public static class TelltaleArchiveUtilities
{
    private static CompressionAlgorithm DetectCompressionAlgorithm(byte[] chunk)
    {
        // Zlib header: first byte 0x78, second byte in {0x9C, 0xDA, 0x01, 0x5E, 0x9E, ...}
        if (chunk[0] == 0x78 && (chunk[1] & 0xF0) == 0xC0) // 0x78 0x9C, 0x78 0xDA, etc.
        {
            return CompressionAlgorithm.Zlib;
        }

        return CompressionAlgorithm.Deflate; // raw Deflate has no header
    }

    /// <summary>
    ///     Compresses a single chunk using the specified algorithm.
    /// </summary>
    /// <returns>Number of compressed bytes written into <paramref name="outputBuffer" />.</returns>
    public static int CompressBlock(ReadOnlySpan<byte> input, CompressionAlgorithm algorithm, byte[] outputBuffer)
    {
        using MemoryStream ms = new(outputBuffer, true);
        if (algorithm == CompressionAlgorithm.Zlib)
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
}

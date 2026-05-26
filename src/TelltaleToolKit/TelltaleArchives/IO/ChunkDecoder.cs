using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using TelltaleToolKit.TelltaleArchives.Caching;
using TelltaleToolKit.TelltaleArchives.Formats;
using TelltaleToolKit.Utility.Blowfish;

namespace TelltaleToolKit.TelltaleArchives.IO;

/// <summary>
///     Provides chunk-level decrypt + decompress logic shared by both
///     <see cref="TTArchive" /> and <see cref="TTArchive2" /> extraction paths.
/// </summary>
/// <remarks>
///     <para>
///         All methods are thread-safe with respect to the archive stream as long as each
///         concurrent caller holds its own <see cref="IChunkCache" /> instance and does not
///         share a <see cref="Stream" /> reference.
///     </para>
/// </remarks>
internal static class ChunkDecoder
{
    /// <summary>
    ///     Decrypts (if encrypted) and decompresses a raw compressed chunk read from disk.
    ///     Returns the decoded byte array.
    /// </summary>
    public static byte[] DecryptAndDecompress(byte[] compressed, uint expectedSize, ArchiveInfo info)
    {
        // Decrypt in place when the archive uses chunk-level encryption.
        if (info.Flags.IsEncrypted())
        {
            Blowfish bf = new(info.BlowfishKey, 7);
            bf.Decipher(compressed.AsSpan(), compressed.Length);
        }

        ArchiveFlags flags = info.Flags;
        return DecompressBlock(compressed, (int)expectedSize, ref flags);
    }

    // -------------------------------------------------------------------------
    // Build a block-offset table for the range [blockStart, blockEnd]
    // -------------------------------------------------------------------------

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

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

    public static byte[] DecompressBlock(byte[] compressedData, int expectedSize, ref ArchiveFlags flags)
    {
        // This is a hack? In TWD:DE, 401_txmesh, there's a page which is the same size as the expected size (65535)
        // C#'s raw deflate fails.
        // ttarchext returns the same data:
        // https://github.com/infernokun/TelltaleGamesExtractionGUI/blob/fec9fc1a70b545bcc67062fedc3fe3f7cd0d3e1b/bin/Debug/net6.0-windows/ttarchext/ttarchext.c#L1512
        if (compressedData.Length == expectedSize)
        {
            return compressedData;
        }

        // Try both Deflate and Zlib if both flags are set
        // TODO: Investigate ttarch
        // CSI games use zlib compression instead of the regular raw deflate.
        // According to TTG Tools, version 8 archives use zlib (presumably for the files themselves)
        if (flags.HasFlag(ArchiveFlags.IsRawDeflateCompressed) && flags.HasFlag(ArchiveFlags.IsZlibCompressed))
        {
            // CompressionAlgorithm compressionAlgorithm = DetectCompressionAlgorithm(compressedData);

            try
            {
                byte[] result = Decompress(ms => new DeflateStream(ms, CompressionMode.Decompress));
                flags &= ~ArchiveFlags.IsZlibCompressed;
                return result;
            }
            catch (Exception)
            {
                //  flags &= ~ContainerFlags.IsRawDeflateCompressed;
            }

            try
            {
                byte[] result = Decompress(ms => new InflaterInputStream(ms));
                return result;
            }
            catch (Exception)
            {
                // flags &= ~ContainerFlags.IsZlibCompressed;
            }

            throw new InvalidDataException("Compression not supported");
        }

        if (flags.HasFlag(ArchiveFlags.IsRawDeflateCompressed))
        {
            return Decompress(ms => new DeflateStream(ms, CompressionMode.Decompress));
        }

        if (flags.HasFlag(ArchiveFlags.IsZlibCompressed))
        {
            return Decompress(ms => new InflaterInputStream(ms));
        }

        if (flags.HasFlag(ArchiveFlags.IsOodleCompressed))
        {
            // TODO: Add Oodle compression.
            throw new NotSupportedException("Oodle compression is not supported yet.");
        }

        // No compression, return the original data
        return compressedData;

        // Local function to handle decompression
        byte[] Decompress(Func<Stream, Stream> streamFactory)
        {
            using MemoryStream outputStream = new(expectedSize);
            using Stream decompressStream = streamFactory(new MemoryStream(compressedData));
            decompressStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }
    }
}

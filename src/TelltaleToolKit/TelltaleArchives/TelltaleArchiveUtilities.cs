using System.IO.Compression;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Utility.Blowfish;
using BlockConfiguration = (int blockSize, int cryptInterval, int cleanInterval);

namespace TelltaleToolKit.TelltaleArchives;

public static class TelltaleArchiveUtilities
{
    public static void DecryptFile(byte[] file, string blowfishKey, int archiveVersion)
    {
        if (file.Length < 4)
            return;

        var magicFourCc = (MetaStreamVersion)BitConverter.ToUInt32(file, 0);
        BlockConfiguration config = GetBlockConfiguration(magicFourCc);

        if (config.blockSize > 0)
        {
            DecryptFileHelper(file, blowfishKey, archiveVersion, config);
        }
    }

    //(int blockSize, int cryptInterval, int cleanInterval);
    private static BlockConfiguration GetBlockConfiguration(MetaStreamVersion type)
        => type switch
        {
            MetaStreamVersion.Unknown1 or MetaStreamVersion.Unknown2 or MetaStreamVersion.Unknown3 => (128, 32, 80),
            MetaStreamVersion.Unknown4 => (256, 8, 24),
            MetaStreamVersion.Mbes => (64, 64, 100),
            _ => (0, 0, 0),
        };

    private static void DecryptFileHelper(byte[] fileData, string key, int archiveVersion, BlockConfiguration config)
    {
        var blowFish = new Blowfish(key, archiveVersion);

        // If the blocksize is 0 (Stream version is not existent), use regular blowfish
        if (config.blockSize == 0)
        {
            blowFish.Decipher(fileData, fileData.Length);
            return;
        }

        // Replace the magic number with Mbin (Might be replaced with MBIN/MTRE)
        Array.Copy(BitConverter.GetBytes((uint)MetaStreamVersion.Mbin), 0, fileData, 0, 4);

        long blocks = (fileData.Length - 4) / config.blockSize;

        for (var i = 0; i < blocks; i++)
        {
            var block = new Span<byte>(fileData, 4 + i * config.blockSize, config.blockSize);

            if (i % config.cryptInterval == 0)
            {
                blowFish.Decipher(block, block.Length);
            }
            else if (i % config.cleanInterval == 0 && i > 0)
            {
                // Clean interval, skip this block
            }
            else
            {
                XorBlock(block, 0xFF);
            }
        }
    }

    public static void XorBlock(Span<byte> block, byte xor)
    {
        for (var i = 0; i < block.Length; i++)
        {
            block[i] ^= xor;
        }
    }

    public static byte[] DecompressBlock(byte[] compressedData, int expectedSize, ref ContainerFlags flags)
    {
        // This is a hack? In TWD:DE, 401_txmesh, there's a page which is the same size as the expected size (65535)
        // C#'s raw deflate fails.
        // ttarchext returns the same data:
        // https://github.com/infernokun/TelltaleGamesExtractionGUI/blob/fec9fc1a70b545bcc67062fedc3fe3f7cd0d3e1b/bin/Debug/net6.0-windows/ttarchext/ttarchext.c#L1512
        if (compressedData.Length == expectedSize)
            return compressedData;

        // Try both Deflate and Zlib if both flags are set
        // TODO: Investigate ttarch
        // CSI games use zlib compression instead of the regular raw deflate.
        // According to TTG Tools, version 8 archives use zlib (presumably for the files themselves)
        if (flags.HasFlag(ContainerFlags.IsRawDeflateCompressed) && flags.HasFlag(ContainerFlags.IsZlibCompressed))
        {
            try
            {
                byte[] result = Decompress(ms => new DeflateStream(ms, CompressionMode.Decompress));
                flags &= ~ContainerFlags.IsZlibCompressed;
                return result;
            }
            catch (Exception)
            {
                //  flags &= ~ContainerFlags.IsRawDeflateCompressed;
            }

            try
            {
                byte[] result = Decompress(ms => new ZLibStream(ms, CompressionMode.Decompress));
                return result;
            }
            catch (Exception)
            {
                // flags &= ~ContainerFlags.IsZlibCompressed;
            }

            throw new InvalidDataException("Compresion not supported");
        }
        else if (flags.HasFlag(ContainerFlags.IsRawDeflateCompressed))
        {
            return Decompress(ms => new DeflateStream(ms, CompressionMode.Decompress));
        }
        else if (flags.HasFlag(ContainerFlags.IsZlibCompressed))
        {
            return Decompress(ms => new ZLibStream(ms, CompressionMode.Decompress));
        }
        else if (flags.HasFlag(ContainerFlags.IsOodleCompressed))
        {
            // TODO: Add Oodle compression.
            throw new NotSupportedException("Oodle compression is not supported yet.");
        }

        // No compression, return the original data
        return compressedData;

        // Local function to handle decompression
        byte[] Decompress(Func<Stream, Stream> streamFactory)
        {
            using var outputStream = new MemoryStream(expectedSize);
            using Stream decompressStream = streamFactory(new MemoryStream(compressedData));
            decompressStream.CopyTo(outputStream);
            return outputStream.ToArray();
        }
    }
}
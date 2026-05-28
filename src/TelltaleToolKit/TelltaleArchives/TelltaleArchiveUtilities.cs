using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Utility.Blowfish;
using BlockConfiguration = (int blockSize, int cryptInterval, int cleanInterval);

namespace TelltaleToolKit.TelltaleArchives;

public static class TelltaleArchiveUtilities
{
    public static void DecryptFile(byte[] file, string blowfishKey, int archiveVersion)
    {
        if (file.Length < 4)
        {
            return;
        }

        DecryptFile(file, new Blowfish(blowfishKey, archiveVersion));
    }

    public static void DecryptFile(byte[] file, Blowfish blowfish)
    {
        if (file.Length < 4)
        {
            return;
        }

        MetaStreamVersion magicFourCc = (MetaStreamVersion)BitConverter.ToUInt32(file, 0);
        BlockConfiguration config = GetBlockConfiguration(magicFourCc);

        if (config.blockSize > 0)
        {
            DecryptFileHelper(file, blowfish, config);
        }
    }

    //(int blockSize, int cryptInterval, int cleanInterval);
    private static BlockConfiguration GetBlockConfiguration(MetaStreamVersion type)
        => type switch
        {
            MetaStreamVersion.Unknown1 or MetaStreamVersion.Unknown2 or MetaStreamVersion.Unknown3 => (128, 32, 80),
            MetaStreamVersion.Unknown4 => (256, 8, 24),
            MetaStreamVersion.Mbes => (64, 64, 100),
            _ => (0, 0, 0)
        };

    private static void DecryptFileHelper(byte[] fileData, Blowfish blowFish, BlockConfiguration config)
    {
        // If the blocksize is 0 (Stream version is not existent), use regular blowfish
        if (config.blockSize == 0)
        {
            blowFish.Decipher(fileData, fileData.Length);
            return;
        }

        // Replace the magic number with Mbin (Might be replaced with MBIN/MTRE)
        Array.Copy(BitConverter.GetBytes((uint)MetaStreamVersion.Mbin), 0, fileData, 0, 4);

        long blocks = (fileData.Length - 4) / config.blockSize;

        for (int i = 0; i < blocks; i++)
        {
            Span<byte> block = new(fileData, 4 + i * config.blockSize, config.blockSize);

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
        for (int i = 0; i < block.Length; i++)
        {
            block[i] ^= xor;
        }
    }

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

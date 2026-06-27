using System.Buffers;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.Encryption;

/// <summary>
///     Provides static helpers to encrypt and decrypt legacy Telltale MetaStream files
///     directly on a seekable <see cref="Stream" />, one block at a time.
///     No full-file buffering is performed — memory usage is O(blockSize).
/// </summary>
public static class LegacyEncryption
{
    /// <inheritdoc cref="Decrypt(Stream, Blowfish)" />
    public static void Decrypt(Stream stream, string blowfishKey, int archiveVersion = 7)
        => Decrypt(stream, new Blowfish(blowfishKey, archiveVersion));

    /// <summary>
    ///     Decrypts a legacy-encrypted MetaStream in-place by reading and rewriting
    ///     one block at a time. The magic is patched to its plaintext equivalent.
    /// </summary>
    /// <param name="stream">
    ///     A readable, writable, seekable stream positioned anywhere.
    ///     The stream must be at least 4 bytes long.
    /// </param>
    /// <param name="blowfish">Pre-built Blowfish instance for this game.</param>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the stream does not support reading, writing, or seeking.
    /// </exception>
    public static void Decrypt(Stream stream, Blowfish blowfish)
    {
        EnsureStream(stream);
        if (stream.Length < 4)
        {
            return;
        }

        MetaStreamMagic magic = ReadMagic(stream);
        (int BlockSize, int CryptInterval, int CleanInterval) config = GetBlockConfig(magic);
        stream.Seek(0, SeekOrigin.Begin);
        if (config.BlockSize == 0)
        {
            return; // not an encrypted magic
        }

        ProcessBlocks(stream, blowfish, config, false);

        // Patch magic to the readable plaintext equivalent
        WriteMagic(stream, GetPlaintextMagic(magic));
        stream.Seek(0, SeekOrigin.Begin);
    }

    // -------------------------------------------------------------------------
    // Public API — Encrypt
    // -------------------------------------------------------------------------

    /// <inheritdoc cref="Encrypt(Stream, MetaStreamMagic, Blowfish)" />
    public static void Encrypt(Stream stream, MetaStreamMagic targetMagic, string blowfishKey, int archiveVersion = 7)
        => Encrypt(stream, targetMagic, new Blowfish(blowfishKey, archiveVersion));

    /// <summary>
    ///     Encrypts a plaintext MetaStream in-place by reading and rewriting one block
    ///     at a time. The magic is patched to <paramref name="targetMagic" /> first so
    ///     the block layout matches what the decryptor will expect.
    /// </summary>
    /// <param name="stream">
    ///     A readable, writable, seekable stream containing a plaintext MetaStream
    ///     (Mbin or Mtre magic at offset 0). Must be at least 4 bytes.
    /// </param>
    /// <param name="targetMagic">
    ///     The encrypted magic to embed, e.g. <see cref="MetaStreamMagic.EncryptedMbin1" />.
    ///     Must be a recognised encrypted magic — drives the block configuration.
    /// </param>
    /// <param name="blowfish">Pre-built Blowfish instance for this game.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown when <paramref name="targetMagic" /> has no encryption scheme.
    /// </exception>
    /// <exception cref="NotSupportedException">
    ///     Thrown when the stream does not support reading, writing, or seeking.
    /// </exception>
    public static void Encrypt(Stream stream, MetaStreamMagic targetMagic, Blowfish blowfish)
    {
        EnsureStream(stream);
        if (stream.Length < 4)
        {
            return;
        }

        (int BlockSize, int CryptInterval, int CleanInterval) config = GetBlockConfig(targetMagic);
        if (config.BlockSize == 0)
        {
            throw new ArgumentException(
                $"{targetMagic} is not a recognised encrypted magic.", nameof(targetMagic));
        }

        // Patch magic before processing so block offsets are consistent with the decryptor.
        WriteMagic(stream, targetMagic);
        ProcessBlocks(stream, blowfish, config, true);
    }


    private static void ProcessBlocks(Stream stream, Blowfish blowfish,
        (int BlockSize, int CryptInterval, int CleanInterval) config, bool encrypt)
    {
        // Blocks begin immediately after the 4-byte magic.
        const int dataStart = 4;
        long dataLength = stream.Length - dataStart;
        if (dataLength <= 0)
        {
            return;
        }

        int blockSize = config.BlockSize;
        long blockCount = dataLength / blockSize;
        byte[] buffer = ArrayPool<byte>.Shared.Rent(blockSize);
        // Trailing partial block: untouched, matches reference behaviour.

        // Rent exactly blockSize bytes — never more — so large files stay lean.
        try
        {
            Span<byte> block = buffer.AsSpan(0, blockSize);
            for (long i = 0; i < blockCount; i++)
            {
                long offset = dataStart + i * blockSize;
                if (i % config.CryptInterval == 0)
                {
                    ReadExactly(stream, offset, block);
                    if (encrypt)
                        blowfish.Encipher(block, block.Length);
                    else
                        blowfish.Decipher(block, block.Length);
                    WriteAt(stream, offset, block);
                }
                else if (i % config.CleanInterval == 0 && i > 0)
                {
                    // clean block – do nothing
                }
                else
                {
                    ReadExactly(stream, offset, block);
                    XorBlock(block, 0xFF);
                    WriteAt(stream, offset, block);
                }
            }
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private static void ReadExactly(Stream stream, long offset, Span<byte> buffer)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        int total = 0;
        while (total < buffer.Length)
        {
            int read = stream.Read(buffer.Slice(total));
            if (read == 0)
                throw new EndOfStreamException($"Expected {buffer.Length} bytes at offset {offset}, got {total}.");
            total += read;
        }
    }

    private static void WriteAt(Stream stream, long offset, ReadOnlySpan<byte> data)
    {
        stream.Seek(offset, SeekOrigin.Begin);
        stream.Write(data);
    }

    private static MetaStreamMagic ReadMagic(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        Span<byte> buf = stackalloc byte[4];
        ReadExactly(stream, 0, buf);
        return (MetaStreamMagic)BitConverter.ToUInt32(buf);
    }

    private static void WriteMagic(Stream stream, MetaStreamMagic magic)
    {
        stream.Seek(0, SeekOrigin.Begin);
        Span<byte> buf = stackalloc byte[4];
        BitConverter.TryWriteBytes(buf, (uint)magic);
        stream.Write(buf);
    }

    private static void EnsureStream(Stream stream)
    {
        if (!stream.CanRead)
        {
            throw new NotSupportedException("Stream must be readable.");
        }

        if (!stream.CanWrite)
        {
            throw new NotSupportedException("Stream must be writable.");
        }

        if (!stream.CanSeek)
        {
            throw new NotSupportedException("Stream must be seekable.");
        }
    }

    private static void XorBlock(Span<byte> block, byte mask)
    {
        for (int i = 0; i < block.Length; i++)
            block[i] ^= mask;
    }

    private static (int BlockSize, int CryptInterval, int CleanInterval) GetBlockConfig(MetaStreamMagic magic) =>
        magic switch
        {
            MetaStreamMagic.EncryptedMbin1
                or MetaStreamMagic.EncryptedMbin2
                or MetaStreamMagic.EncryptedMbin3 => (128, 32, 80),
            MetaStreamMagic.EncryptedMtre
                or MetaStreamMagic.EncryptedMcom => (256, 8, 24),
            MetaStreamMagic.Mbes => (64, 64, 100),
            _ => default
        };

    private static MetaStreamMagic GetPlaintextMagic(MetaStreamMagic encrypted) => encrypted switch
    {
        MetaStreamMagic.EncryptedMbin1
            or MetaStreamMagic.EncryptedMbin2
            or MetaStreamMagic.EncryptedMbin3 or MetaStreamMagic.Mbes => MetaStreamMagic.Mbin,
        MetaStreamMagic.EncryptedMtre => MetaStreamMagic.Mtre,
        MetaStreamMagic.EncryptedMcom => MetaStreamMagic.Mcom,
        _ => encrypted
    };
}

using System.Text;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.TelltaleArchives;

// The real name is ttarch which stands for "Telltale Archive".
public class T3Archive : ArchiveBase
{
    /// <summary>
    /// Reads the oldest available encrypted versions of the archives.
    /// </summary>
    /// <param name="reader"></param>
    public void ReadLegacyStream(BinaryReader reader)
    {
        reader.BaseStream.Seek(0, SeekOrigin.Begin);
        Info.Version = ArchiveVersion.Zero;

        uint headerSize = reader.ReadUInt32();

        if (headerSize > 128)
        {
            Info.Flags |= ContainerFlags.IsEncrypted;

            byte[] header = reader.ReadBytes((int)headerSize);

            DecryptBlock(header, 0, Info.BlowfishKey, Info.Flags);

            ReadEntries(new MemoryStream(header));
            Info.FilesOffset = reader.BaseStream.Position;
        }
        else
        {
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            ReadEntries(reader);
            Info.FilesOffset = reader.ReadUInt32();
            uint filesSize = reader.ReadUInt32();
        }
    }

    public void ReadEntries(Stream stream)
    {
        using var reader = new BinaryReader(stream, Encoding.UTF8, true);
        ReadEntries(reader);
    }

    private void ReadEntries(BinaryReader reader)
    {
        int directoriesCount = reader.ReadInt32();
        for (var i = 0; i < directoriesCount; i++)
        {
            int directoryNameLength = reader.ReadInt32();

            if (directoryNameLength > 255)
            {
                throw new ArgumentOutOfRangeException(
                    $"Error: directory name length {directoryNameLength} is too long.", nameof(directoryNameLength)
                );
            }

            string name = Encoding.ASCII.GetString(reader.ReadBytes(directoryNameLength));
        }

        Info.FileCount = reader.ReadUInt32();

        FileEntries = new TelltaleFileEntry[Info.FileCount];

        for (var i = 0; i < Info.FileCount; i++)
        {
            FileEntries[i] = new TelltaleFileEntry();

            int filenameLength = reader.ReadInt32();
            if (filenameLength > 255)
            {
                throw new ArgumentOutOfRangeException(
                    $"Error: File name length {filenameLength} is too long.", nameof(filenameLength));
            }

            byte[] fileName = reader.ReadBytes(filenameLength);

            FileEntries[i].Name = Encoding.ASCII.GetString(fileName);
            _ = reader.ReadInt32(); // always shows 0 value. Probably a way to assign to a folder, in the order of the folder names.
            FileEntries[i].FileOffset = reader.ReadUInt32();
            FileEntries[i].FileSize = reader.ReadInt32();

            FileEntries[i].Crc64 = Symbol.GetCrc64(FileEntries[i].Name);
        }
    }

    protected override void ReadMetadata()
    {
        using BinaryReader reader = new(ArchiveStream, Encoding.UTF8, true);

        Info.Version = (ArchiveVersion)reader.ReadUInt32();
        var version = (int)Info.Version;

        if (version is > 9 or < 1)
        {
            ReadLegacyStream(reader);
            return;
        }

        uint decryptionMode = reader.ReadUInt32();
        if (decryptionMode > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(decryptionMode),
                "Error: decryptionMode not supported yet");
        }

        Info.Flags |= decryptionMode == 1 ? ContainerFlags.IsEncrypted : ContainerFlags.None;

        // Unknown
        uint _ = reader.ReadUInt32();

        // Files mode 2 is the most common value. No idea what it means.
        uint filesMode = version >= 3 ? reader.ReadUInt32() : 0;
        if (filesMode > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(filesMode),
                "Error: files_mode {filesMode} is not supported yet");
        }

        // Compression is supported from version 3.
        if (version >= 3)
        {
            Info.ChunkCount = reader.ReadUInt32();
            if (Info.ChunkCount > 100000)
            {
                throw new ArgumentOutOfRangeException(nameof(Info.ChunkCount),
                    "Error: Chunk counts are more than expected!");
            }

            if (Info.ChunkCount > 0)
            {
                // Set both flags on. The decompression function will recognize which one is used.
                Info.Flags |= ContainerFlags.IsRawDeflateCompressed | ContainerFlags.IsZlibCompressed;
                Info.ChunkBlockSizes = new long[Info.ChunkCount];
                for (var i = 0; i < Info.ChunkCount; i++)
                {
                    Info.ChunkBlockSizes[i] = reader.ReadUInt32();
                }
            }

            uint FileDataSize =
                reader.ReadUInt32(); // the size of the field where are stored all the files contents

            if (version >= 4)
            {
                reader.ReadUInt32(); // unknown ? priority ?
                reader.ReadUInt32();

                if (version >= 7)
                {
                    uint xModeValue1 = reader.ReadUInt32(); // unknown
                    uint xModeValue2 = reader.ReadUInt32(); // unknown

                    Info.Flags |= xModeValue1 == 1 || xModeValue2 == 1
                        ? ContainerFlags.IsXMode
                        : ContainerFlags.None;

                    const uint blockSize = 1024;
                    Info.ChunkSize = (int)(reader.ReadUInt32() * blockSize);
                    if (version >= 8)
                    {
                        var t = reader.ReadByte(); // unknown boolean
                    }

                    // FilesMode 2 = compressed
                    // FilesMode 1 = normal?
                    // Or maybe filesmode 2 means NO folders, while 1 means there are?
                    if (version >= 9 && filesMode >= 1)
                    {
                        // Another 0?
                        reader.ReadUInt32();
                    }
                }
            }
        }

        int infoHeaderSize = reader.ReadInt32(); // size of the header

        // This is a workaround for some version 8 archives. I...I don't know why.
        infoHeaderSize = infoHeaderSize == 0 ? reader.ReadInt32() : infoHeaderSize;

        int compressedInfoHeaderSize =
            version >= 7 && filesMode >= 2 ? reader.ReadInt32() : 0; // size of the compressed data

        byte[] header =
            version >= 7 && filesMode == 2
                ? reader.ReadBytes(compressedInfoHeaderSize)
                : reader.ReadBytes(infoHeaderSize);

        Info.FilesOffset = reader.BaseStream.Position;

        if (IsCompressed() && version >= 7 && filesMode == 2)
        {
            ContainerFlags newFlags = Info.Flags;
            // Headers are not compressed for some reason in earlier versions.
            header = TelltaleArchiveUtilities.DecompressBlock(header, infoHeaderSize, ref newFlags);
            // Info.Flags = newFlags;
        }

        DecryptBlock(header, version, Info.BlowfishKey, Info.Flags);

        // Set the archive offset (info header)
        var streamHeader = new MemoryStream(header);
        ReadEntries(streamHeader);
    }


    public override void ExtractAll(string destinationPath)
    {
        throw new NotImplementedException();
    }

    public override MemoryStream ExtractFile(ulong crc64)
    {
        TelltaleFileEntry? entry = FindEntry(crc64);

        if (entry == null)
            throw new FileNotFoundException($"File '{crc64}' not found in the archive.");

        return ExtractFile(entry);
    }

    public override MemoryStream ExtractFile(string name)
    {
        TelltaleFileEntry? entry = FindEntry(name);

        if (entry == null)
            throw new FileNotFoundException($"File '{name}' not found in the archive.");

        return ExtractFile(entry);
    }

    public MemoryStream ExtractFile(TelltaleFileEntry entry)
    {
        byte[] result; // Initialize the result array

        int fileSize = entry.FileSize; // Get the file size

        if (!IsCompressed())
        {
            ArchiveStream.Seek(Info.FilesOffset + entry.FileOffset, SeekOrigin.Begin);
            result = new byte[fileSize];
            var totalRead = 0;
            while (totalRead < result.Length)
            {
                int bytesRead = ArchiveStream.Read(result, totalRead, result.Length - totalRead);
                if (bytesRead == 0)
                    throw new EndOfStreamException($"Unable to read {result.Length} bytes from stream.");
                totalRead += bytesRead;
            } // Read the file data directly

            TelltaleArchiveUtilities.DecryptFile(result, Info.BlowfishKey, (int)Info.Version);
        }
        else
        {
            var blockStartIndex = (int)(entry.FileOffset / Info.ChunkSize);
            var blockEndIndex = (int)((entry.FileOffset + fileSize) / Info.ChunkSize);

            if (blockStartIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(blockStartIndex), "Block start index is less than 0.");
            if (blockStartIndex > (int)Info.ChunkCount)
                throw new ArgumentOutOfRangeException(nameof(blockStartIndex),
                    "Block start index is greater than chunk count.");
            if (blockEndIndex > (int)Info.ChunkCount)
                throw new ArgumentOutOfRangeException(nameof(blockEndIndex),
                    "Block end index is greater than chunk count.");

            long blockStartOffset = 0;

            for (var i = 0; i < blockStartIndex; i++)
            {
                blockStartOffset += Info.ChunkBlockSizes[i];
            }

            ArchiveStream.Seek(Info.FilesOffset + blockStartOffset,
                SeekOrigin.Begin); // Seek to the block start offset

            using MemoryStream ms = new();
            for (int i = blockStartIndex; i <= blockEndIndex; i++)
            {
                var tmp = new byte[Info.ChunkBlockSizes[i]];

                // Replace ReadExactly with manual reading
                var totalRead = 0;
                while (totalRead < tmp.Length)
                {
                    int bytesRead = ArchiveStream.Read(tmp, totalRead, tmp.Length - totalRead);
                    if (bytesRead == 0)
                        throw new EndOfStreamException($"Unable to read {tmp.Length} bytes from stream.");
                    totalRead += bytesRead;
                }

                DecryptBlock(tmp, (int)Info.Version, Info.BlowfishKey, Info.Flags);

                ContainerFlags containerFlags = Info.Flags;
                tmp = TelltaleArchiveUtilities.DecompressBlock(tmp, Info.ChunkSize, ref containerFlags);

                ms.Write(tmp);
            }

            byte[] block = ms.ToArray();
            result = new byte[fileSize];

            // This is relative to the blocks themselves
            long startIndex = entry.FileOffset - Info.ChunkSize * blockStartIndex;
            Array.Copy(block, startIndex, result, 0, result.Length);
        }

        return new MemoryStream(result);
    }
}
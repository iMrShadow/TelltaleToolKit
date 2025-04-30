using System.Text;

namespace TelltaleToolKit.TelltaleArchives;

public class T3Archive2 : ArchiveBase
{
    public void ReadContainer(BinaryReader reader)
    {
        Info.ContainerVersion = (ContainerVersion)reader.ReadUInt32();

        // Setup encryption and compression flags
        if (Info.ContainerVersion is ContainerVersion.Ttce or ContainerVersion.TtCe)
        {
            Info.Flags |= ContainerFlags.IsEncrypted;
        }

        switch (Info.ContainerVersion)
        {
            case ContainerVersion.Ttce or ContainerVersion.Ttcz:
                Info.Flags |= ContainerFlags.IsRawDeflateCompressed;
                break;
            case ContainerVersion.TtCe or ContainerVersion.TtCz:
            {
                uint compressionType = reader.ReadUInt32();

                Info.Flags = compressionType switch
                {
                    0 => ContainerFlags.IsRawDeflateCompressed,
                    1 => ContainerFlags.IsOodleCompressed,
                    _ => throw new NotSupportedException($"Unsupported compression type: {compressionType}"),
                };
                break;
            }
        }

        if (IsCompressed())
        {
            Info.ChunkSize = reader.ReadInt32(); // Also known as window size. This is the size per page.
            if (Info.ChunkSize < 0)
            {
                throw new InvalidDataException("Invalid chunk size");
            }
            
            Info.ChunkCount = reader.ReadUInt32(); // Number of chunks in the archive

            Info.ChunkBlockSizes = new long[Info.ChunkCount];

            long previousChunkOffset = reader.ReadInt64(); // Read the first chunk block
            for (var i = 0; i < Info.ChunkCount; i++)
            {
                long nextChunkOffset = reader.ReadInt64();

                Info.ChunkBlockSizes[i] =
                    nextChunkOffset - previousChunkOffset; // Calculate the offset of the chunk block

                previousChunkOffset = nextChunkOffset; // Update the first chunk block offset
            }
        }
        else
        {
            ulong containerSize = reader.ReadUInt64(); // Read the size of the archive
        }
    }

    protected override void ReadMetadata()
    {
        // TODO I am aware that the initial headers are not exclusive to TTArch2, but rather for telltale containers (used for save files)
        // I will assume they are specific to TTArch2 for convenience.
        using var mainReader = new BinaryReader(ArchiveStream, Encoding.UTF8, true);

        ReadContainer(mainReader); // Read the container header
        Info.ArchiveOffset =
            mainReader.BaseStream
                .Position; // Set the file offset to the current position in the stream (Where TTA4/TTA3 starts)

        byte[] finalFullHeader = [];
        uint fileNameBufferSize;
        if (IsCompressed())
        {
            // Try to read the TTArch2 header
            byte[] header = DecodeBlock(
                mainReader.ReadBytes((int)Info.ChunkBlockSizes[0]),
                Info.ChunkSize,
                Info.BlowfishKey,
                Info.Flags
            ); // Decrypt and decompress the header

            using BinaryReader headerReader = new(new MemoryStream(header));
            Info.Version = (ArchiveVersion)headerReader.ReadUInt32(); // Read the version of the archive

            if (Info.Version is ArchiveVersion.Tta3)
            {
                headerReader.ReadUInt32(); // Read the size of the header
            }

            fileNameBufferSize = headerReader.ReadUInt32(); // Read the size of the file name buffer
            ArgumentOutOfRangeException.ThrowIfGreaterThan<uint>(
                fileNameBufferSize,
                0x10000000,
                nameof(fileNameBufferSize)
            );

            Info.FileCount = headerReader.ReadUInt32(); // Read the number of files in the archive
            ArgumentOutOfRangeException.ThrowIfGreaterThan<uint>(Info.FileCount, 0xFFFFF, nameof(Info.FileCount));

            FileEntries = new TelltaleFileEntry[Info.FileCount]; // Initialize the file entries dictionary

            Info.FilesOffset = headerReader.BaseStream.Position + (28 * Info.FileCount) + fileNameBufferSize;

            // Read all needed chunk blocks for the header
            if (Info.FilesOffset > headerReader.BaseStream.Length)
            {
                mainReader.BaseStream.Seek(Info.ArchiveOffset,
                    SeekOrigin.Begin); // Reset the stream position to the beginning
                long chunksToIterate = Info.FilesOffset / Info.ChunkSize + 1;

                using MemoryStream ms = new();

                for (var i = 0; i < chunksToIterate; i++)
                {
                    byte[] chunk = DecodeBlock(
                        mainReader.ReadBytes((int)Info.ChunkBlockSizes[i]),
                        Info.ChunkSize,
                        Info.BlowfishKey,
                        Info.Flags
                    );

                    ms.Write(chunk, 0, chunk.Length);
                }

                finalFullHeader = ms.ToArray();
            }
        }

        using BinaryReader fullHeader = IsCompressed() ? new(new MemoryStream(finalFullHeader)) : mainReader;

        Info.Version = (ArchiveVersion)fullHeader.ReadUInt32(); // Read the version of the archive

        if (Info.Version is ArchiveVersion.Tta3 or ArchiveVersion.Tta2)
        {
            uint unknown = fullHeader.ReadUInt32(); // Read the size of the header
        }

        fileNameBufferSize = fullHeader.ReadUInt32(); // Read the size of the file name buffer
        ArgumentOutOfRangeException.ThrowIfGreaterThan<uint>(
            fileNameBufferSize,
            0x10000000,
            nameof(fileNameBufferSize)
        );

        Info.FileCount = fullHeader.ReadUInt32(); // Read the number of files in the archive
        ArgumentOutOfRangeException.ThrowIfGreaterThan<uint>(Info.FileCount, 0xFFFFF, nameof(Info.FileCount));

        Info.FilesOffset = IsCompressed()
            ? Info.FilesOffset
            : fullHeader.BaseStream.Position + (28 * Info.FileCount) + fileNameBufferSize;

        FileEntries = new TelltaleFileEntry[Info.FileCount]; // Initialize the file entries dictionary

        var filesAndNamesTotalSize =
            (int)((Info.FileCount * 28) + fileNameBufferSize); // Calculate the total size of files and names
        // 28 is not a good number

        var nameOffsets = new uint[FileCount()]; // Array to store name offsets

        for (var i = 0; i < FileCount(); i++)
        {
            FileEntries[i] = new TelltaleFileEntry
            {
                Crc64 = fullHeader.ReadUInt64(),
                FileOffset = fullHeader.ReadInt64(),
            };

            if (Info.Version is ArchiveVersion.Tta2)
            {
                uint unused = fullHeader.ReadUInt32(); // Read an unused value (possibly a version CRC)
            }

            FileEntries[i].FileSize = fullHeader.ReadInt32();
            int unknown = fullHeader.ReadInt32(); // Read an unknown value (possibly a folder index)
            ushort nameBlock = fullHeader.ReadUInt16(); // Read the name block offset
            ushort nameOffset = fullHeader.ReadUInt16(); // Read the name offset

            nameOffsets[i] = (uint)(nameBlock * 0x10000 + nameOffset); // Calculate the name offset

            //  ArgumentOutOfRangeException.ThrowIfGreaterThan(FileEntries[i].NameOffset, fileNameBufferSize, ); // Ensure the name offset is valid
        }

        long fileNamesOffset = fullHeader.BaseStream.Position; // position for filename table

        for (var i = 0; i < FileCount(); i++)
        {
            fullHeader.BaseStream.Seek(fileNamesOffset + nameOffsets[i], SeekOrigin.Begin); // Seek to the name offset

            MemoryStream nameEntry = new();
            byte character;
            while ((character = fullHeader.ReadByte()) != 0)
            {
                nameEntry.WriteByte(character);
            }

            FileEntries[i].Name = Encoding.ASCII.GetString(nameEntry.ToArray());
        }
    }

    public byte[] DecodeBlock(byte[] data, int expectedBufferSize, string key, ContainerFlags flags)
    {
        if (!IsEncrypted() && !IsCompressed())
        {
            return data; // No compression or encryption, return the original data
        }

        DecryptBlock(data, 7, key, flags);
        byte[] decompressedData = TelltaleArchiveUtilities.DecompressBlock(data, expectedBufferSize, ref flags);
        return decompressedData;
    }


    public override void ExtractAll(string destinationPath)
    {
        throw new NotImplementedException();
    }

    public override MemoryStream ExtractFile(string name)
    {
        using BinaryReader reader = new(ArchiveStream, Encoding.UTF8, true);

        TelltaleFileEntry entry = FindEntry(name) ??
                                 throw new FileNotFoundException($"File '{name}' not found in the archive.");

        long fileOffset = Info.FilesOffset + entry.FileOffset; // Calculate the file offset
        int fileSize = entry.FileSize; // Get the file size

        MemoryStream result = new(fileSize);
        BinaryWriter resultWriter = new(result);

        if (IsCompressed())
        {
            var blockStartIndex = (int)(fileOffset / Info.ChunkSize);
            var blockEndIndex = (int)((fileOffset + fileSize) / Info.ChunkSize);

            ArgumentOutOfRangeException.ThrowIfLessThan(blockStartIndex, 0, "Block start index is less than 0.");
            ArgumentOutOfRangeException.ThrowIfGreaterThan(
                blockStartIndex,
                (int)Info.ChunkCount,
                "Block start index is greater than chunk count."
            );
            ArgumentOutOfRangeException.ThrowIfGreaterThan(
                blockEndIndex,
                (int)Info.ChunkCount,
                "Block end index is greater than chunk count."
            );

            long blockStartOffset = 0;

            for (var i = 0; i < blockStartIndex; i++)
            {
                blockStartOffset += Info.ChunkBlockSizes[i];
            }

            // Seek to the block start offset
            reader.BaseStream.Seek(Info.ArchiveOffset + blockStartOffset, SeekOrigin.Begin); 

            using MemoryStream ms = new();
            using BinaryWriter blockWriter = new(ms);
            for (int blockIndex = blockStartIndex; blockIndex <= blockEndIndex; blockIndex++)
            {
                byte[] chunk = DecodeBlock(
                    reader.ReadBytes((int)Info.ChunkBlockSizes[blockIndex]),
                    Info.ChunkSize,
                    Info.BlowfishKey,
                    Info.Flags
                ); // Read the chunk block

                blockWriter.Write(chunk, 0, chunk.Length);
            }

            // This is relative to the blocks themselves
            long startIndex = fileOffset - (Info.ChunkSize * blockStartIndex); 
            resultWriter.Write(ms.ToArray(), (int)startIndex, fileSize); // Write the data to the result stream
        }
        else
        {
            reader.BaseStream.Seek(fileOffset, SeekOrigin.Begin); // Seek to the file offset
            resultWriter.Write(reader.ReadBytes(fileSize), 0, fileSize); // Read the file data
        }

        return result;
    }
}
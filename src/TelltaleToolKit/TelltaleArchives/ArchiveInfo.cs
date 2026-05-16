namespace TelltaleToolKit.TelltaleArchives;

public class ArchiveInfo
{
    public ContainerVersion ContainerVersion { get; set; }
    public ArchiveVersion Version { get; set; }
    public ContainerFlags Flags { get; set; }
    public uint ChunkSize { get; set; }
    public uint ChunkCount { get; set; }
    public ulong[] ChunkBlockSizes { get; set; } = [];

    /// <summary>
    /// The Blowfish key used for encryption/decryption.
    /// </summary>
    public string BlowfishKey { get; set; } = string.Empty;

    public ulong FilesOffset { get; set; }
    public ulong ArchiveOffset { get; set; } // This is for TTA2, TTA3, TTA4
    public uint FileCount { get; set; }
}

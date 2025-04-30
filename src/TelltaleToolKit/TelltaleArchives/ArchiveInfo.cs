namespace TelltaleToolKit.TelltaleArchives;

public class ArchiveInfo
{
    public ContainerVersion ContainerVersion { get; set; }
    public ArchiveVersion Version { get; set; }
    public ContainerFlags Flags { get; set; }
    public int ChunkSize { get; set; }
    public uint ChunkCount { get; set; }
    public long[] ChunkBlockSizes { get; set; } = [];

    /// <summary>
    /// The Blowfish key used for encryption/decryption.
    /// </summary>
    public string BlowfishKey { get; set; } = string.Empty;

    public long FilesOffset { get; set; }
    public long ArchiveOffset { get; set; } // This is for TTA2, TTA3, TTA4
    public uint FileCount { get; set; }
}
namespace TelltaleToolKit.TelltaleArchives;

public class TelltaleFileEntry
{
    public string Name { get; set; } = string.Empty;
    public ulong Crc64 { get; init; }
    public long FileOffset { get; set; } // The offset starts when the actual files are written.
    public int FileSize { get; set; }

    public override string ToString()
    {
        return Crc64 == 0
            ? $"{Name} (Offset: {FileOffset}, Size: {FileSize})"
            : $"{Name} (CRC: {Crc64:X8}, Offset: {FileOffset}, Size: {FileSize})";
    }
}
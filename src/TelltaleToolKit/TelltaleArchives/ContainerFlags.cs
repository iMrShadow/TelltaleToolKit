namespace TelltaleToolKit.TelltaleArchives;

[Flags]
public enum ContainerFlags
{
    None = 0,
    IsEncrypted = 1 << 1, // If the archive is Blowfish encrypted
    IsZlibCompressed = 1 << 3, // If the archive is compressed with zlib
    IsRawDeflateCompressed = 1 << 4, // If the archive is compressed with raw deflate (zlib header stripped)
    IsOodleCompressed = 1 << 5, // If the archive is compressed with Oodle
    IsModifiedBlowfishEncrypted = 1 << 6, // If the archive is encrypted with a weird Blowfish key
    IsXMode = 1 << 7,
}
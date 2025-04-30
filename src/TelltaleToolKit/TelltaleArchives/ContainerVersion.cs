namespace TelltaleToolKit.TelltaleArchives;

public enum ContainerVersion
{
    Ttcn = 0x5454434E, //  TTCN (NCTT) Telltale Tool Container Normal? Non-compressed?
    Ttce = 0x54544345, // TTCE (ECTT) Telltale Tool Container Encrypted
    TtCe = 0x54544365, // TTCe (eCTT) Telltale Tool Container Encrypted (supports Oodle compression)
    Ttcz = 0x5454435A, // Telltale Tool Container Zlib
    TtCz = 0x5454437A, // Telltale Tool Container Zlib (supports Oodle compression)
}
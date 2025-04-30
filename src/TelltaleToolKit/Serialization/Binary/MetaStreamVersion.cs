namespace TelltaleToolKit.Serialization.Binary;

public enum MetaStreamVersion : uint
{
    Mbin = 0x4D42494E,
    Mbes = 0x4D424553,
    Mtre = 0x4D545245,
    Mcom = 0x4D434F4D,
    Msv4 = 0x4D535356,
    Msv5 = 0x4D535635,
    Msv6 = 0x4D535636,
    
    // I have no idea why Telltale got these values
    Unknown1 = 0xFB4A1764,
    Unknown2 = 0xEB794091,
    Unknown3 = 0x64AFDEFB,
    Unknown4 = 0x64AFDEAA,
}
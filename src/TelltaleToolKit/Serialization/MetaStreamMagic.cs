namespace TelltaleToolKit.Serialization;

public enum MetaStreamMagic : uint
{
    Mbin = 0x4D42494E,
    Mbes = 0x4D424553,
    Mtre = 0x4D545245,
    Mcom = 0x4D434F4D,
    Msv4 = 0x4D535356,
    Msv5 = 0x4D535635,
    Msv6 = 0x4D535636,

    // I have no idea why Telltale got these values
    EncryptedMbin1 = 0xFB4A1764, // version 2
    EncryptedMbin2 = 0xEB794091, // version 2
    EncryptedMbin3 = 0x64AFDEFB, // version 2
    EncryptedMtre = 0x64AFDEAA, // version 3
    EncryptedMcom = 0x64AFDEBB // version 3
}

public static class MetaStreamVersionExtensions
{
    /// <summary>
    /// Returns the MetaStream version associated with the given magic.
    /// </summary>
    /// <remarks>
    /// The metastream version is the version of the meta stream format.
    /// </remarks>
    /// <param name="magic"></param>
    /// <returns></returns>
    public static uint GetMetaStreamVersion(this MetaStreamMagic magic)
    {
        return magic switch
        {
            MetaStreamMagic.Mbin or MetaStreamMagic.Mbes => 1,
            MetaStreamMagic.EncryptedMbin1 or MetaStreamMagic.EncryptedMbin2 or MetaStreamMagic.EncryptedMbin3 => 2, // no basic version
            MetaStreamMagic.Mtre or MetaStreamMagic.Mcom or MetaStreamMagic.EncryptedMcom or MetaStreamMagic.EncryptedMtre => 3,
            MetaStreamMagic.Msv4 => 4,
            MetaStreamMagic.Msv5 => 5,
            MetaStreamMagic.Msv6 => 6,
            _ => 0
        };
    }
}

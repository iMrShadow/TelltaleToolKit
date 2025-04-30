using System.IO.Hashing;
using System.Text;
using TelltaleToolKit.Reflection;

namespace TelltaleToolKit.Utility;

/// <summary>
/// MetaClass version info (crc32) calculating utility.
/// This is only useful for looking at how the game calculate the versions themselves.
/// </summary>
public static class T3Crc32Utilities
{
    /// <summary>
    /// Calculate CRC32 for a given MetaClass.
    /// This CRC32 is used for the 2016 version of the Telltale Tool.
    /// </summary>
    /// <param name="metaClass"></param>
    /// <returns></returns>
    public static uint CalculateCRC32_2016(MetaClass metaClass)
    {
        // if it's blocked, start with 0xFFFFFFFF (or -1)
        int buf = metaClass.IsBlocked() ? -1 : 0; 
        var crc32 = new Crc32();
        crc32.Append(BitConverter.GetBytes(buf));

        foreach (MetaMember member in metaClass.Members)
        {
            if (member.Flags.HasFlag(MetaFlags.MetaSerializeDisable))
                continue;
            bool isMemberBlocked = member.Type.IsBlocked();

            // Append member Name
            crc32.Append(Encoding.UTF8.GetBytes(member.MemberName)); 
            // Append member Type CRC64
            crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64)); 
            // Append 1 or 0 whether it's blocked or not
            crc32.Append(new[] { Convert.ToByte(isMemberBlocked) }); 
        }

        return crc32.GetCurrentHashAsUInt32();
    }

    /// <summary>
    /// Calculate CRC32 for a given MetaClass.
    /// This CRC32 is used for the 2005 version of the Telltale Tool.
    /// </summary>
    /// <param name="metaClass"></param>
    /// <returns></returns>
    public static uint CalculateCRC32_2005(MetaClass metaClass)
    {
        // Initialize CRC with 0xFFFFFFFF if class is blocked (not NonBlocked), 0 otherwise
        // if it's blocked, start with 0xFFFFFFFF
        uint buf = metaClass.IsBlocked() ? 0xFFFF : 0u; 
        var crc32 = new Crc32();
        crc32.Append(BitConverter.GetBytes(buf));

        foreach (MetaMember member in metaClass.Members)
        {
            if (member.Flags.HasFlag(MetaFlags.MetaSerializeDisable))
                continue;

            crc32.Append(Encoding.UTF8.GetBytes(member.MemberName));
            
            string typeName = member.Type.FullTypeName;
            // Hack for inherited classes. The types of all `Baseclass_` members are pointers.
            // Presumably that is fixed after the first MBIN games.
            if (member.MemberName.StartsWith("Baseclass_"))
            {
                typeName += " *";
            }
            crc32.Append(Encoding.UTF8.GetBytes(typeName));
        }

        return crc32.GetCurrentHashAsUInt32();
    }

    /// <summary>
    /// Calculate CRC32 for a given MetaClass.
    /// This CRC32 is used for the 2008 version of the Telltale Tool.
    /// </summary>
    /// <param name="metaClass"></param>
    /// <returns></returns>
    public static uint CalculateCRC32_2008(MetaClass metaClass)
    {
        int buf = metaClass.IsBlocked() ? -1 : 0; // if it's blocked, start with 0xFFFFFFFF (or -1)
        var crc32 = new Crc32();
        crc32.Append(BitConverter.GetBytes(buf));

        foreach (MetaMember member in from member in metaClass.Members where !member.Flags.HasFlag(MetaFlags.MetaSerializeDisable) let isMemberBlocked = member.Type.IsBlocked() select member)
        {
            crc32.Append(Encoding.UTF8.GetBytes(member.MemberName)); // Append member name
            crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64)); // TODO: Get CRC32
        }

        return crc32.GetCurrentHashAsUInt32();
    }

    /// <summary>
    /// Calculate CRC32 for a given MetaClass.
    /// This CRC32 is used for the 2013 version of the Telltale Tool.
    /// </summary>
    /// <param name="metaClass"></param>
    /// <returns></returns>
    public static uint CalculateCRC32_2013(MetaClass metaClass)
    {
        int buf = metaClass.IsBlocked() ? -1 : 0; // if it's blocked, start with 0xFFFFFFFF (or -1)
        var crc32 = new Crc32();
        crc32.Append(BitConverter.GetBytes(buf));

        foreach (MetaMember member in metaClass.Members)
        {
            if (member.Flags.HasFlag(MetaFlags.MetaSerializeDisable))
                continue;

            bool isMemberBlocked = member.Type.IsBlocked();

            crc32.Append(Encoding.UTF8.GetBytes(member.MemberName)); // Append member name
            crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64)); // Append member Type CRC64
            crc32.Append(new[] { Convert.ToByte(isMemberBlocked) }); // Append 1 or 0 whether it's blocked or not
            crc32.Append(BitConverter.GetBytes(member.Type.Symbol.Crc64)); // TODO: Get CRC32
        }

        return crc32.GetCurrentHashAsUInt32();
    }
}
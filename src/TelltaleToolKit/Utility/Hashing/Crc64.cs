using System.Diagnostics;

namespace TelltaleToolKit.Utility.Hashing;

public static class Crc64
{
    private const ulong Polynomial = 0x42F0E1EBA9EA3693UL;
    private static readonly ulong[] s_table = BuildTable();

    private static ulong[] BuildTable()
    {
        ulong[] table = new ulong[256];
        for (int i = 0; i < 256; i++)
        {
            ulong crc = (ulong)i << 56;
            for (int j = 0; j < 8; j++)
                crc = (crc & (1UL << 63)) != 0
                    ? (crc << 1) ^ Polynomial
                    : crc << 1;
            table[i] = crc;
        }

        return table;
    }

    /// <summary>
    /// Computes a CRC64 hash of an ASCII string, lowercased, using the
    /// ECMA-182 polynomial (0x42F0E1EBA9EA3693) with init=0 and no final XOR.
    /// Zero allocations. Input must be ASCII-only.
    /// </summary>
    /// <remarks>
    /// This method uses the standard CRC64 algorithm consistent across all Telltale Tool games.
    /// The input is converted to lowercase before hashing, making the hash case-insensitive.
    /// </remarks>
    public static ulong Compute(string? input)
    {
        if (input == null)
            return 0;

        Debug.Assert(IsAscii(input), "Input must be ASCII-only");

        ulong crc = 0;
        foreach (char c in input)
        {
            byte b = (byte)(c | ((uint)(c - 'A') < 26u ? 0x20u : 0u));
            crc = (crc << 8) ^ s_table[(byte)((crc >> 56) ^ b)];
        }

        return crc;
    }

    private static bool IsAscii(string s)
    {
#if NET8_0_OR_GREATER
        return s.All(char.IsAscii);
#else
        foreach (char c in s)
            if (c > 127) return false;
        return true;
#endif
    }
}

using System.Buffers.Binary;
using System.Text;

namespace TelltaleToolKit.Utility.Blowfish;

/// <summary>
/// Class that provides 'Blowfish' encryption.
/// <para> There are two versions:</para> 
/// - The regular Blowfish encryption which is used in older games until ttarch version 6.<br />
/// - The modified Blowfish encryption which is used in all new games.
/// </summary>
public class Blowfish
{
    private const int N = 16;

    private readonly uint[] _p;
    private readonly uint[,] _s;
    private readonly bool _isModifiedBlowfishEncryption;

    /// <summary>
    /// Constructs and initializes a blowfish instance with the supplied key.
    /// </summary>
    /// <param name="key">The key to cipher with.</param>
    /// <param name="version"></param>
    public Blowfish(Span<byte> key, int version)
    {
        _isModifiedBlowfishEncryption = version >= 7;

        _p = BlowfishConstants.P.Clone() as uint[] ?? throw new InvalidOperationException();
        _s = BlowfishConstants.S.Clone() as uint[,] ?? throw new InvalidOperationException();

        for (short i = 0, j = 0; i < N + 2; ++i)
        {
            uint data = 0x00000000;
            for (short k = 0; k < 4; ++k)
            {
                data = (data << 8) | key[j];
                j = (short)((j + 1) % key.Length);
            }

            _p[i] ^= data;
        }

        if (_isModifiedBlowfishEncryption)
        {
            _s[0, 118] = BinaryPrimitives.ReverseEndianness(_s[0, 118]);
        }

        uint dataLeft = 0x00000000;
        uint dataRight = 0x00000000;

        for (short i = 0; i < N + 2; i += 2)
        {
            Encipher(ref dataLeft, ref dataRight);
            _p[i] = dataLeft;
            _p[i + 1] = dataRight;
        }

        for (short i = 0; i < 4; ++i)
        {
            for (short j = 0; j < 256; j += 2)
            {
                Encipher(ref dataLeft, ref dataRight);

                _s[i, j] = dataLeft;
                _s[i, j + 1] = dataRight;
            }
        }
    }

    /// <summary>
    /// Constructs and initializes a blowfish instance with the supplied key.
    /// </summary>
    /// <param name="key">The key to cipher with.</param>
    /// <param name="version"></param>
    public Blowfish(string key, int version)
        : this(Encoding.GetEncoding("ISO-8859-1").GetBytes(key), version)
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private uint F(uint x)
    {
        var d = (ushort)(x & 0x00FF);
        x >>= 8;
        var c = (ushort)(x & 0x00FF);
        x >>= 8;
        var b = (ushort)(x & 0x00FF);
        x >>= 8;
        var a = (ushort)(x & 0x00FF);
        // y = ((S[0][a] + S[1][b]) ^ S[2][c]) + S[3][d];
        uint y = _s[0, a] + _s[1, b];
        y ^= _s[2, c];
        y += _s[3, d];

        return y;
    }

    /// <summary>
    /// Encrypts a byte array in place.
    /// </summary>
    /// <param name="data">The array to encrypt.</param>
    /// <param name="length">The amount to encrypt.</param>
    public void Encipher(Span<byte> data, int length)
    {
        int fullBlocks = length - length % 8; // Ensure only complete blocks are processed

        for (var i = 0; i < fullBlocks; i += 8)
        {
            var xl = (uint)((data[i + 3] << 24) | (data[i + 2] << 16) | (data[i + 1] << 8) | data[i]);
            var xr = (uint)((data[i + 7] << 24) | (data[i + 6] << 16) | (data[i + 5] << 8) | data[i + 4]);

            if (_isModifiedBlowfishEncryption)
            {
                Encipher7(ref xl, ref xr);
            }
            else
            {
                Encipher(ref xl, ref xr);
            }

            data[i] = (byte)(xl >> 24);
            data[i + 1] = (byte)(xl >> 16);
            data[i + 2] = (byte)(xl >> 8);
            data[i + 3] = (byte)(xl);
            data[i + 4] = (byte)(xr >> 24);
            data[i + 5] = (byte)(xr >> 16);
            data[i + 6] = (byte)(xr >> 8);
            data[i + 7] = (byte)(xr);
        }
    }

    /// <summary>
    /// Encrypts 8 bytes of data (1 block)
    /// </summary>
    /// <param name="xl">The left part of the 8 bytes.</param>
    /// <param name="xr">The right part of the 8 bytes.</param>
    private void Encipher(ref uint xl, ref uint xr)
    {
        uint xlCopy = xl;
        uint xrCopy = xr;

        for (short i = 0; i < N; ++i)
        {
            xlCopy ^= _p[i];
            xrCopy = F(xlCopy) ^ xrCopy;

            (xlCopy, xrCopy) = (xrCopy, xlCopy);
        }

        (xlCopy, xrCopy) = (xrCopy, xlCopy);

        xrCopy ^= _p[N];
        xlCopy ^= _p[N + 1];

        xl = xlCopy;
        xr = xrCopy;
    }

    /// <summary>
    /// Encrypts 8 bytes of data (1 block)
    /// </summary>
    /// <param name="xl">The left part of the 8 bytes.</param>
    /// <param name="xr">The right part of the 8 bytes.</param>
    private void Encipher7(ref uint xl, ref uint xr)
    {
        uint xlCopy = xl;
        uint xrCopy = xr;

        for (short i = 0; i < N; ++i)
        {
            uint temp = i switch
            {
                // version 7
                1 => _p[3],
                2 => _p[4],
                3 => _p[1],
                4 => _p[2],
                _ => _p[i]
            };

            xlCopy ^= temp;
            xrCopy = F(xlCopy) ^ xrCopy;

            (xlCopy, xrCopy) = (xrCopy, xlCopy);
        }

        (xlCopy, xrCopy) = (xrCopy, xlCopy);

        xrCopy ^= _p[N];
        xlCopy ^= _p[N + 1];

        xl = xlCopy;
        xr = xrCopy;
    }

    /// <summary>
    /// Encrypts a string.
    /// </summary>
    /// <param name="data">The string to encrypt</param>
    /// <returns>Encrypted string</returns>
    public string Encipher(string data)
    {
        byte[] b = Encoding.Unicode.GetBytes(data);
        Encipher(b, b.Length);

        return Convert.ToBase64String(b);
    }

    /// <summary>
    /// Decrypts a byte array in place.
    /// </summary>
    /// <param name="data">The array to decrypt.</param>
    /// <param name="length">The amount to decrypt.</param>
    public void Decipher(Span<byte> data, int length)
    {
        // if ((length % 8) != 0)
        // Ensure only complete blocks are processed
        int fullBlocks = length - length % 8;

        for (var i = 0; i < fullBlocks; i += 8)
        {
            var xl = (uint)((data[i + 3] << 24) | (data[i + 2] << 16) | (data[i + 1] << 8) | data[i]);
            var xr = (uint)((data[i + 7] << 24) | (data[i + 6] << 16) | (data[i + 5] << 8) | data[i + 4]);
            Decipher(ref xl, ref xr);

            data[i + 3] = (byte)(xl >> 24);
            data[i + 2] = (byte)(xl >> 16);
            data[i + 1] = (byte)(xl >> 8);
            data[i + 0] = (byte)xl;
            data[i + 7] = (byte)(xr >> 24);
            data[i + 6] = (byte)(xr >> 16);
            data[i + 5] = (byte)(xr >> 8);
            data[i + 4] = (byte)xr;
        }
    }

    /// <summary>
    /// Decrypts 8 bytes of data (1 block)
    /// </summary>
    /// <param name="xl">The left part of the 8 bytes.</param>
    /// <param name="xr">The right part of the 8 bytes.</param>
    private void Decipher(ref uint xl, ref uint xr)
    {
        uint xlCopy = xl;
        uint xrCopy = xr;

        for (short i = N + 1; i > 1; --i)
        {
            uint temp;
            if (_isModifiedBlowfishEncryption)
            {
                temp = i switch
                {
                    // Modified Blowfish - Version >= 7
                    4 => _p[2],
                    3 => _p[1],
                    2 => _p[4],
                    _ => _p[i]
                };
            }
            else
            {
                temp = _p[i];
            }

            xlCopy ^= temp;
            xrCopy = F(xlCopy) ^ xrCopy;

            // Exchange Xl and Xr
            (xlCopy, xrCopy) = (xrCopy, xlCopy);
        }

        // Exchange Xl and Xr
        (xlCopy, xrCopy) = (xrCopy, xlCopy);

        xrCopy ^= _isModifiedBlowfishEncryption ? _p[3] : _p[1]; // version7
        xlCopy ^= _p[0];

        (xl, xr) = (xlCopy, xrCopy);
    }
}
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<Datestamp>))]
public class Datestamp
{
    [MetaMember("mSec")]
    public byte Sec { get; set; }

    [MetaMember("mMin")]
    public byte Min { get; set; }

    [MetaMember("mHour")]
    public byte Hour { get; set; }

    [MetaMember("mMday")]
    public byte Mday { get; set; }

    [MetaMember("mMon")]
    public byte Mon { get; set; }

    [MetaMember("mYear")]
    public byte Year { get; set; }

    [MetaMember("mWday")]
    public byte Wday { get; set; }

    [MetaMember("mYday")]
    public ushort Yday { get; set; }

    [MetaMember("mIsdst")]
    public byte Isdst { get; set; }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<DateStamp>))]
public class DateStamp
{
    [MetaMember("mSec")]
    public byte Sec { get; set; }

    [MetaMember("mMin")]
    public byte Min { get; set; }

    [MetaMember("mHour")]
    public byte Hour { get; set; }

    [MetaMember("mMday")]
    public byte Day { get; set; }

    [MetaMember("mMon")]
    public byte Month { get; set; }

    [MetaMember("mYear")]
    public byte Year { get; set; }

    [MetaMember("mWday")]
    public byte Wday { get; set; }

    [MetaMember("mYday")]
    public ushort Yday { get; set; }

    [MetaMember("mIsdst")]
    public byte Isdst { get; set; }
}

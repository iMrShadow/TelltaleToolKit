using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Miscellaneous;

[MetaSerializer(typeof(MetaClassSerializer<EnumRecordingStatus>))]
public struct EnumRecordingStatus
{
    [MetaMember("mVal")]
    public RecordingStatus Val { get; set; }

    [MetaSerializer(typeof(EnumSerializer<RecordingStatus>))]
    public enum RecordingStatus
    {
        None = 0,
        AtStudio = 1,
        Recorded = 2,
        Delivered = 3
    }
}

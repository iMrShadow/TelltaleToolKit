using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class CompressedTimeKeys
{
    public byte[] BitBuffer { get; set; } = [];
    public byte[] Buffer { get; set; } = [];

    public int CurrentSample { get; set; } = -1;
    public int CurrentSampleInBlock { get; set; } = 1;
    public float CurrentDelta { get; set; } = 0.033333335f;
    public float CurrentTime { get; set; } = -0.033333335f;
    public float PreviousTime { get; set; } = -0.033333335f;
    public int NumBits { get; set; } = -1;

    public class Serializer : MetaSerializer<CompressedTimeKeys>
    {
        public override void PreSerialize(ref CompressedTimeKeys? obj, MetaStream stream, MetaClassType? type = null) =>
            obj ??= new CompressedTimeKeys();

        public override void Serialize(ref CompressedTimeKeys obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else
            {
                byte first = stream.ReadByte();
                int len;
                if (first == 0xFF)
                    len = stream.ReadUInt16();
                else
                    len = first;

                obj.Buffer = stream.ReadBytes((len * 8 + 7) / 8);
            }
        }
    }
}

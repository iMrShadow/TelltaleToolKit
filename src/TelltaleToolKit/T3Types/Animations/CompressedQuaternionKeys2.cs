using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class CompressedQuaternionKeys2 : IAnimationValueInterface
{
    public byte[] Buffer { get; set; } = [];

    public CompressedTimeKeys CompressedTimeKeys { get; set; }

    public class Serializer : MetaSerializer<CompressedQuaternionKeys2>
    {
        public override void PreSerialize(ref CompressedQuaternionKeys2? obj, MetaStream stream,
            MetaClassType? type = null) =>
            obj ??= new CompressedQuaternionKeys2();

        public override void Serialize(ref CompressedQuaternionKeys2 obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else
            {
                byte first = stream.ReadByte();
                int len;
                if (first == 255)
                    len = stream.ReadUInt16();
                else
                    len = first;

                obj.Buffer = stream.ReadBytes((len * 8 + 7) / 8);

                CompressedTimeKeys compressedTimeKeys = new();
                stream.Serialize(ref compressedTimeKeys);
                obj.CompressedTimeKeys = compressedTimeKeys;
            }
        }
    }

    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; } = new();
}

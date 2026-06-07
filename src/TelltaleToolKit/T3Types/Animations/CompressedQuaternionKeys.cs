using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Animations;

[MetaSerializer(typeof(Serializer))]
public class CompressedQuaternionKeys
{
    public string Name { get; set; } = string.Empty;
    public byte[] Buffer { get; set; } = [];
    public float MinTime { get; set; }
    public float MaxTime { get; set; }
    public int Flags { get; set; }


    public class Serializer : MetaSerializer<CompressedQuaternionKeys>
    {
        public override void Serialize(ref CompressedQuaternionKeys obj, MetaStream stream, MetaClassType? type = null)
        {
            // TODO: Test this type.

            if (stream.Mode is MetaStreamMode.Write)
            {
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                obj.Name = stream.ReadString();
                obj.Flags = stream.ReadInt32();
                obj.MinTime = stream.ReadSingle();
                obj.MaxTime = stream.ReadSingle();

                short numSamples = stream.ReadInt16();

                obj.Buffer = stream.ReadBytes(numSamples * 6);
            }
        }
    }
}

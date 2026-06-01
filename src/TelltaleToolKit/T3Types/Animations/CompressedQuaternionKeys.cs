using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Animations;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class CompressedQuaternionKeys
{
    public string Name { get; set; } = string.Empty;
    public byte[] Buffer { get; set; } = [];
    public float MinTime { get; set; }
    public float MaxTime { get; set; }
    public int Flags { get; set; }


    public class Serializer : MetaClassSerializer<CompressedQuaternionKeys>
    {
        public override void Serialize(ref CompressedQuaternionKeys obj, MetaStream stream)
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

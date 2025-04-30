using TelltaleToolKit.Reflection;
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

            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.Name = streamReader.ReadString();
                obj.Flags = streamReader.ReadInt32();
                obj.MinTime = streamReader.ReadSingle();
                obj.MaxTime = streamReader.ReadSingle();

                short numSamples = streamReader.ReadInt16();

                obj.Buffer = streamReader.ReadBytes(numSamples * 6);
            }
        }
    }
}
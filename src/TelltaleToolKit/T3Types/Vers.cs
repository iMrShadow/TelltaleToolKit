using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types;

public class Vers
{
    public string FileName { get; set; }
    public string ClassName { get; set; }
    public uint Crc32 { get; set; }
    public uint Size { get; set; }
    public bool IsBlocked { get; set; }

    public List<MemberRegistry> Registries { get; set; } = [];

    public class MemberRegistry
    {
        public string MemberName { get; set; }
        public string MemberType { get; set; }
        public uint Size { get; set; }
        public bool IsBlocked { get; set; }

        public uint Crc32 { get; set; }

        public override string ToString() =>
            $"{MemberName}:{MemberType}:{Size}:{IsBlocked}:{Crc32}";
    }

    // TODO: Convert this to the meta type system.
    [MetaClassSerializerGlobal(typeof(VersSerializer))]
    public class VersSerializer : MetaClassSerializer<Vers>
    {
        public override void Serialize(ref Vers obj, MetaStream stream)
        {
            PreSerialize(ref obj, stream, null);

            if (stream is MetaStreamWriter streamWriter)
            {
                streamWriter.Write(obj.FileName);
                streamWriter.Write(obj.ClassName);
                streamWriter.Write(obj.Crc32);
                streamWriter.Write(obj.Size);
                streamWriter.Write(obj.IsBlocked);
                streamWriter.Write(obj.Crc32);
                streamWriter.Write(obj.Registries.Count);

                for (int i = 0; i < obj.Registries.Capacity; i++)
                {
                    streamWriter.Write(obj.Registries[i].MemberName);
                    streamWriter.Write(obj.Registries[i].MemberType);
                    streamWriter.Write(obj.Registries[i].Size);
                    streamWriter.Write(obj.Registries[i].IsBlocked);
                    streamWriter.Write(obj.Registries[i].Crc32);
                }
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.FileName = streamReader.ReadString();
                obj.ClassName = streamReader.ReadString();
                obj.Crc32 = streamReader.ReadUInt32();
                obj.Size = streamReader.ReadUInt32();
                obj.IsBlocked = streamReader.ReadBoolean();

                obj.Registries.Capacity = streamReader.ReadInt32();

                for (int i = 0; i < obj.Registries.Capacity; i++)
                {
                    obj.Registries.Add(new MemberRegistry());
                    obj.Registries[i].MemberName = streamReader.ReadString();
                    obj.Registries[i].MemberType = streamReader.ReadString();
                    obj.Registries[i].Size = streamReader.ReadUInt32();
                    obj.Registries[i].IsBlocked = streamReader.ReadBoolean();
                    obj.Registries[i].Crc32 = streamReader.ReadUInt32();
                }
            }
        }
    }
}

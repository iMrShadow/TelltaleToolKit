using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

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
    [MetaSerializer(typeof(VersSerializer))]
    public class VersSerializer : MetaSerializer<Vers>
    {
        public override void Serialize(ref Vers obj, MetaStream stream, MetaClassType? type = null)
        {
            PreSerialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.FileName);
                stream.Write(obj.ClassName);
                stream.Write(obj.Crc32);
                stream.Write(obj.Size);
                stream.Write(obj.IsBlocked);
                stream.Write(obj.Crc32);
                stream.Write(obj.Registries.Count);

                for (int i = 0; i < obj.Registries.Capacity; i++)
                {
                    stream.Write(obj.Registries[i].MemberName);
                    stream.Write(obj.Registries[i].MemberType);
                    stream.Write(obj.Registries[i].Size);
                    stream.Write(obj.Registries[i].IsBlocked);
                    stream.Write(obj.Registries[i].Crc32);
                }
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                obj.FileName = stream.ReadString();
                obj.ClassName = stream.ReadString();
                obj.Crc32 = stream.ReadUInt32();
                obj.Size = stream.ReadUInt32();
                obj.IsBlocked = stream.ReadBoolean();

                obj.Registries.Capacity = stream.ReadInt32();

                for (int i = 0; i < obj.Registries.Capacity; i++)
                {
                    obj.Registries.Add(new MemberRegistry());
                    obj.Registries[i].MemberName = stream.ReadString();
                    obj.Registries[i].MemberType = stream.ReadString();
                    obj.Registries[i].Size = stream.ReadUInt32();
                    obj.Registries[i].IsBlocked = stream.ReadBoolean();
                    obj.Registries[i].Crc32 = stream.ReadUInt32();
                }
            }
        }
    }
}

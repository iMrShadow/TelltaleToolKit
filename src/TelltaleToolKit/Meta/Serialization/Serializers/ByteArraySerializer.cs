using TelltaleToolKit.Meta.Reflection;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(ByteArraySerializer))]
public class ByteArraySerializer : MetaSerializer<byte[]>
{
    public override void Serialize(ref byte[] obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Write)
            stream.Write(obj);
        else if (stream.Mode is MetaStreamMode.Read)
        {
            int size = stream.ReadInt32();
            obj = stream.ReadBytes(size);
        }
    }
}

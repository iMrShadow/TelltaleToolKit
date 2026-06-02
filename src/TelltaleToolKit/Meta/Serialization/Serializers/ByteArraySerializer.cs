namespace TelltaleToolKit.Meta.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(ByteArraySerializer))]
public class ByteArraySerializer : MetaClassSerializer<byte[]>
{
    public override void Serialize(ref byte[] obj, MetaStream stream)
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

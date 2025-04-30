using System.IO;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types;

public struct HandleObjectInfo
{
    [MetaMember("mObjectName")]
    public Symbol ObjectName;
    [MetaMember("mFlags")]
    public Flags Flags;

    public override string ToString()
    {
        return $"Symbol: {ObjectName}";
    }
}

// public class HandleLock<T> : HandleBase
// {
// }

public class HandleBase
{
    // public Symbol ObjectName;
    public HandleObjectInfo ObjectInfo;

    [MetaClassSerializerGlobal(typeof(HandleBaseSerializer))]
    public class HandleBaseSerializer : MetaClassSerializer<HandleBase>
    {
        public override void PreSerialize(ref HandleBase obj, MetaStream stream, MetaClassType? type = null)
        {
            if (obj is null)
            {
                obj = new HandleBase();
            }
        }
        
        public override void Serialize(ref HandleBase obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                // stream.Serialize(ref (object)handle.ObjectInfo.mObjectName, description);
                streamWriter.Write(obj.ObjectInfo.ObjectName);
            }
            else if (stream is MetaStreamReader streamReader)
            {
                obj.ObjectInfo.ObjectName = streamReader.ReadSymbol();
                // Console.WriteLine(obj.ObjectInfo.ObjectName.Crc64);
            }
        }
    }

    public override string ToString()
    {
        return $"Handle: {ObjectInfo}";
    }
}
[MetaClassSerializerGlobal(typeof(HandleSerializer<>), typeof(Handle<>))]
public class Handle<T> : HandleBase
{

}

public class HandleSerializer<T> : MetaClassSerializer<Handle<T>>
{
    
    public override void PreSerialize(ref Handle<T> obj, MetaStream stream, MetaClassType? type = null)
    {
        obj = new Handle<T>();
    }
    
    public override void Serialize(ref Handle<T> obj, MetaStream stream)
    {
        HandleBase handle = obj;
        TTKContext.Instance().GetSerializer<HandleBase>().PreSerialize(ref handle, stream);
        TTKContext.Instance().GetSerializer<HandleBase>().Serialize(ref handle, stream);
    }
}
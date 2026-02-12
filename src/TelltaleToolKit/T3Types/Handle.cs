using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types;

public class HandleObjectInfo
{
    public Symbol ObjectName = new Symbol(0);
    public Flags Flags;
    public MetaClassType Type;
    public object HandleObject;

    public override string ToString()
    {
        return $"Symbol: {ObjectName}";
    }
}

public class HandleBase
{
    // public Symbol ObjectName;
    public HandleObjectInfo ObjectInfo = new HandleObjectInfo();

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
            if (stream.Configuration.Version is MetaStreamVersion.Msv4 or MetaStreamVersion.Msv5
                or MetaStreamVersion.Msv6)
            {
                stream.Serialize(ref obj.ObjectInfo.ObjectName);
            }
            else
            {
                var name = string.Empty;
                stream.Serialize(ref name);
                obj.ObjectInfo.ObjectName = new Symbol(name);
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

public class HandleLock<T> : Handle<T> 
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
        Toolkit.Instance.GetSerializer<HandleBase>().PreSerialize(ref handle, stream);
        Toolkit.Instance.GetSerializer<HandleBase>().Serialize(ref handle, stream);
    }
}
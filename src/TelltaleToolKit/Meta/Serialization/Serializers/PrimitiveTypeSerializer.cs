using TelltaleToolKit.Meta.Reflection;

namespace TelltaleToolKit.Meta.Serialization.Serializers;

[MetaSerializer(typeof(CharSerializer))]
public sealed class CharSerializer : MetaSerializer<char>
{
    /// <inheritdoc/>
    public override void Serialize(ref char obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(BoolSerializer))]
public sealed class BoolSerializer : MetaSerializer<bool>
{
    /// <inheritdoc/>
    public override void Serialize(ref bool obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(SByteSerializer))]
public sealed class SByteSerializer : MetaSerializer<sbyte>
{
    /// <inheritdoc/>
    public override void Serialize(ref sbyte obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(ByteSerializer))]
public sealed class ByteSerializer : MetaSerializer<byte>
{
    /// <inheritdoc/>
    public override void Serialize(ref byte obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(Int16Serializer))]
public sealed class Int16Serializer : MetaSerializer<short>
{
    /// <inheritdoc/>
    public override void Serialize(ref short obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(UInt16Serializer))]
public sealed class UInt16Serializer : MetaSerializer<ushort>
{
    /// <inheritdoc/>
    public override void Serialize(ref ushort obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(Int32Serializer))]
public sealed class Int32Serializer : MetaSerializer<int>
{
    /// <inheritdoc/>
    public override void Serialize(ref int obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(UInt32Serializer))]
public sealed class UInt32Serializer : MetaSerializer<uint>
{
    /// <inheritdoc/>
    public override void Serialize(ref uint obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(Int64Serializer))]
public sealed class Int64Serializer : MetaSerializer<long>
{
    /// <inheritdoc/>
    public override void Serialize(ref long obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(UInt64Serializer))]
public sealed class UInt64Serializer : MetaSerializer<ulong>
{
    /// <inheritdoc/>
    public override void Serialize(ref ulong obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(SingleSerializer))]
public sealed class SingleSerializer : MetaSerializer<float>
{
    /// <inheritdoc/>
    public override void Serialize(ref float obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(DoubleSerializer))]
public sealed class DoubleSerializer : MetaSerializer<double>
{
    /// <inheritdoc/>
    public override void Serialize(ref double obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }
}

[MetaSerializer(typeof(StringSerializer))]
public sealed class StringSerializer : MetaSerializer<string>
{
    /// <inheritdoc/>
    public override void Serialize(ref string obj, MetaStream stream, MetaClassType? type = null)
    {
        stream.Serialize(ref obj);
    }

    /// <inheritdoc/>
    public override void PreSerialize(ref string? obj, MetaStream stream, MetaClassType? type)
    {
    }
}

public sealed class EnumSerializer<TEnum> : MetaSerializer<TEnum> where TEnum : Enum
{
    /// <inheritdoc/>
    public override void Serialize(ref TEnum obj, MetaStream stream, MetaClassType? type = null)
    {
        if (stream.Mode is MetaStreamMode.Write)
        {
            stream.Write(Convert.ToInt32(obj));
        }
        else if (stream.Mode is MetaStreamMode.Read)
        {
            obj = (TEnum)Enum.ToObject(typeof(TEnum), stream.ReadInt32());
        }
    }
}

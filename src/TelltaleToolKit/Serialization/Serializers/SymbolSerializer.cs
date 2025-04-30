using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types;

namespace TelltaleToolKit.Serialization.Serializers;

[MetaClassSerializerGlobal(typeof(SymbolSerializer))]
public class SymbolSerializer : MetaClassSerializer<Symbol>
{
    public override void PreSerialize(ref Symbol obj, MetaStream stream, MetaClassType? type = null)
    {
        obj = Symbol.DefaultSymbol;
    }
    
    public override void Serialize(ref Symbol obj, MetaStream stream)
    {
        stream.Serialize(ref obj);
    }
}
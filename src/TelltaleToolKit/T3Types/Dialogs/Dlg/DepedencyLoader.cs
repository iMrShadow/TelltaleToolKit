using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DependencyLoader
{
    public List<string> ResourceNames { get; set; } = [];

    public class Serializer : MetaClassSerializer<DependencyLoader>
    {
        public override void Serialize(ref DependencyLoader obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                bool hasResourceNames = streamReader.ReadBoolean();

                if (!hasResourceNames) 
                    return;
                
                // This is a little bit weird.
                // In the real serialization function, the object is base-casted to DCArray<String>. (base-casting is casting to a parent class).
                // Which leads to the question - which type does inherit DCArray<String>? It does not make any sense.
                MetaClassType type = streamReader.ReadMetaClassType();

                if (type.Symbol.SymbolName != null && !type.Symbol.SymbolName.Equals("DCArray<String>"))
                {
                    Console.WriteLine("Type `DepedencyLoader` does not serialize DCArray<String>!");
                }
                
                List<string> objResourceNames = obj.ResourceNames;
                TTK.PreSerialize(ref objResourceNames, stream);
                TTK.Serialize(ref objResourceNames, stream);
            }
        }
    }
}
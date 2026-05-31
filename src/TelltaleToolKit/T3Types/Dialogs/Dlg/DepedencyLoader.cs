using System.Diagnostics;
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
            if (stream is BinaryMetaStreamWriter)
            {
                throw new NotImplementedException();
            }

            if (stream is BinaryMetaStreamReader streamReader)
            {
                bool hasResourceNames = streamReader.ReadBoolean();

                if (!hasResourceNames)
                    return;

                // This is a little bit weird.
                // In the real serialization function, the object is base-casted to DCArray<String>. (base-casting is casting to a parent class).
                // Which leads to the question - which type does inherit DCArray<String>? It does not make any sense.
                MetaClassType? type = streamReader.ReadMetaClassType();

                if (type == null)
                    throw new InvalidOperationException("[DependencyLoader] Type is not registered.");

                if (type.Symbol.DebugString != null && !type.Symbol.DebugString.Equals("DCArray<String>"))
                {
                    Toolkit.Instance.Logger.LogWarning($"[DependencyLoader] Unknown type `{type.Symbol.DebugString}`!");
                }

                List<string> objResourceNames = obj.ResourceNames;
                stream.Serialize(ref objResourceNames);
            }
        }
    }
}

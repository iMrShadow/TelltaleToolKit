using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class DependencyLoader
{
    public List<string>? ResourceNames { get; set; }

    public class Serializer : MetaSerializer<DependencyLoader>
    {
        public override void PreSerialize(ref DependencyLoader? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new DependencyLoader();
        }

        public override void Serialize(ref DependencyLoader obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                bool hasResourceNames = obj.ResourceNames != null;
                stream.Write(hasResourceNames);

                if (!hasResourceNames)
                    return;

                var classType = MetaClassTypeRegistry.GetByName("DCArray<String>");
                stream.Write(classType);

                List<string>? objResourceNames = obj.ResourceNames;
                stream.Serialize(ref objResourceNames);
            }
            else
            {
                bool hasResourceNames = stream.ReadBoolean();

                if (!hasResourceNames)
                    return;

                // This is a little bit weird.
                // In the real serialization function, the object is base-casted to DCArray<String>. (base-casting is casting to a parent class).
                // Which leads to the question - which type does inherit DCArray<String>? It does not make any sense.
                MetaClassType? typeS = stream.ReadMetaClassType();

                if (typeS == null)
                    throw new InvalidOperationException("[DependencyLoader] Type is not registered.");

                if (typeS.Symbol.DebugString != null && !typeS.Symbol.DebugString.Equals("DCArray<String>"))
                {
                    Toolkit.Instance.Logger.LogWarning(
                        $"[DependencyLoader] Unknown type `{typeS.Symbol.DebugString}`!");
                }

                obj.ResourceNames = [];
                List<string> objResourceNames = obj.ResourceNames;
                stream.Serialize(ref objResourceNames);
            }
        }
    }
}

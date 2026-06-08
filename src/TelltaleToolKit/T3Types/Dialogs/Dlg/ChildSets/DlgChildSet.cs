using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class DlgChildSet
{
    [MetaMember("mChildren")]
    public List<IDlgChild> Children { get; set; } = [];

    [MetaMember("mParent")]
    public DlgNodeLink Parent { get; set; }

    public class Serializer : MetaSerializer<DlgChildSet>
    {
        public override void PreSerialize(ref DlgChildSet? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new DlgChildSet();
        }

        public override void Serialize(ref DlgChildSet obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotSupportedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int numChildren = stream.ReadInt32();

                for (var i = 0; i < numChildren; i++)
                {
                    MetaClassType? typeS = stream.ReadMetaClassType();
                    if (typeS == null)
                        throw new InvalidOperationException("[DlgChildSet] Type is not registered.");

                    MetaSerializer childSerializer = Toolkit.Instance.GetSerializer(typeS.LinkingType);

                    object? dlgChild = null;
                    childSerializer.PreSerialize(ref dlgChild, stream, typeS);
                    childSerializer.Serialize(ref dlgChild, stream, typeS);

                    if (dlgChild is IDlgChild child)
                        obj.Children.Add(child);
                }
            }
        }
    }
}

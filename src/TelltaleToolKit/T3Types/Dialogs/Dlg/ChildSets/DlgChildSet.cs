using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
public class DlgChildSet
{
    [MetaMember("mChildren")]
    public List<IDlgChild> Children { get; set; } = [];

    [MetaMember("mParent")]
    public DlgNodeLink Parent { get; set; }

    public class Serializer : MetaClassSerializer<DlgChildSet>
    {
        public override void PreSerialize(ref DlgChildSet obj, MetaStream stream, MetaClassType? type = null)
        {
            if (stream is MetaStreamReader)
            {
                obj = new DlgChildSet();
            }
        }

        public override void Serialize(ref DlgChildSet obj, MetaStream stream)
        {
            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotSupportedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                int numChildren = streamReader.ReadInt32();

                for (var i = 0; i < numChildren; i++)
                {
                    MetaClassType type = streamReader.ReadMetaClassType();
                    MetaClassSerializer childSerializer = TTKContext.Instance().GetSerializer(type.LinkingType);

                    object? dlgChild = null;
                    childSerializer.PreSerialize(ref dlgChild, stream);
                    childSerializer.Serialize(ref dlgChild, stream);

                    if (dlgChild is IDlgChild child)
                        obj.Children.Add(child);
                }
            }
        }
    }
}
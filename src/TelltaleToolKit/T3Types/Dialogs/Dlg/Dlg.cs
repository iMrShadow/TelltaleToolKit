using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaSerializer(typeof(Serializer))]
public class Dlg : IDlgObjIdOwner, ITaskOwner
{
    [MetaMember("Baseclass_DlgObjIDOwner")]
    public DlgObjIDOwner DlgObjIdOwner { get; set; }

    [MetaMember("Baseclass_TaskOwner")]
    public TaskOwner TaskOwner { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; }

    [MetaMember("mVersion")]
    public int Version { get; set; }

    [MetaMember("mDefFolderID")]
    public DlgObjId DefFolderID { get; set; }

    [MetaMember("mLangDB")]
    public LanguageDb LangDB { get; set; }

    [MetaMember("mProjectID")]
    public uint ProjectID { get; set; }

    [MetaMember("mResourceLocationID")]
    public Symbol ResourceLocationID { get; set; }

    [MetaMember("mChronology")]
    public int Chronology { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mDependencies")]
    public DependencyLoader Dependencies { get; set; }

    [MetaMember("mProdReportProps")]
    public PropertySet ProdReportProps { get; set; }

    [MetaMember("mJiraRecordManager")]
    public JiraRecordManager JiraRecordManager { get; set; }

    [MetaMember("mbHasToolOnlyData")]
    public bool HasToolOnlyData { get; set; }

    public bool ToolFlag { get; set; }

    public List<DlgFolder> Folders { get; set; } = [];

    public List<IDlgNode> Nodes { get; set; } = [];

    public class Serializer : MetaSerializer<Dlg>
    {
        private static readonly MetaClassSerializer<Dlg> s_metaClassSerializer = new();
        public override void Serialize(ref Dlg obj, MetaStream stream)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            MetaClass metaClass = stream.GetMetaClass(typeof(Dlg))!;

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException();
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                int folderCount = stream.ReadInt32();
                obj.Folders.Capacity = folderCount;

                for (var i = 0; i < folderCount; i++)
                {
                    var folder = new DlgFolder();
                    stream.Serialize(ref folder);
                    obj.Folders.Add(folder);
                }

                int nodeCount = stream.ReadInt32();
                obj.Nodes.Capacity = nodeCount;
                for (var i = 0; i < nodeCount; i++)
                {
                    MetaClassType? type = stream.ReadMetaClassType();

                    if (type == null)
                        throw new InvalidOperationException("[Dlg] Type is not registered.");

                    MetaSerializer metaSerializer = Toolkit.Instance.GetSerializer(type.LinkingType);

                    object node = null!;
                    metaSerializer.PreSerialize(ref node, stream);
                    metaSerializer.Serialize(ref node, stream);

                    if (node is IDlgNode dlgNode)
                    {
                        obj.Nodes.Add(dlgNode);
                    }
                }

                if (metaClass.ContainsMember("mbHasToolOnlyData"))
                {
                    // Read runtime boolean
                    _ = stream.ReadBoolean();
                }
            }
        }
    }
}

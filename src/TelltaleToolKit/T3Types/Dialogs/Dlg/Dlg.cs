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

    [MetaMember("mResourcePath")]
    public string ResourcePath { get; set; }

    public bool ToolFlag { get; set; }

    public List<DlgFolder> Folders { get; set; } = [];

    public List<IDlgNode> Nodes { get; set; } = [];

    public class Serializer : MetaSerializer<Dlg>
    {
        private static readonly MetaClassSerializer<Dlg> s_metaClassSerializer = new();

        public override void Serialize(ref Dlg obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            MetaClass metaClass = stream.GetMetaClass(typeof(Dlg))!;

            if (stream.Mode is MetaStreamMode.Write)
            {
                int folderCount = obj.Folders.Count;
                stream.Write(folderCount);

                foreach (var folder in obj.Folders)
                {
                    var folderRef = folder;
                    stream.Serialize(ref folderRef);
                }

                int nodeCount = obj.Nodes.Count;
                stream.Write(nodeCount);

                foreach (var node in obj.Nodes)
                {
                    var nodeType = stream.GetMetaClass(node.GetType())
                                   ?? throw new InvalidOperationException(
                                       $"[Dlg] Type '{node.GetType()}' is not registered in params or workspace.");

                    stream.Write(nodeType.ClassType);

                    MetaSerializer metaSerializer = Toolkit.Instance.GetSerializer(nodeType.ClassType.LinkingType);

                    object nodeObj = node;
                    metaSerializer.PreSerialize(ref nodeObj!, stream, nodeType.ClassType);
                    metaSerializer.Serialize(ref nodeObj, stream, nodeType.ClassType);
                }

                if (metaClass.ContainsMember("mbHasToolOnlyData"))
                {
                    stream.Write(obj.HasToolOnlyData);
                }
            }
            else
            {
                int folderCount = stream.ReadInt32();
                obj.Folders.Capacity = folderCount;

                for (int i = 0; i < folderCount; i++)
                {
                    var folder = new DlgFolder();
                    stream.Serialize(ref folder);
                    obj.Folders.Add(folder);
                }

                int nodeCount = stream.ReadInt32();
                obj.Nodes.Capacity = nodeCount;
                for (int i = 0; i < nodeCount; i++)
                {
                    MetaClassType? typeS = stream.ReadMetaClassType();

                    if (typeS == null)
                        throw new InvalidOperationException("[Dlg] Type is not registered.");

                    MetaSerializer metaSerializer = Toolkit.Instance.GetSerializer(typeS.LinkingType);

                    object node = null!;
                    metaSerializer.PreSerialize(ref node, stream, typeS);
                    metaSerializer.Serialize(ref node, stream, typeS);

                    if (node is IDlgNode dlgNode)
                    {
                        obj.Nodes.Add(dlgNode);
                    }
                }

                // TODO: Verify if this is really true
                if (metaClass.ContainsMember("mbHasToolOnlyData"))
                {
                    // Read runtime boolean
                    _ = stream.ReadBoolean();
                }
            }
        }
    }
}

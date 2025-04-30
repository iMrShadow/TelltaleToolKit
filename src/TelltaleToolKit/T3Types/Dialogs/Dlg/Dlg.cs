using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Properties;

namespace TelltaleToolKit.T3Types.Dialogs.Dlg;

[MetaClassSerializerGlobal(typeof(Serializer))]
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

    public class Serializer : MetaClassSerializer<Dlg>
    {
        private static readonly DefaultClassSerializer<Dlg> DefaultSerializer = new();
        public override void Serialize(ref Dlg obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            MetaClass metaClass = stream.GetMetaClass(typeof(Dlg))!;

            if (stream is MetaStreamWriter)
            {
                throw new NotImplementedException();
            }
            else if (stream is MetaStreamReader streamReader)
            {
                var folderCount = streamReader.ReadInt32();
                obj.Folders.Capacity = folderCount;

                for (var i = 0; i < folderCount; i++)
                {
                    var folder = new DlgFolder();
                    TTK.PreSerialize(ref folder, stream);
                    TTK.Serialize(ref folder, stream);
                    obj.Folders.Add(folder);
                }

                var nodeCount = streamReader.ReadInt32();
                obj.Nodes.Capacity = nodeCount;
                for (var i = 0; i < nodeCount; i++)
                {
                    MetaClassType type = streamReader.ReadMetaClassType();
                    MetaClassSerializer metaClassSerializer = TTKContext.Instance().GetSerializer(type.LinkingType);

                    object node = null!;
                    metaClassSerializer.PreSerialize(ref node, stream);
                    metaClassSerializer.Serialize(ref node, stream);

                    if (node is IDlgNode dlgNode)
                    {
                        obj.Nodes.Add(dlgNode);
                    }
                }

                if (metaClass.ContainsMember("mbHasToolOnlyData"))
                {
                    // Read runtime boolean
                    _ = streamReader.ReadBoolean();
                }
            }
        }
    }
}
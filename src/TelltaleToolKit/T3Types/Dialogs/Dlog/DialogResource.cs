using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaSerializer(typeof(Serializer))]
public class DialogResource
{
    [MetaMember("miNextDialogID")]
    public int NextDialogId { get; set; }

    [MetaMember("miNextBranchID")]
    public int NextBranchId { get; set; }

    [MetaMember("miNextItemID")]
    public int NextItemId { get; set; }

    [MetaMember("miNextExchangeID")]
    public int NextExchangeId { get; set; }

    [MetaMember("miNextLineID")]
    public int NextLineId { get; set; }

    [MetaMember("miNextTextID")]
    public int NextTextId { get; set; }

    [MetaMember("miNextChoreID")]
    public int NextChoreId { get; set; }

    [MetaMember("mDialogs")]
    public List<int> DialogIDs { get; set; } = [];

    [MetaMember("mSoloItems")]
    public List<int> SoloItemIDs { get; set; } = [];

    [MetaMember("mTexts")]
    public List<int> TextIDs { get; set; } = [];

    [MetaMember("mProjectID")]
    public int ProjectId { get; set; }

    [MetaMember("mTaskID")]
    public uint mTaskID { get; set; }


    [MetaMember("mResourcePath")]
    public string ResourcePath { get; set; } = string.Empty;

    public List<DialogDialog> Dialogs = [];
    public List<DialogBranch> Branches = [];
    public List<DialogItem> Items = [];
    public List<DialogExchange> Exchanges = [];
    public List<DialogLine> Lines = [];
    public List<DialogText> Texts = [];

    public class Serializer : MetaSerializer<DialogResource>
    {
        private static readonly MetaClassSerializer<DialogResource> s_metaClassSerializer = new();

        public override void PreSerialize(ref DialogResource? obj, MetaStream stream, MetaClassType? type = null)
        {
            obj ??= new DialogResource();
        }

        public override void Serialize(ref DialogResource obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj!, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Dialogs.Count);
                stream.Write(obj.Branches.Count);
                stream.Write(obj.Items.Count);
                stream.Write(obj.Exchanges.Count);
                stream.Write(obj.Lines.Count);
                stream.Write(obj.Texts.Count);

                SerializeDialogBaseArray(obj.Dialogs, obj.Dialogs.Count, stream);
                SerializeDialogBaseArray(obj.Branches, obj.Branches.Count, stream);
                SerializeDialogBaseArray(obj.Items, obj.Items.Count, stream);
                SerializeDialogBaseArray(obj.Exchanges, obj.Exchanges.Count, stream);
                SerializeDialogBaseArray(obj.Lines, obj.Lines.Count, stream);
                SerializeDialogBaseArray(obj.Texts, obj.Texts.Count, stream);
            }
            else
            {
                int dialogsCount = stream.ReadInt32();
                int branchesCount = stream.ReadInt32();
                int itemsCount = stream.ReadInt32();
                int exchangesCount = stream.ReadInt32();
                int linesCount = stream.ReadInt32();
                int textCount = stream.ReadInt32();

                SerializeDialogBaseArray(obj.Dialogs, dialogsCount, stream);
                SerializeDialogBaseArray(obj.Branches, branchesCount, stream);
                SerializeDialogBaseArray(obj.Items, itemsCount, stream);
                SerializeDialogBaseArray(obj.Exchanges, exchangesCount, stream);
                SerializeDialogBaseArray(obj.Lines, linesCount, stream);
                SerializeDialogBaseArray(obj.Texts, textCount, stream);
            }
        }

        private static void SerializeDialogBaseArray<T>(List<T> list, int count, MetaStream stream)
            where T : IDialogBase, new()
        {
            if (stream.Mode is MetaStreamMode.Write)
            {
                foreach (var item in list)
                {
                    stream.Write(item.DialogBase.ActualId);
                }

                foreach (var item in list)
                {
                    T dialogBase = item;
                    stream.Serialize(ref dialogBase);
                }
            }
            else
            {
                if (count <= 0) return;

                int[] ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = stream.ReadInt32();
                }

                for (int i = 0; i < count; i++)
                {
                    T dialog = new();
                    stream.Serialize(ref dialog);
                    dialog.DialogBase.ActualId = ids[i];
                    list.Add(dialog);
                }
            }
        }
    }
}

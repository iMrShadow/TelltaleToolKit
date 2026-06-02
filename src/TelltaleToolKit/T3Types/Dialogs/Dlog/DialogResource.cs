using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Dialogs;

[MetaClassSerializerGlobal(typeof(Serializer))]
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

    [MetaMember("mResourcePath")]
    public string ResourcePath { get; set; } = string.Empty;

    public List<DialogDialog> Dialogs = [];
    public List<DialogBranch> Branches = [];
    public List<DialogItem> Items = [];
    public List<DialogExchange> Exchanges = [];
    public List<DialogLine> Lines = [];
    public List<DialogText> Texts = [];

    public class Serializer : MetaClassSerializer<DialogResource>
    {
        private static readonly DefaultClassSerializer<DialogResource> DefaultSerializer = new();

        public override void Serialize(ref DialogResource obj, MetaStream stream)
        {
            DefaultSerializer.PreSerialize(ref obj, stream);
            DefaultSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                throw new NotImplementedException($"Serializer is not implement for {SerializationType}");
            }

            if (stream.Mode is MetaStreamMode.Read)
            {
                // Console.WriteLine("Current position: " + stream.GetPosition());
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
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                // Console.WriteLine(count);
                if (count <= 0)
                {
                    return;
                }

                var ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = stream.ReadInt32();
                }

                var dialog = default(T);

                for (var i = 0; i < count; i++)
                {
                    Toolkit.Instance.GetSerializer<T>().Serialize(ref dialog, stream);
                    list.Add(dialog);
                    dialog.DialogBase.ActualId = ids[i];
                }
            }
        }
    }
}

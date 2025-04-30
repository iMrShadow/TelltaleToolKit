using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.Serialization.Serializers;

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

            if (stream is MetaStreamWriter streamWriter)
            {
                throw new NotImplementedException($"Serializer is not implement for {SerializationType}");
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // Console.WriteLine("Current position: " + stream.GetCurrentPosition());
                int dialogsCount = streamReader.ReadInt32();
                int branchesCount = streamReader.ReadInt32();
                int itemsCount = streamReader.ReadInt32();
                int exchangesCount = streamReader.ReadInt32();
                int linesCount = streamReader.ReadInt32();
                int textCount = streamReader.ReadInt32();

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
            if (stream is MetaStreamWriter streamWriter)
            {
            }
            else if (stream is MetaStreamReader streamReader)
            {
                // Console.WriteLine(count);
                if (count <= 0)
                {
                    return;
                }

                var ids = new int[count];
                for (int i = 0; i < count; i++)
                {
                    ids[i] = streamReader.ReadInt32();
                }

                var dialog = default(T);

                for (var i = 0; i < count; i++)
                {
                    TTKContext.Instance().GetSerializer<T>().Serialize(ref dialog, stream);
                    list.Add(dialog);
                    dialog.DialogBase.ActualId = ids[i];
                }
            }
        }
    }
}
using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Dialogs.Dlg;

namespace TelltaleToolKit.T3Types.Miscellaneous.PreloadPackage;

[MetaSerializer(typeof(MetaClassSerializer<RuntimeDataDialog>))]
public class RuntimeDataDialog
{
    [MetaMember("mDialogResourceVectors")]
    public List<DlgObjIdAndResourceVector> DialogResourceVectors { get; set; } = [];

    [MetaMember("mStartNodeOffsets")]
    public List<DlgObjIdAndStartNodeOffset> StartNodeOffsets { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<DialogResourceInfo>))]
    public class DialogResourceInfo
    {
        [MetaMember("mResourceSeenTimes")]
        public ResourceSeenTimes ResourceSeenTimes { get; set; } = new();

        [MetaMember("mResourceKey")]
        public ResourceKey ResourceKey { get; set; } = new();
    }

    [MetaSerializer(typeof(MetaClassSerializer<DlgObjIdAndResourceVector>))]
    public class DlgObjIdAndResourceVector
    {
        [MetaMember("mVector")]
        public List<DialogResourceInfo> Vector { get; set; } = [];

        [MetaMember("mID")]
        public DlgObjId ID { get; set; } = new();
    }

    [MetaSerializer(typeof(MetaClassSerializer<DlgObjIdAndStartNodeOffset>))]
    public class DlgObjIdAndStartNodeOffset
    {
        [MetaMember("mOffset")]
        public StartNodeOffset Offset { get; set; } = new();

        [MetaMember("mID")]
        public DlgObjId ID { get; set; }
    }
}

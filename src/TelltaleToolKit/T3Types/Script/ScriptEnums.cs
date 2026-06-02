using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Script;

[MetaSerializer(typeof(MetaClassSerializer<ScriptEnum>))]
public class ScriptEnum
{
    [MetaMember("mCurValue")]
    public string CurValue { get; set; } = string.Empty;

    public override string ToString() => CurValue;
}

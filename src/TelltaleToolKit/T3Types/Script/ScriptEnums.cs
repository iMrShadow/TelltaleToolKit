using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Script;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<ScriptEnum>))]
public class ScriptEnum
{
    [MetaMember("mCurValue")]
    public string CurValue { get; set; } = string.Empty;

    public override string ToString() => CurValue;
}
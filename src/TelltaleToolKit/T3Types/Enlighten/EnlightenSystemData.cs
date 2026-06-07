using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Enlighten;

[MetaSerializer(typeof(MetaClassSerializer<EnlightenSystemData>))]
public class EnlightenSystemData
{
    [MetaMember( "mName")]
    public Symbol Name { get; set; } = Symbol.Empty;
    [MetaMember( "mEnvTileName")]
    public Symbol EnvTileName { get; set; } = Symbol.Empty;
    [MetaMember( "mRadSystemCore")]
    public BinaryBuffer RadSystemCore { get; set; } = new();
    [MetaMember( "mInputWorkspace")]
    public BinaryBuffer InputWorkspace { get; set; } = new();
    [MetaMember( "mClusterAlbedoWorkspaceMaterial")]
    public BinaryBuffer ClusterAlbedoWorkspaceMaterial { get; set; } = new();
    [MetaMember( "mPrecomputedVisibility")]
    public BinaryBuffer PrecomputedVisibility { get; set; } = new();
}

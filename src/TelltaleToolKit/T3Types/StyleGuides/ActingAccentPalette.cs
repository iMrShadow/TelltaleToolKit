using TelltaleToolKit.Meta.Reflection;
using TelltaleToolKit.Meta.Serialization;
using TelltaleToolKit.Meta.Serialization.Serializers;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Mathematics;

namespace TelltaleToolKit.T3Types.StyleGuides;

[MetaSerializer(typeof(Serializer))]
public class ActingAccentPalette
{
    [MetaSerializer(typeof(EnumSerializer<EnumTrackID>))]
    public enum EnumTrackID
    {
        body = 0x1,
        face = 0x2,
        head1 = 0x3,
        head2 = 0x4
    }

    [MetaSerializer(typeof(EnumSerializer<Overrun>))]
    public enum Overrun
    {
        Disallowed = 0,
        Allowed = 1
    }

    [MetaMember("Baseclass_ActingOverridablePropOwner")]
    public ActingOverridablePropOwner BaseClassActingOverridablePropOwner { get; set; }

    [MetaMember("Baseclass_UID::Owner")]
    public Owner BaseClassOwner { get; set; }

    [MetaMember("mName")]
    public string Name { get; set; } = string.Empty;

    [MetaMember("mStartOffsetRange")]
    public Range<float> StartOffsetRange { get; set; } = new();

    [MetaMember("mMoodOverrunAllowed")]
    public EnumOverrun MoodOverrunAllowed { get; set; }

    [MetaMember("mDisableAct")]
    public bool DisableAct { get; set; }

    [MetaMember("mValidIntensityRange")]
    public Range<float> ValidIntensityRange { get; set; } = new();

    [MetaMember("mSpilloutBufPostRange")]
    public Range<float> SpilloutBufPostRange { get; set; } = new();

    [MetaMember("mRandomChance")]
    public float RandomChance { get; set; }

    [MetaMember("mTrackID")]
    public EnumTrackID TrackID { get; set; }

    [MetaMember("mGroupMembershipUID")]
    public int GroupMembershipUID { get; set; }

    [MetaMember("mFlags")]
    public Flags Flags { get; set; }

    [MetaMember("mVersion")]
    public int Version { get; set; }

    public List<ActingResource> Resources { get; set; } = [];

    [MetaSerializer(typeof(MetaClassSerializer<EnumOverrun>))]
    public struct EnumOverrun
    {
        [MetaMember("mVal")]
        public Overrun Val { get; set; }
    }

    public class Serializer : MetaSerializer<ActingAccentPalette>
    {
        private static readonly MetaClassSerializer<ActingAccentPalette> s_metaClassSerializer = new();

        public override void Serialize(ref ActingAccentPalette obj, MetaStream stream, MetaClassType? type = null)
        {
            s_metaClassSerializer.PreSerialize(ref obj, stream);
            s_metaClassSerializer.Serialize(ref obj, stream);

            if (stream.Mode is MetaStreamMode.Write)
            {
                stream.Write(obj.Resources.Count);

                foreach (ActingResource resource in obj.Resources)
                {
                    ActingResource actingResource = resource;
                    stream.Serialize(ref actingResource);
                }
            }
            else if (stream.Mode is MetaStreamMode.Read)
            {
                int numResources = stream.ReadInt32();
                obj.Resources.Clear();
                for (int i = 0; i < numResources; i++)
                {
                    MetaClass? mcd = stream.GetMetaClass(typeof(ActingResource));
                    ActingResource resource = new();
                    stream.Serialize(ref resource);
                    obj.Resources.Add(resource);
                }
            }
        }
    }

    public class PaletteClassStatus
    {
    }
}

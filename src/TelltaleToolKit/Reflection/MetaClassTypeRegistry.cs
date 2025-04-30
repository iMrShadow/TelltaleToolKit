using System.Runtime.CompilerServices;
using TelltaleToolKit.T3Types;
using TelltaleToolKit.T3Types.ActorMaps;
using TelltaleToolKit.T3Types.Animations;
using TelltaleToolKit.T3Types.Audio;
using TelltaleToolKit.T3Types.Chores;
using TelltaleToolKit.T3Types.Common;
using TelltaleToolKit.T3Types.Common.UID;
using TelltaleToolKit.T3Types.Dialogs;
using TelltaleToolKit.T3Types.Dialogs.Dlg;
using TelltaleToolKit.T3Types.Dialogs.Dlg.Nodes;
using TelltaleToolKit.T3Types.Dialogs.DlgSettings;
using TelltaleToolKit.T3Types.Fonts;
using TelltaleToolKit.T3Types.InputMaps;
using TelltaleToolKit.T3Types.Languages.Landb;
using TelltaleToolKit.T3Types.Languages.Langdb;
using TelltaleToolKit.T3Types.Languages.Llm;
using TelltaleToolKit.T3Types.Languages.Locreg;
using TelltaleToolKit.T3Types.LightMaps;
using TelltaleToolKit.T3Types.Mathematics;
using TelltaleToolKit.T3Types.Meshes;
using TelltaleToolKit.T3Types.Meshes.T3Types;
using TelltaleToolKit.T3Types.Miscellaneous;
using TelltaleToolKit.T3Types.NavCam;
using TelltaleToolKit.T3Types.Overlays;
using TelltaleToolKit.T3Types.Properties;
using TelltaleToolKit.T3Types.Rules;
using TelltaleToolKit.T3Types.Scenes;
using TelltaleToolKit.T3Types.Script;
using TelltaleToolKit.T3Types.Skeletons;
using TelltaleToolKit.T3Types.StyleGuides;
using TelltaleToolKit.T3Types.Text;
using TelltaleToolKit.T3Types.Textures;
using TelltaleToolKit.T3Types.Textures.T3Types;
using TelltaleToolKit.T3Types.Voice;
using TelltaleToolKit.T3Types.WalkBoxes;

namespace TelltaleToolKit.Reflection;

public static class MetaClassTypeRegistry
{
    // Lookup by type, name, or CRC hash
    private static readonly Dictionary<string, MetaClassType> ByName = new(StringComparer.Ordinal);
    private static readonly Dictionary<ulong, MetaClassType> ByHash = new();

    /// <summary>
    /// All encountered types in Telltale Tool in their original nameof form.
    /// Most types were provided by Lucas's VersDB (Many special thanks), David M and Proton.
    /// </summary>
    static MetaClassTypeRegistry()
    {
	    // @formatter:off
        // Types which are linked to int are NOT supported.
        Register("ActingAccentPalette::EnumOverrun",typeof(NotImplementedException)); // Telltale...you have outdone yourself. EnumDescriptionMemories. For some reason this type's owner is also ActingAccentPalette. No idea why.
        Register("ActingCommandSequence",typeof(NotImplementedException));
        Register("ActingCommandSequence::Context",typeof(NotImplementedException));
        Register("ActingPalette::EnumEndOffsetRelativeTo",typeof(NotImplementedException));
        Register("ActingPalette::EnumEndRelativeTo",typeof(NotImplementedException));
        Register("ActingPalette::EnumOverrun",typeof(NotImplementedException));
        Register("ActingPalette::PaletteFlags",typeof(NotImplementedException));
        Register("ActingPaletteGroup::ActingPaletteTransition", typeof(ActingPaletteGroup.ActingPaletteTransition));
        Register("ActingResourceOwner",typeof(NotImplementedException));
        Register("AnimationDrivenPathSegment", typeof(NotImplementedException));
        Register("AnimationMixer<AnimOrChore>", typeof(NotImplementedException));
        Register("AnimationMixer<Color>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<ActorAgentMapper>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<AgentMap>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<AnimOrChore>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Animation>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<AudioData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<BlendGraph>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<BlendGraphManager>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<BlendMode>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Chore>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<D3DMesh>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<DialogResource>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Dlg>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<EventStorage>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Font>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<InputMapper>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<LanguageDatabase>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<LanguageResource>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<LightProbeData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<LocomotionDB>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<ParticleProperties>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<ParticleSprite>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<PhonemeTable>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<PhysicsData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<PhysicsObject>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<PreloadPackage::RuntimeDataDialog>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<PreloadPackage::RuntimeDataScene>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<PropertySet>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Rule>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Rules>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SaveGame>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Scene>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<Skeleton>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundAmbience::AmbienceDefinition>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundBusSnapshot::Snapshot>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundBusSnapshot::SnapshotSuite>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundEventBankDummy>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundEventData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundEventSnapshotData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<SoundReverbDefinition>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<StyleGuide>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<StyleGuideRef>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<StyleIdleTransitionsRes>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<T3OverlayData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<T3Texture>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<TransitionMap>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<VoiceData>>", typeof(NotImplementedException));
        Register("AnimationMixer<class Handle<WalkBoxes>>", typeof(NotImplementedException));
        Register("AnimationMixer<PhonemeKey>", typeof(NotImplementedException));
        Register("AnimationMixer<Polar>", typeof(NotImplementedException));
        Register("AnimationMixer<Quaternion>", typeof(NotImplementedException));
        Register("AnimationMixer<ScriptEnum>", typeof(NotImplementedException));
        Register("AnimationMixer<SkeletonPose>", typeof(NotImplementedException));
        Register("AnimationMixer<SoundEventName<0>>", typeof(NotImplementedException));
        Register("AnimationMixer<SoundEventName<1>>", typeof(NotImplementedException));
        Register("AnimationMixer<SoundEventName<2>>", typeof(NotImplementedException));
        Register("AnimationMixer<String>", typeof(NotImplementedException));
        Register("AnimationMixer<Symbol>", typeof(NotImplementedException));
        Register("AnimationMixer<T3NormalSampleData,T3HeapAllocator>", typeof(NotImplementedException));
        Register("AnimationMixer<T3PositionSampleData,T3HeapAllocator>", typeof(NotImplementedException));
        Register("AnimationMixer<Transform>", typeof(NotImplementedException));
        Register("AnimationMixer<Vector2>", typeof(NotImplementedException));
        Register("AnimationMixer<Vector3>", typeof(NotImplementedException));
        Register("AnimationMixer<Vector4>", typeof(NotImplementedException));
        Register("AnimationMixer<bool>", typeof(NotImplementedException));
        Register("AnimationMixer<float>", typeof(NotImplementedException));
        Register("AnimationMixer<int>", typeof(NotImplementedException));
        Register("BitSet<T3EffectParameterType,59>", typeof(BitSet<T3EffectParameterType>));
        Register("BitSet<enum T3MaterialChannelType,46,0>",typeof(BitSet<T3MaterialChannelType>));
        Register("ChoreAgentInst", typeof(NotImplementedException));
        Register("ChoreInst*", typeof(ChoreInst));
        Register("CompressedSkeletonPoseContext", typeof(NotImplementedException));
        Register("D3DMesh::LocalTransformEntry", typeof(NotImplementedException));
        Register("D3DMesh::Texture", typeof(D3DMesh.Texture));
        Register("DArray<InputMapper*>", typeof(List<InputMapper>));
        Register("DArray<unsignedint>", typeof(List<uint>));
        Register("DCArray<D3DMesh::AnimatedVertexEntry>", typeof(NotImplementedException));
        Register("DCArray<D3DMesh::BoneEntry>", typeof(List<D3DMesh.BoneEntry>));
        Register("DCArray<D3DMesh::LocalTransformEntry>", typeof(NotImplementedException));
        Register("DCArray<D3DMesh::SkinningEntry>", typeof(List<D3DMesh.SkinningEntry>));
        Register("DCArray<D3DMesh::Texture>", typeof(List<D3DMesh.Texture>));
        Register("DCArray<D3DMesh::VertexAnimation>", typeof(List<D3DMesh.VertexAnimation>));
        Register("DCArray<DCArray<D3DMesh::LocalTransformEntry>>", typeof(NotImplementedException));
        Register("DCArray<DCArray<T3MeshBonePaletteEntry>>", typeof(NotImplementedException));
        Register("DCArray<DCArray<T3MeshLocalTransformEntry>>", typeof(NotImplementedException));
        Register("DCArray<DlgStructs::DlgObjIDAndDlg>", typeof(NotImplementedException));
        Register("DCArray<LightProbeData::ProbeSH>", typeof(NotImplementedException));
        Register("DCArray<LightProbeData::Tetrahedra>", typeof(NotImplementedException));
        Register("DCArray<RenderObject_Mesh::TriangleSetInstance>", typeof(NotImplementedException));
        Register("DCArray<RenderObject_Mesh::VertexAnimationInstance>", typeof(List<RenderObject_Mesh.VertexAnimationInstance>));
        Register("DCArray<T3EffectBinaryDataCg::ParameterLocation>", typeof(List<T3EffectBinaryDataCg.ParameterLocation>));
        Register("DCArray<T3EffectBinaryDataCg::ParameterOffsets>", typeof(List<T3EffectBinaryDataCg.ParameterOffsets>));
        Register("DCArray<T3EffectBinaryDataCg::Pass>", typeof(List<T3EffectBinaryDataCg.Pass>));
        Register("DCArray<T3EffectBinaryDataCg::SamplerState>", typeof(List<T3EffectBinaryDataCg.SamplerState>));
        Register("DCArray<T3EffectBinaryDataCg::VertexStreamIndex>", typeof(List<T3EffectBinaryDataCg.VertexStreamIndex>));
        Register("DCArray<T3EffectPreloadPackage::EffectEntry>", typeof(NotImplementedException));
        Register("DCArray<T3EffectPreloadPackage::VarianceEntry>", typeof(NotImplementedException));
        Register("DCArray<T3MeshBonePaletteEntry>", typeof(NotImplementedException));
        Register("DglChoiceInstance",typeof(NotImplementedException));
        Register("DialogBaseInstance<DialogBranch>",typeof(NotImplementedException));
        Register("DialogBaseInstance<DialogDialog>",typeof(NotImplementedException));
        Register("DialogBaseInstance<DialogItem>",typeof(NotImplementedException));
        Register("EnlightenModule::EnlightenSettings", typeof(NotImplementedException));
        Register("EnlightenModule::EnlightenSettings::Quality", typeof(NotImplementedException));
        Register("Handle<T3EffectBinary>", typeof(NotImplementedException));
        Register("Handle<T3EffectPreloadPackage>", typeof(NotImplementedException));
        Register("IntrusiveSet<Symbol,PropertySet::KeyInfo,TagPropertyKeyInfoSet,less<Symbol>>", typeof(NotImplementedException));
        Register("KeyframedValue<CompressedPathBlockingValue::CompressedPathInfoKey>::Sample",typeof(NotImplementedException));
        Register("KeyframedValue<class Handle<Chore>>::Sample",typeof(KeyframedValue<Handle<Chore>>.Sample));
        Register("KeyframedValue<class Handle<D3DMesh>>::Sample",typeof(KeyframedValue<Handle<D3DMesh>>.Sample));
        Register("KeyframedValue<class Handle<Dlg>>::Sample",typeof(KeyframedValue<Handle<Dlg>>.Sample));
        Register("KeyframedValue<class Handle<Font>>::Sample",typeof(KeyframedValue<Handle<Font>>.Sample));
        Register("KeyframedValue<class Handle<PhonemeTable>>::Sample",typeof(KeyframedValue<Handle<PhonemeTable>>.Sample));
        Register("KeyframedValue<class Handle<PropertySet>>::Sample",typeof(KeyframedValue<Handle<PropertySet>>.Sample));
        Register("KeyframedValue<class Handle<Scene>>::Sample",typeof(KeyframedValue<Handle<Scene>>.Sample));
        Register("KeyframedValue<class Handle<SoundAmbience::AmbienceDefinition>>::Sample",typeof(NotImplementedException));
        Register("KeyframedValue<class Handle<SoundBusSnapshot::Snapshot>>::Sample",typeof(NotImplementedException));
        Register("KeyframedValue<class Handle<SoundBusSnapshot::SnapshotSuite>>::Sample",typeof(NotImplementedException));
        Register("KeyframedValue<class Handle<SoundData>>::Sample",typeof(KeyframedValue<Handle<SoundData>>.Sample));
        Register("KeyframedValue<class Handle<SoundEventData>>::Sample",typeof(KeyframedValue<Handle<SoundEventData>>.Sample));
        Register("KeyframedValue<class Handle<SoundEventSnapshotData>>::Sample",typeof(KeyframedValue<Handle<SoundEventSnapshotData>>.Sample));
        Register("KeyframedValue<class Handle<SoundReverbDefinition>>::Sample",typeof(KeyframedValue<Handle<SoundReverbDefinition>>.Sample));
        Register("KeyframedValue<class Handle<T3Texture>>::Sample",typeof(KeyframedValue<Handle<T3Texture>>.Sample));
        Register("KeyframedValue<class Handle<WalkBoxes>>::Sample",typeof(KeyframedValue<Handle<WalkBoxes>>.Sample));
        Register("KeyframedValue<LocationInfo>::Sample",typeof(KeyframedValue<LocationInfo>.Sample));
        Register("KeyframedValue<Polar>::Sample",typeof(KeyframedValue<Polar>.Sample));
        Register("KeyframedValue<ScriptEnum>::Sample",typeof(KeyframedValue<ScriptEnum>.Sample));
        Register("KeyframedValue<SoundEventName<0>>::Sample",typeof(KeyframedValue<SoundEventName>.Sample));
        Register("KeyframedValue<SoundEventName<1>>::Sample",typeof(KeyframedValue<SoundEventName>.Sample));
        Register("KeyframedValue<SoundEventName<2>>::Sample",typeof(KeyframedValue<SoundEventName>.Sample));
        Register("KeyframedValue<Symbol>::Sample",typeof(KeyframedValue<Symbol>.Sample));
        Register("KeyframedValue<T3VertexBufferSample<T3NormalSampleData,T3HeapAllocator>>::Sample",typeof(NotImplementedException));
        Register("KeyframedValue<T3VertexBufferSample<T3PositionSampleData,T3HeapAllocator>>::Sample",typeof(NotImplementedException));
        Register("KeyframedValue<Vector2>::Sample",typeof(KeyframedValue<Vector2>.Sample));
        Register("KeyframedValue<Vector4>::Sample",typeof(KeyframedValue<Vector4>.Sample));
        Register("LanguageLookupMap::DlgIDSet", typeof(NotImplementedException));
        Register("LanguageRegister",typeof(NotImplementedException));
        Register("Map<String,Rule*,less<String>>", typeof(NotImplementedException));
        Register("Map<Symbol,D3DMesh::AnimatedVertexGroupEntry,less<Symbol>>", typeof(NotImplementedException));
        Register("Map<float,KeyframedValue<int>,less<float>>", typeof(NotImplementedException));
        Register("Map<int,DCArray<unsignedint>,less<int>>", typeof(NotImplementedException));
        Register("Map<int,unsignedint,less<int>>", typeof(NotImplementedException));
        Register("MergeInGuideInfo",typeof(NotImplementedException));
        Register("MergeInMoodInfo",typeof(NotImplementedException));
        Register("Note",typeof(NotImplementedException));
        Register("Note::Entry",typeof(NotImplementedException));
        Register("NoteCategory",typeof(NotImplementedException));
        Register("NoteCollection",typeof(NotImplementedException));
        Register("Physics::State", typeof(NotImplementedException));
        Register("Procedural_AnimatedLookAt_Value",typeof(NotImplementedException));
        Register("Procedural_Eyes_Value",typeof(NotImplementedException));
        Register("RenderObject_Decal", typeof(NotImplementedException));
        Register("RenderObject_HLSMovie", typeof(NotImplementedException));
        Register("RenderObject_Mesh::MeshInstance", typeof(RenderObject_Mesh.MeshInstance));
        Register("RenderObject_Viewport", typeof(NotImplementedException));
        Register("SArray<DCArray<D3DMesh::Texture>,14>", typeof(NotImplementedException));
        Register("SArray<DCArray<RenderObject_Mesh::TextureInstance>,14>", typeof(NotImplementedException));
        Register("SArray<T3VertexComponent,13>", typeof(NotImplementedException));
        Register("SArray<int,14>", typeof(NotImplementedException));
        Register("SArray<unsignedint,2>", typeof(uint[]));
        Register("ScriptEnum:AIAgentState", typeof(ScriptEnum));
        Register("ScriptEnum:AIDummyPos", typeof(ScriptEnum));
        Register("ScriptEnum:AIPatrolType", typeof(ScriptEnum));
        Register("ScriptEnum:BlendTypes", typeof(ScriptEnum));
        Register("ScriptEnum:Case State", typeof(ScriptEnum)); // This hasn't been serialized, but it's defined in csi3system.lua
        Register("ScriptEnum:ChaseForwardVector", typeof(ScriptEnum));
        Register("ScriptEnum:Chore", typeof(ScriptEnum));
        Register("ScriptEnum:ControllerButtons", typeof(ScriptEnum));
        Register("ScriptEnum:Cursors", typeof(ScriptEnum));
        Register("ScriptEnum:DialogMode", typeof(ScriptEnum));
        Register("ScriptEnum:Difficulty level", typeof(ScriptEnum));
        Register("ScriptEnum:DraxFollowing", typeof(ScriptEnum));
        Register("ScriptEnum:Evidence Comparison", typeof(ScriptEnum));
        Register("ScriptEnum:Evidence State", typeof(ScriptEnum));
        Register("ScriptEnum:Evidence", typeof(ScriptEnum));
        Register("ScriptEnum:EvidenceUpdate", typeof(ScriptEnum));
        Register("ScriptEnum:FactLog", typeof(ScriptEnum));
        Register("ScriptEnum:Fingerprint Database Type", typeof(ScriptEnum));
        Register("ScriptEnum:Flyover Movie State", typeof(ScriptEnum));
        Register("ScriptEnum:GamepadButton", typeof(ScriptEnum));
        Register("ScriptEnum:Gender", typeof(ScriptEnum));
        Register("ScriptEnum:Goal", typeof(ScriptEnum));
        Register("ScriptEnum:GrootFlowers", typeof(ScriptEnum));
        Register("ScriptEnum:Hints", typeof(ScriptEnum));
        Register("ScriptEnum:Insect", typeof(ScriptEnum));
        Register("ScriptEnum:KamariaAdvice", typeof(ScriptEnum));
        Register("ScriptEnum:Lab Equipment Type", typeof(ScriptEnum));
        Register("ScriptEnum:LastVisited", typeof(ScriptEnum));
        Register("ScriptEnum:LightComposerCameraZone", typeof(ScriptEnum));
        Register("ScriptEnum:LightComposerLightSourceQuadrant", typeof(ScriptEnum));
        Register("ScriptEnum:LightComposerNodeLocation", typeof(ScriptEnum));
        Register("ScriptEnum:Location", typeof(ScriptEnum));
        Register("ScriptEnum:MenuAlignment", typeof(ScriptEnum));
        Register("ScriptEnum:MenuVerticalAlignment", typeof(ScriptEnum));
        Register("ScriptEnum:Platform", typeof(ScriptEnum));
        Register("ScriptEnum:PropPresetDepthofFieldSAMPLE", typeof(ScriptEnum));
        Register("ScriptEnum:PropPresetLensKits", typeof(ScriptEnum));
        Register("ScriptEnum:QTE_Type", typeof(ScriptEnum));
        Register("ScriptEnum:Reconstructions", typeof(ScriptEnum));
        Register("ScriptEnum:ReticleActions", typeof(ScriptEnum));
        Register("ScriptEnum:ReticleDisplayMode", typeof(ScriptEnum));
        Register("ScriptEnum:RoomFrom", typeof(ScriptEnum));
        Register("ScriptEnum:RoomTo", typeof(ScriptEnum));
        Register("ScriptEnum:Script Function", typeof(ScriptEnum));
        Register("ScriptEnum:StruggleType", typeof(ScriptEnum));
        Register("ScriptEnum:Suspect", typeof(ScriptEnum));
        Register("ScriptEnum:Thoroughness", typeof(ScriptEnum));
        Register("ScriptEnum:Tool Type", typeof(ScriptEnum));
        Register("ScriptEnum:Topics", typeof(ScriptEnum));
        Register("ScriptEnum:Trinity Evidence", typeof(ScriptEnum));
        Register("ScriptEnum:Trinity Suspect", typeof(ScriptEnum));
        Register("ScriptEnum:UIColor", typeof(ScriptEnum));
        Register("ScriptEnum:UseableType", typeof(ScriptEnum));
        Register("ScriptEnum:Victim", typeof(ScriptEnum));
        Register("ScriptEnum:View", typeof(ScriptEnum));
        Register("ScriptEnum:Warrant", typeof(ScriptEnum));
        Register("ScriptEnum:Warrent", typeof(ScriptEnum));
        Register("ScriptEnum:WhatTellGamoraPostFlashback", typeof(ScriptEnum));
        Register("ScriptEnum:WhatTellNebulaThroneRoomGamora", typeof(ScriptEnum));
        Register("ScriptEnum:WhatTellNebulaTrainingRoomGamora", typeof(ScriptEnum));
        Register("ScriptEnum:WhatTellThanosTrainingRoomGamora", typeof(ScriptEnum));
        Register("ScriptEnum:WhatTellThanosTrainingRoomNebula", typeof(ScriptEnum));
        Register("ScriptEnum:WormName", typeof(ScriptEnum));
        Register("Set<unsignedint,less<unsignedint>>", typeof(NotImplementedException));
        Register("SingleContributionValue<float>",typeof(NotImplementedException));
        Register("SkeletonPoseCompoundValue",typeof(NotImplementedException));
        Register("SoundAmbience::EventContext", typeof(NotImplementedException));
        Register("Style::StyleIdleManager::FadeData", typeof(NotImplementedException));
        Register("T3EffectBinary", typeof(NotImplementedException));
        Register("class T3EffectParameters", typeof(NotImplementedException));
        Register("T3EffectBinaryData", typeof(T3EffectBinaryData));
        Register("T3EffectBinaryDataCg", typeof(T3EffectBinaryDataCg));
        Register("T3EffectBinaryDataCg::ParameterLocation", typeof(T3EffectBinaryDataCg.ParameterLocation));
        Register("T3EffectBinaryDataCg::Pass", typeof(T3EffectBinaryDataCg.Pass));
        Register("T3EffectBinaryDataCg::SamplerState", typeof(T3EffectBinaryDataCg.SamplerState));
        Register("class T3EffectBinaryDataCg::Technique", typeof(T3EffectBinaryDataCg.Technique));
        Register("T3EffectBinaryDataHlsl_D3D", typeof(T3EffectBinaryDataHlsl_D3D));
        Register("T3EffectPreloadPackage", typeof(NotImplementedException));
        Register("T3MaterialBrushNormalImportParams", typeof(NotImplementedException));
        Register("T3MaterialTextureImport", typeof(NotImplementedException));
        Register("T3MeshBonePaletteEntry", typeof(NotImplementedException));
        Register("T3MeshBuffer", typeof(NotImplementedException));
        Register("T3MeshCPUSkinningData", typeof(NotImplementedException));
        Register("T3MeshTexture", typeof(NotImplementedException));
        Register("T3MeshVertexState", typeof(NotImplementedException));
        Register("T3OverlayObjectData_Text", typeof(T3OverlayObjectDataText));
        Register("T3OverlayTextParams", typeof(T3OverlayTextParams));
        Register("T3VertexComponent", typeof(T3VertexComponent));
        Register("T3VertexDeclaration", typeof(NotImplementedException));
        Register("T3VertexSampleDataBase",typeof(NotImplementedException));
        Register("TRange<unsignedint>", typeof(NotImplementedException));
        Register("class TRange<unsigned long>", typeof(NotImplementedException));
        Register("TextBuffer::Line", typeof(NotImplementedException));
        Register("__int64", typeof(long),MetaFlags.MetaSerializeBlockingDisabled);
        Register("bool", typeof(bool), MetaFlags.MetaSerializeBlockingDisabled);
        Register("char", typeof(sbyte),MetaFlags.MetaSerializeBlockingDisabled);
        Register("class ActingAccentPalette", typeof(NotImplementedException));
        Register("class ActingOverridablePropOwner", typeof(ActingOverridablePropOwner));
        Register("class ActingPalette", typeof(ActingPalette));
        Register("class ActingPaletteClass", typeof(ActingPaletteClass));
        Register("class ActingPaletteGroup", typeof(ActingPaletteGroup));
        Register("class ActingResource", typeof(ActingResource));
        Register("class ActorAgentBinding", typeof(ActorAgentBinding));
        Register("class ActorAgentMapper", typeof(ActorAgentMapper));
        Register("class Agent", typeof(Agent));
        Register("class AgentMap", typeof(AgentMap));
        Register("class AgentMap::AgentMapEntry", typeof(AgentMap.AgentMapEntry));
        Register("class AgentState", typeof(AgentState));
        Register("class AnimOrChore", typeof(AnimOrChore));
        Register("class AnimatedValueInterface<bool>", typeof(AnimatedValueInterface<bool>));
        Register("class AnimatedValueInterface<class AnimOrChore>", typeof(AnimatedValueInterface<AnimOrChore>));
        Register("class AnimatedValueInterface<class Color>", typeof(AnimatedValueInterface<Color>));
        Register("class AnimatedValueInterface<class Handle<class Chore> >", typeof(AnimatedValueInterface<Handle<Chore>>));
        Register("class AnimatedValueInterface<class Handle<class D3DMesh> >", typeof(AnimatedValueInterface<Handle<D3DMesh>>));
        Register("class AnimatedValueInterface<class Handle<class Dlg> >", typeof(AnimatedValueInterface<Handle<Dlg>>));
        Register("class AnimatedValueInterface<class Handle<class Font> >", typeof(AnimatedValueInterface<Handle<Font>>));
        Register("class AnimatedValueInterface<class Handle<class PhonemeTable> >", typeof(AnimatedValueInterface<Handle<PhonemeTable>>));
        Register("class AnimatedValueInterface<class Handle<class PropertySet> >", typeof(AnimatedValueInterface<Handle<PropertySet>>));
        Register("class AnimatedValueInterface<class Handle<class Scene> >", typeof(AnimatedValueInterface<Handle<Scene>>));
        Register("class AnimatedValueInterface<class Handle<class SoundBusSnapshot::Snapshot> >", typeof(AnimatedValueInterface<Handle<SoundBusSnapshot.Snapshot>>));
        Register("class AnimatedValueInterface<class Handle<class SoundBusSnapshot::SnapshotSuite> >", typeof(AnimatedValueInterface<Handle<SoundBusSnapshot.SnapshotSuite>>));
        Register("class AnimatedValueInterface<class Handle<class SoundData> >", typeof(AnimatedValueInterface<Handle<SoundData>>));
        Register("class AnimatedValueInterface<class Handle<class SoundEventData> >", typeof(AnimatedValueInterface<Handle<SoundEventData>>));
        Register("class AnimatedValueInterface<class Handle<class SoundEventSnapshotData> >", typeof(AnimatedValueInterface<Handle<SoundEventSnapshotData>>));
        Register("class AnimatedValueInterface<class Handle<class SoundReverbDefinition> >", typeof(AnimatedValueInterface<Handle<SoundReverbDefinition>>));
        Register("class AnimatedValueInterface<class Handle<class T3Texture> >", typeof(AnimatedValueInterface<Handle<T3Texture>>));
        Register("class AnimatedValueInterface<class Handle<class WalkBoxes> >", typeof(AnimatedValueInterface<Handle<WalkBoxes>>));
        Register("class AnimatedValueInterface<class Handle<struct SoundAmbience::AmbienceDefinition> >", typeof(AnimatedValueInterface<Handle<SoundAmbience.AmbienceDefinition>>));
        Register("class AnimatedValueInterface<class LocationInfo>", typeof(AnimatedValueInterface<LocationInfo>));
        Register("class AnimatedValueInterface<class Polar>", typeof(AnimatedValueInterface<Polar>));
        Register("class AnimatedValueInterface<class Quaternion>", typeof(AnimatedValueInterface<Quaternion>));
        Register("class AnimatedValueInterface<class SoundEventName<0> >", typeof(AnimatedValueInterface<SoundEventName>));
        Register("class AnimatedValueInterface<class SoundEventName<1> >", typeof(AnimatedValueInterface<SoundEventName>));
        Register("class AnimatedValueInterface<class SoundEventName<2> >", typeof(AnimatedValueInterface<SoundEventName>));
        Register("class AnimatedValueInterface<class String>", typeof(AnimatedValueInterface<string>));
        Register("class AnimatedValueInterface<class Symbol>", typeof(AnimatedValueInterface<Symbol>));
        Register("class AnimatedValueInterface<class T3VertexBufferSample<class T3NormalSampleData,class T3HeapAllocator> >", typeof(AnimatedValueInterface<T3VertexBufferSample<T3NormalSampleData, T3HeapAllocator>>));
        Register("class AnimatedValueInterface<class T3VertexBufferSample<class T3PositionSampleData,class T3HeapAllocator> >", typeof(AnimatedValueInterface<T3VertexBufferSample<T3PositionSampleData, T3HeapAllocator>>));
        Register("class AnimatedValueInterface<class Transform>", typeof(AnimatedValueInterface<Transform>));
        Register("class AnimatedValueInterface<class Vector2>", typeof(AnimatedValueInterface<Vector2>));
        Register("class AnimatedValueInterface<class Vector3>", typeof(AnimatedValueInterface<Vector3>));
        Register("class AnimatedValueInterface<class Vector4>", typeof(AnimatedValueInterface<Vector4>));
        Register("class AnimatedValueInterface<float>", typeof(AnimatedValueInterface<float>));
        Register("class AnimatedValueInterface<int>", typeof(AnimatedValueInterface<int>));
        Register("class AnimatedValueInterface<struct CompressedPathBlockingValue::CompressedPathInfoKey>", typeof(AnimatedValueInterface<CompressedPathBlockingValue.CompressedPathInfoKey>));
        Register("class AnimatedValueInterface<struct PhonemeKey>", typeof(AnimatedValueInterface<PhonemeKey>));
        Register("class AnimatedValueInterface<struct ScriptEnum>", typeof(AnimatedValueInterface<ScriptEnum>));
        Register("class AnimatedValueInterface<ulong long>", typeof(AnimatedValueInterface<uint>));
        Register("class AnimatedValueInterface<unsigned __int64>", typeof(AnimatedValueInterface<ulong>));
        Register("class Animation", typeof(Animation));
        Register("class AnimationConstraintParameters", typeof(AnimationConstraintParameters));
        Register("class AnimationManager", typeof(AnimationManager));
        Register("class AnimationMixer<class LocationInfo>", typeof(AnimationMixer<LocationInfo>));
        Register("class AnimationValueInterfaceBase", typeof(AnimationValueInterfaceBase));
        Register("class AssetCollection", typeof(AssetCollection));
        Register("class AudioData", typeof(AudioData));
        Register("class AutoActStatus", typeof(AutoActStatus));
        Register("class BGM_HeadTurn_Value", typeof(BGM_HeadTurn_Value));
        Register("class BallJointKey", typeof(BallJointKey));
        Register("class BallTwistJointKey", typeof(BallTwistJointKey));
        Register("class BinaryBuffer", typeof(BinaryBuffer));
        Register("class BitSetBase<1>", typeof(BitSetBase));
        Register("class BitSetBase<2>", typeof(BitSetBase));
        Register("class BitSetBase<3>", typeof(BitSetBase));
        Register("class BitSetBase<4>", typeof(BitSetBase));
        Register("class BitSetBase<5>", typeof(BitSetBase));
        Register("class BitSetBase<6>", typeof(BitSetBase));
        Register("class BitSetBase<7>", typeof(BitSetBase));
        Register("class BitSetBase<8>", typeof(BitSetBase));
        Register("class BitSetBase<9>", typeof(BitSetBase));
        Register("class BlendCameraResource", typeof(BlendCameraResource));
        Register("class BlendEntry", typeof(BlendEntry));
        Register("class BlendGraph", typeof(BlendGraph));
        Register("class BlendGraphManager", typeof(BlendGraphManager));
        Register("class BlendGraphManagerInst", typeof(BlendGraphManagerInst));
        Register("class BlendMode", typeof(BlendMode));
        Register("class BlockingValue", typeof(CompressedPathBlockingValue));
        Register("class BoneContraints", typeof(BoneConstraints));
        Register("class BoundingBox", typeof(BoundingBox), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class Camera", typeof(Camera));
        Register("class CameraFacingTypes", typeof(CameraFacingTypes));
        Register("class CameraSelect", typeof(CameraSelect));
        Register("class Chore", typeof(Chore));
        Register("class ChoreAgent", typeof(ChoreAgent));
        Register("class ChoreAgent::Attachment", typeof(ChoreAgent.Attachment));
        Register("class ChoreAgentInst*__ptr64", typeof(ChoreAgentInst));
        Register("class ChoreAgentInst::SyncValue*__ptr64", typeof(ChoreAgentInst.SyncValue));
        Register("class ChoreInst", typeof(ChoreInst));
        Register("class ChoreInst*__ptr64", typeof(ChoreInst));
        Register("class ChoreResource", typeof(ChoreResource));
        Register("class ChoreResource::Block", typeof(ChoreResource.Block));
        Register("class CinematicLight", typeof(CinematicLight));
        Register("class CinematicLightRig", typeof(CinematicLightRig));
        Register("class Color", typeof(Color), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class ColorHDR", typeof(ColorHDR));
        Register("class CompressedKeys<bool>", typeof(CompressedKeys<bool>));
        Register("class CompressedKeys<class AnimOrChore>", typeof(CompressedKeys<AnimOrChore>));
        Register("class CompressedKeys<class Color>", typeof(CompressedKeys<Color>));
        Register("class CompressedKeys<class Handle<class D3DMesh> >", typeof(CompressedKeys<Handle<D3DMesh>>));
        Register("class CompressedKeys<class Handle<class Dlg> >", typeof(CompressedKeys<Handle<Dlg>>));
        Register("class CompressedKeys<class Handle<class Font> >", typeof(CompressedKeys<Handle<Font>>));
        Register("class CompressedKeys<class Handle<class PhonemeTable> >", typeof(CompressedKeys<Handle<PhonemeTable>>));
        Register("class CompressedKeys<class Handle<class PropertySet> >", typeof(CompressedKeys<Handle<PropertySet>>));
        Register("class CompressedKeys<class Handle<class Scene> >", typeof(CompressedKeys<Handle<Scene>>));
        Register("class CompressedKeys<class Handle<class SoundBusSnapshot::Snapshot> >", typeof(CompressedKeys<Handle<SoundBusSnapshot.Snapshot>>));
        Register("class CompressedKeys<class Handle<class SoundBusSnapshot::SnapshotSuite> >", typeof(CompressedKeys<Handle<SoundBusSnapshot.SnapshotSuite>>));
        Register("class CompressedKeys<class Handle<class SoundData> >", typeof(CompressedKeys<Handle<SoundData>>));
        Register("class CompressedKeys<class Handle<class SoundEventData> >", typeof(CompressedKeys<Handle<SoundEventData>>));
        Register("class CompressedKeys<class Handle<class SoundEventSnapshotData> >", typeof(CompressedKeys<Handle<SoundEventSnapshotData>>));
        Register("class CompressedKeys<class Handle<class SoundReverbDefinition> >", typeof(CompressedKeys<Handle<SoundReverbDefinition>>));
        Register("class CompressedKeys<class Handle<class T3Texture> >", typeof(CompressedKeys<Handle<T3Texture>>));
        Register("class CompressedKeys<class Handle<class WalkBoxes> >", typeof(CompressedKeys<Handle<WalkBoxes>>));
        Register("class CompressedKeys<class Handle<struct SoundAmbience::AmbienceDefinition> >", typeof(CompressedKeys<Handle<SoundAmbience.AmbienceDefinition>>));
        Register("class CompressedKeys<class LocationInfo>", typeof(CompressedKeys<LocationInfo>));
        Register("class CompressedKeys<class Polar>", typeof(CompressedKeys<Polar>));
        Register("class CompressedKeys<class Quaternion>", typeof(CompressedKeys<Quaternion>));
        Register("class CompressedKeys<class SoundEventName<0> >", typeof(CompressedKeys<SoundEventName>));
        Register("class CompressedKeys<class SoundEventName<1> >", typeof(CompressedKeys<SoundEventName>));
        Register("class CompressedKeys<class SoundEventName<2> >", typeof(CompressedKeys<SoundEventName>));
        Register("class CompressedKeys<class String>", typeof(CompressedKeys<String>));
        Register("class CompressedKeys<class Symbol>", typeof(CompressedKeys<Symbol>));
        Register("class CompressedKeys<class T3VertexBufferSample<class T3NormalSampleData,class T3HeapAllocator> >", typeof(CompressedKeys<T3VertexBufferSample<T3NormalSampleData, T3HeapAllocator>>));
        Register("class CompressedKeys<class T3VertexBufferSample<class T3PositionSampleData,class T3HeapAllocator> >", typeof(CompressedKeys<T3VertexBufferSample<T3PositionSampleData, T3HeapAllocator>>));
        Register("class CompressedKeys<class Transform>", typeof(CompressedKeys<Transform>));
        Register("class CompressedKeys<class Vector2>", typeof(CompressedKeys<Vector2>));
        Register("class CompressedKeys<class Vector3>", typeof(CompressedKeys<Vector3>));
        Register("class CompressedKeys<class Vector4>", typeof(CompressedKeys<Vector4>));
        Register("class CompressedKeys<float>", typeof(CompressedKeys<float>));
        Register("class CompressedKeys<int>", typeof(CompressedKeys<int>));
        Register("class CompressedKeys<struct CompressedPathBlockingValue::CompressedPathInfoKey>", typeof(CompressedKeys<CompressedPathBlockingValue.CompressedPathInfoKey>));
        Register("class CompressedKeys<struct PhonemeKey>", typeof(CompressedKeys<PhonemeKey>));
        Register("class CompressedKeys<struct ScriptEnum>", typeof(CompressedKeys<ScriptEnum>));
        Register("class CompressedKeys<unsigned __int64>", typeof(CompressedKeys<ulong>));
        Register("class CompressedPathBlockingValue", typeof(CompressedPathBlockingValue));
        Register("class CompressedPhonemeKeys", typeof(CompressedPhonemeKeys));
        Register("class CompressedQuaternionKeys", typeof(CompressedQuaternionKeys));
        Register("class CompressedQuaternionKeys2", typeof(CompressedQuaternionKeys2));
        Register("class CompressedSkeletonPoseKeys", typeof(CompressedSkeletonPoseKeys));
        Register("class CompressedSkeletonPoseKeys2", typeof(CompressedSkeletonPoseKeys2));
        Register("class CompressedTransformKeys", typeof(CompressedTransformKeys));
        Register("class CompressedVector3Keys", typeof(CompressedVector3Keys));
        Register("class CompressedVector3Keys2", typeof(CompressedVector3Keys2));
        Register("class CompressedVertexNormalKeys", typeof(CompressedVertexNormalKeys));
        Register("class CompressedVertexPositionKeys", typeof(CompressedVertexPositionKeys));
        Register("class ContainerInterface", typeof(ContainerInterface));
        Register("class CorrespondencePoint", typeof(CorrespondencePoint));
        Register("class D3DIndexBuffer", typeof(T3IndexBuffer));
        Register("class D3DMesh", typeof(D3DMesh));
        Register("class D3DMesh::TriangleSet", typeof(D3DMesh.TriangleSet));
        Register("class D3DTexture", typeof(T3Texture));
        Register("class D3DVertexBuffer", typeof(T3VertexBuffer));
        Register("class DArray<bool>", typeof(List<bool>));
        Register("class DArray<class InputMapper * __ptr64>", typeof(List<InputMapper>));
        Register("class DArray<int>", typeof(List<int>));
        Register("class DCArray<EnlightenProbeData>", typeof(List<EnlightenProbeData>));
        Register("class DCArray<EnlightenSystemData>", typeof(List<EnlightenSystemData>));
        Register("class DCArray<bool>", typeof(List<bool>));
        Register("class DCArray<class ActingPalette>", typeof(List<ActingPalette>));
        Register("class DCArray<class ActingPaletteClass>", typeof(List<ActingPaletteClass>));
        Register("class DCArray<class ActingPaletteGroup>", typeof(List<ActingPaletteGroup>));
        Register("class DCArray<class ActingResource>", typeof(List<ActingResource>));
        Register("class DCArray<class AnimOrChore>", typeof(List<AnimOrChore>));
        Register("class DCArray<class BlendEntry>", typeof(List<BlendEntry>));
        Register("class DCArray<class ChoreAgent>", typeof(List<ChoreAgent>));
        Register("class DCArray<class ChoreResource::Block>", typeof(List<ChoreResource.Block>));
        Register("class DCArray<class ChoreResource>", typeof(List<ChoreResource>));
        Register("class DCArray<class Color>", typeof(List<Color>));
        Register("class DCArray<class CorrespondencePoint>", typeof(List<CorrespondencePoint>));
        Register("class DCArray<class D3DMesh::PaletteEntry>", typeof(List<D3DMesh.PaletteEntry>));
        Register("class DCArray<class D3DMesh::TriangleSet>", typeof(List<D3DMesh.TriangleSet>));
        Register("class DCArray<class D3DTexture>", typeof(List<T3Texture>));
        Register("class DCArray<class DCArray<class PropertySet> >", typeof(List<List<PropertySet>>));
        Register("class DCArray<class DCArray<class String> >", typeof(List<List<string>>));
        Register("class DCArray<class DCArray<struct D3DMesh::PaletteEntry> >", typeof(List<List<D3DMesh.PaletteEntry>>));
        Register("class DCArray<class FileName<class SoundEventBankDummy> >", typeof(List<FileName<SoundEventBankDummy>>));
        Register("class DCArray<class FontConfig>", typeof(List<FontConfig>));
        Register("class DCArray<class Guide>", typeof(List<Guide>));
        Register("class DCArray<class Handle<class AnimOrChore> >", typeof(List<Handle<AnimOrChore>>));
        Register("class DCArray<class Handle<class AudioData> >", typeof(List<Handle<AudioData>>));
        Register("class DCArray<class Handle<class Chore> >", typeof(List<Handle<Chore>>));
        Register("class DCArray<class Handle<class D3DMesh> >", typeof(List<Handle<D3DMesh>>));
        Register("class DCArray<class Handle<class PropertySet> >", typeof(List<Handle<PropertySet>>));
        Register("class DCArray<class Handle<class Rules> >", typeof(List<Handle<Rules>>));
        Register("class DCArray<class Handle<class Scene> >", typeof(List<Handle<Scene>>));
        Register("class DCArray<class Handle<class SoundData> >", typeof(List<Handle<SoundData>>));
        Register("class DCArray<class Handle<class T3Texture> >", typeof(List<Handle<T3Texture>>));
        Register("class DCArray<class HandleBase>", typeof(List<HandleBase>));
        Register("class DCArray<class HandleLock<class Scene> >", typeof(List<Handle<Scene>>));
        Register("class DCArray<class InputMapper::EventMapping>", typeof(List<InputMapper.EventMapping>));
        Register("class DCArray<class KeyframedValue<bool>::Sample>", typeof(List<KeyframedValue<bool>.Sample>));
        Register("class DCArray<class KeyframedValue<class AnimOrChore>::Sample>", typeof(List<KeyframedValue<AnimOrChore>.Sample>));
        Register("class DCArray<class KeyframedValue<class Color>::Sample>", typeof(List<KeyframedValue<Color>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class Chore> >::Sample>", typeof(List<KeyframedValue<Handle<Chore>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class D3DMesh> >::Sample>", typeof(List<KeyframedValue<Handle<D3DMesh>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class Dlg> >::Sample>", typeof(List<KeyframedValue<Handle<Dlg>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class Font> >::Sample>", typeof(List<KeyframedValue<Handle<Font>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class PhonemeTable> >::Sample>", typeof(List<KeyframedValue<Handle<PhonemeTable>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class PropertySet> >::Sample>", typeof(List<KeyframedValue<Handle<PropertySet>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class Scene> >::Sample>", typeof(List<KeyframedValue<Handle<Scene>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class SoundBusSnapshot::Snapshot> >::Sample>", typeof(List<KeyframedValue<Handle<SoundBusSnapshot.Snapshot>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class SoundBusSnapshot::SnapshotSuite> >::Sample>", typeof(List<KeyframedValue<Handle<SoundBusSnapshot.SnapshotSuite>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class SoundData> >::Sample>", typeof(List<KeyframedValue<Handle<SoundData>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class SoundEventData> >::Sample>", typeof(List<KeyframedValue<Handle<SoundEventData>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class SoundEventSnapshotData> >::Sample>", typeof(List<KeyframedValue<Handle<SoundEventSnapshotData>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class SoundReverbDefinition> >::Sample>", typeof(List<KeyframedValue<Handle<SoundReverbDefinition>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class T3Texture> >::Sample>", typeof(List<KeyframedValue<Handle<T3Texture>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<class WalkBoxes> >::Sample>", typeof(List<KeyframedValue<Handle<WalkBoxes>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Handle<struct SoundAmbience::AmbienceDefinition> >::Sample>", typeof(List<KeyframedValue<Handle<SoundAmbience.AmbienceDefinition>>.Sample>));
        Register("class DCArray<class KeyframedValue<class LocationInfo>::Sample>", typeof(List<KeyframedValue<LocationInfo>.Sample>));
        Register("class DCArray<class KeyframedValue<class PhonemeKey>>", typeof(List<KeyframedValue<PhonemeKey>>));
        Register("class DCArray<class KeyframedValue<class Polar>::Sample>", typeof(List<KeyframedValue<Polar>.Sample>));
        Register("class DCArray<class KeyframedValue<class Quaternion>::Sample>", typeof(List<KeyframedValue<Quaternion>.Sample>));
        Register("class DCArray<class KeyframedValue<class SoundEventName<0> >::Sample>", typeof(List<KeyframedValue<SoundEventName>.Sample>));
        Register("class DCArray<class KeyframedValue<class SoundEventName<1> >::Sample>", typeof(List<KeyframedValue<SoundEventName>.Sample>));
        Register("class DCArray<class KeyframedValue<class SoundEventName<2> >::Sample>", typeof(List<KeyframedValue<SoundEventName>.Sample>));
        Register("class DCArray<class KeyframedValue<class String>::Sample>", typeof(List<KeyframedValue<string>.Sample>));
        Register("class DCArray<class KeyframedValue<class Symbol>::Sample>", typeof(List<KeyframedValue<Symbol>.Sample>));
        Register("class DCArray<class KeyframedValue<class T3VertexBufferSample<class T3NormalSampleData,class T3HeapAllocator> >::Sample>", typeof(List<KeyframedValue<T3VertexBufferSample<T3NormalSampleData, T3HeapAllocator>>.Sample>));
        Register("class DCArray<class KeyframedValue<class T3VertexBufferSample<class T3PositionSampleData,class T3HeapAllocator> >::Sample>", typeof(List<KeyframedValue<T3VertexBufferSample<T3PositionSampleData, T3HeapAllocator>>.Sample>));
        Register("class DCArray<class KeyframedValue<class Transform>::Sample>", typeof(List<KeyframedValue<Transform>.Sample>));
        Register("class DCArray<class KeyframedValue<class Vector2>::Sample>", typeof(List<KeyframedValue<Vector2>.Sample>));
        Register("class DCArray<class KeyframedValue<class Vector3>::Sample>", typeof(List<KeyframedValue<Vector3>.Sample>));
        Register("class DCArray<class KeyframedValue<class Vector4>::Sample>", typeof(List<KeyframedValue<Vector4>.Sample>));
        Register("class DCArray<class KeyframedValue<float>::Sample>", typeof(List<KeyframedValue<float>.Sample>));
        Register("class DCArray<class KeyframedValue<int>::Sample>", typeof(List<KeyframedValue<int>.Sample>));
        Register("class DCArray<class KeyframedValue<struct CompressedPathBlockingValue::CompressedPathInfoKey>::Sample>", typeof(List<KeyframedValue<CompressedPathBlockingValue.CompressedPathInfoKey>.Sample>));
        Register("class DCArray<class KeyframedValue<struct PhonemeKey>::Sample>", typeof(List<KeyframedValue<PhonemeKey>.Sample>));
        Register("class DCArray<class KeyframedValue<struct ScriptEnum>::Sample>", typeof(List<KeyframedValue<ScriptEnum>.Sample>));
        Register("class DCArray<class KeyframedValue<unsigned __int64>::Sample>", typeof(List<KeyframedValue<ulong>.Sample>));
        Register("class DCArray<class LanguageLookupMap::DlgIDSet>", typeof(List<LanguageLookupMap.DlgIdSet>));
        Register("class DCArray<class LanguageResLocal>", typeof(List<LanguageResLocal>));
        Register("class DCArray<class LightGroupInstance>", typeof(List<LightGroupInstance>));
        Register("class DCArray<class LogicGroup>", typeof(List<LogicGroup>));
        Register("class DCArray<class Map<class String,class String,struct std::less<class String> > >", typeof(List<Dictionary<String, String>>));
        Register("class DCArray<class ParticlePropConnect>", typeof(List<ParticlePropConnect>));
        Register("class DCArray<class ProjectDatabaseIDPair>", typeof(List<ProjectDatabaseIdPair>));
        Register("class DCArray<class PropertySet>", typeof(List<PropertySet>));
        Register("class DCArray<class Ptr<class ActingAccentPalette> >", typeof(List<ActingAccentPalette>));
        Register("class DCArray<class Ptr<class ActingPalette> >", typeof(List<ActingPalette>));
        Register("class DCArray<class Ptr<class ActingPaletteClass> >", typeof(List<ActingPaletteClass>));
        Register("class DCArray<class Ptr<class ActingPaletteGroup> >", typeof(List<ActingPaletteGroup>));
        Register("class DCArray<class Ptr<class AnimationValueInterfaceBase> >", typeof(List<AnimationValueInterfaceBase>));
        Register("class DCArray<class Ptr<class DlgChild> >", typeof(List<DlgChild>));
        Register("class DCArray<class Ptr<class DlgChoiceInstance>>", typeof(NotImplementedException));
        Register("class DCArray<class RenderObject_Mesh::MeshInstance>", typeof(List<RenderObject_Mesh.MeshInstance>));
        Register("class DCArray<class ResourceBundle::ResourceInfo>", typeof(List<ResourceBundle.ResourceInfo>));
        Register("class DCArray<class SaveGame::AgentInfo>", typeof(List<SaveGame.AgentInfo>));
        Register("class DCArray<class Skeleton::Entry>", typeof(List<Skeleton.Entry>));
        Register("class DCArray<class String>", typeof(List<string>));
        Register("class DCArray<class StyleGuideRef>", typeof(List<StyleGuideRef>));
        Register("class DCArray<class Symbol>", typeof(List<Symbol>));
        Register("class DCArray<class T3Texture>", typeof(List<T3Texture>));
        Register("class DCArray<class T3ToonGradientRegion>", typeof(List<T3ToonGradientRegion>));
        Register("class DCArray<class Transform>", typeof(List<Transform>));
        Register("class DCArray<class Vector2>", typeof(List<Vector2>));
        Register("class DCArray<class Vector3>", typeof(List<Vector3>));
        Register("class DCArray<class WalkBoxes::Quad>", typeof(List<WalkBoxes.Quad>));
        Register("class DCArray<class WalkBoxes::Tri>", typeof(List<WalkBoxes.Tri>));
        Register("class DCArray<class WalkBoxes::Vert>", typeof(List<WalkBoxes.Vert>));
        Register("class DCArray<class WorkingMesh::BonePalette>", typeof(List<WorkingMesh.BonePalette>));
        Register("class DCArray<class WorkingMesh::Mesh>", typeof(List<WorkingMesh.Mesh>));
        Register("class DCArray<class WorkingMesh::Shader>", typeof(List<WorkingMesh.Shader>));
        Register("class DCArray<class WorkingMesh::Triangle>", typeof(List<WorkingMesh.Triangle>));
        Register("class DCArray<class WorkingMesh::Vertex>", typeof(List<WorkingMesh.Vertex>));
        Register("class DCArray<float>", typeof(List<float>));
        Register("class DCArray<int>", typeof(List<int>));
        Register("class DCArray<struct DlgNodeExchange::Entry>", typeof(List<DlgNodeExchange.Entry>));
        Register("class DCArray<struct DlgNodeInstanceParallel::ElemInstanceData>", typeof(NotImplementedException));
        Register("class DCArray<struct DlgNodeInstanceSequence::ElemInstanceData>", typeof(NotImplementedException));
        Register("class DCArray<struct EventStorage::PageEntry>", typeof(List<EventStorage.PageEntry>));
        Register("class DCArray<struct Font::GlyphInfo>", typeof(List<Font.GlyphInfo>));
        Register("class DCArray<struct MeshSceneLightmapData::Entry>", typeof(List<MeshSceneLightmapData.Entry>));
        Register("class DCArray<struct ParticleProperties::Animation>", typeof(List<ParticleProperties.Animation>));
        Register("class DCArray<struct ParticleSprite::Animation>", typeof(List<ParticleSprite.Animation>));
        Register("class DCArray<struct PreloadPackage::ResourceKey>", typeof(List<PreloadPackage.ResourceKey>));
        Register("class DCArray<struct PreloadPackage::RuntimeDataDialog::DialogResourceInfo>", typeof(List<PreloadPackage.RuntimeDataDialog.DialogResourceInfo>));
        Register("class DCArray<struct PreloadPackage::RuntimeDataDialog::DlgObjIdAndResourceVector>", typeof(List<PreloadPackage.RuntimeDataDialog.DlgObjIdAndResourceVector>));
        Register("class DCArray<struct PreloadPackage::RuntimeDataDialog::DlgObjIdAndStartNodeOffset>", typeof(List<PreloadPackage.RuntimeDataDialog.DlgObjIdAndStartNodeOffset>));
        Register("class DCArray<struct Procedural_LookAt::Constraint>", typeof(List<ProceduralLookAt.Constraint>));
        Register("class DCArray<struct RenderObject_Mesh::TextureInstance>", typeof(List<RenderObject_Mesh.TextureInstance>));
        Register("class DCArray<struct SkeletonPoseValue::BoneEntry>", typeof(List<SkeletonPoseValue.BoneEntry>));
        Register("class DCArray<struct SkeletonPoseValue::Sample>", typeof(List<SkeletonPoseValue.Sample>));
        Register("class DCArray<struct SklNodeData>", typeof(List<SklNodeData>));
        Register("class DCArray<struct SoundAmbience::EventContext>", typeof(List<SoundAmbience.EventContext>));
        Register("class DCArray<struct T3LightSceneInternalData::LightmapPage>", typeof(List<T3LightSceneInternalData.LightmapPage>));
        Register("class DCArray<struct T3MaterialCompiledData>", typeof(List<T3MaterialCompiledData>));
        Register("class DCArray<struct T3MaterialNestedMaterial>", typeof(List<T3MaterialNestedMaterial>));
        Register("class DCArray<struct T3MaterialParameter>", typeof(List<T3MaterialParameter>));
        Register("class DCArray<struct T3MaterialPassData>", typeof(List<T3MaterialPassData>));
        Register("class DCArray<struct T3MaterialPreShader>", typeof(List<T3MaterialPreShader>));
        Register("class DCArray<struct T3MaterialRuntimeProperty>", typeof(List<T3MaterialRuntimeProperty>));
        Register("class DCArray<struct T3MaterialStaticParameter>", typeof(List<T3MaterialStaticParameter>));
        Register("class DCArray<struct T3MaterialTexture>", typeof(List<T3MaterialTexture>));
        Register("class DCArray<struct T3MaterialTextureParam>", typeof(List<T3MaterialTextureParam>));
        Register("class DCArray<struct T3MaterialTransform2D>", typeof(List<T3MaterialTransform2D>));
        Register("class DCArray<struct T3MeshBatch>", typeof(List<T3MeshBatch>));
        Register("class DCArray<struct T3MeshBoneEntry>", typeof(List<T3MeshBoneEntry>));
        Register("class DCArray<struct T3MeshEffectPreload>", typeof(List<T3MeshEffectPreload>));
        Register("class DCArray<struct T3MeshEffectPreloadDynamicFeatures>", typeof(List<T3MeshEffectPreloadDynamicFeatures>));
        Register("class DCArray<struct T3MeshEffectPreloadEntry>", typeof(List<T3MeshEffectPreloadEntry>));
        Register("class DCArray<struct T3MeshLOD>", typeof(List<T3MeshLOD>));
        Register("class DCArray<struct T3MeshLocalTransformEntry>", typeof(List<T3MeshLocalTransformEntry>));
        Register("class DCArray<struct T3MeshMaterial>", typeof(List<T3MeshMaterial>));
        Register("class DCArray<struct T3MeshMaterialOverride>", typeof(List<T3MeshMaterialOverride>));
        Register("class DCArray<struct T3MeshPropertyEntry>", typeof(List<T3MeshPropertyEntry>));
        Register("class DCArray<struct T3MeshTexture>", typeof(List<T3MeshTexture>));
        Register("class DCArray<struct T3OcclusionMeshBatch>", typeof(List<T3OcclusionMeshBatch>));
        Register("class DCArray<struct T3OverlayObjectData_Sprite>", typeof(List<T3OverlayObjectDataSprite>));
        Register("class DCArray<struct T3OverlayObjectData_Text>", typeof(List<T3OverlayObjectDataText>));
        Register("class DCArray<unsigned char>", typeof(List<byte>));
        Register("class DCArray<unsigned int>", typeof(List<uint>));
        Register("class DCArray<unsigned short>", typeof(List<ushort>));
        Register("class DateStamp", typeof(DateStamp));
        Register("class DebugString", typeof(DebugString));
        Register("class DelaunayTriangleSet", typeof(DelaunayTriangleSet));
        Register("class DependencyLoader<1>", typeof(DependencyLoader)); // TODO?
        Register("class DialogBase", typeof(DialogBase));
        Register("class DialogBranch", typeof(DialogBranch));
        Register("class DialogDialog", typeof(DialogDialog));
        Register("class DialogExchange", typeof(DialogExchange));
        Register("class DialogInstance::InstanceID", typeof(DialogInstance.InstanceID));
        Register("class DialogItem", typeof(DialogItem));
        Register("class DialogLine", typeof(DialogLine));
        Register("class DialogResource", typeof(DialogResource));
        Register("class DialogText", typeof(DialogText));
        Register("class Dlg", typeof(Dlg));
        Register("class DlgChainHead", typeof(DlgChainHead));
        Register("class DlgChild", typeof(DlgChild));
        Register("class DlgChildSet", typeof(DlgChildSet));
        Register("class DlgChildSetChoice", typeof(DlgChildSetChoice));
        Register("class DlgChildSetChoicesChildPost", typeof(DlgChildSetChoicesChildPost));
        Register("class DlgChildSetChoicesChildPre", typeof(DlgChildSetChoicesChildPre));
        Register("class DlgChildSetConditionalCase", typeof(DlgChildSetConditionalCase));
        Register("class DlgChoice", typeof(DlgChoice));
        Register("class DlgChoicesChildPost", typeof(DlgChoicesChildPost));
        Register("class DlgChoicesChildPre", typeof(DlgChoicesChildPre));
        Register("class DlgCondition", typeof(DlgCondition));
        Register("class DlgConditionInput", typeof(DlgConditionInput));
        Register("class DlgConditionRule", typeof(DlgConditionRule));
        Register("class DlgConditionSet", typeof(DlgConditionSet));
        Register("class DlgConditionTime", typeof(DlgConditionTime));
        Register("class DlgConditionalCase", typeof(DlgConditionalCase));
        Register("class DlgDownstreamVisibilityConditions", typeof(DlgDownstreamVisibilityConditions));
        Register("class DlgFolder", typeof(DlgFolder));
        Register("class DlgFolderChild", typeof(DlgFolderChild));
        Register("class DlgLine", typeof(DlgLine));
        Register("class DlgLineCollection", typeof(DlgLineCollection));
        Register("class DlgNode", typeof(DlgNode));
        Register("class DlgNodeCancelChoices", typeof(DlgNodeCancelChoices));
        Register("class DlgNodeChoices", typeof(DlgNodeChoices));
        Register("class DlgNodeChore", typeof(DlgNodeChore));
        Register("class DlgNodeConditional", typeof(DlgNodeConditional));
        Register("class DlgNodeCriteria", typeof(DlgNodeCriteria));
        Register("class DlgNodeExchange", typeof(DlgNodeExchange));
        Register("class DlgNodeExit", typeof(DlgNodeExit));
        Register("class DlgNodeIdle", typeof(DlgNodeIdle));
        Register("class DlgNodeJump", typeof(DlgNodeJump));
        Register("class DlgNodeLink", typeof(DlgNodeLink));
        Register("class DlgNodeLogic", typeof(DlgNodeLogic));
        Register("class DlgNodeMarker", typeof(DlgNodeMarker));
        Register("class DlgNodeNotes", typeof(DlgNodeNotes));
        Register("class DlgNodeParallel", typeof(DlgNodeParallel));
        Register("class DlgNodeParallel::DlgChildSetElement", typeof(DlgNodeParallel.DlgChildSetElement));
        Register("class DlgNodeParallel::PElement", typeof(DlgNodeParallel.PElement));
        Register("class DlgNodeScript", typeof(DlgNodeScript));
        Register("class DlgNodeSequence", typeof(DlgNodeSequence));
        Register("class DlgNodeSequence::DlgChildSetElement", typeof(DlgNodeSequence.DlgChildSetElement));
        Register("class DlgNodeSequence::Element", typeof(DlgNodeSequence.Element));
        Register("class DlgNodeStart", typeof(DlgNodeStart));
        Register("class DlgNodeStats", typeof(DlgNodeStats));
        Register("class DlgNodeStats::Cohort", typeof(DlgNodeStats.Cohort));
        Register("class DlgNodeStats::DlgChildSetCohort", typeof(DlgNodeStats.DlgChildSetCohort));
        Register("class DlgNodeStoryBoard", typeof(DlgNodeStoryBoard));
        Register("class DlgNodeText", typeof(DlgNodeText));
        Register("class DlgNodeWait", typeof(DlgNodeWait));
        Register("class DlgObjID", typeof(DlgObjId));
        Register("class DlgObjIDOwner", typeof(DlgObjIDOwner));
        Register("class DlgObjectProps", typeof(DlgObjectProps));
        Register("class DlgObjectPropsMap", typeof(DlgObjectPropsMap));
        Register("class DlgObjectPropsMap::GroupDefinition", typeof(DlgObjectPropsMap.GroupDefinition));
        Register("class DlgObjectPropsOwner", typeof(DlgObjectPropsOwner));
        Register("class DlgSystemSettings", typeof(DlgSystemSettings));
        Register("class DlgVisibilityConditions", typeof(DlgVisibilityConditions));
        Register("class DlgVisibilityConditionsOwner", typeof(DlgVisibilityConditionsOwner));
        Register("class EnlightenData", typeof(EnlightenData));
        Register("class EnlightenModule", typeof(EnlightenModule));
        Register("class EnlightenProbeData", typeof(EnlightenProbeData));
        Register("class EnlightenSignature", typeof(EnlightenSignature));
        Register("class EnlightenSystem", typeof(EnlightenSystem));
        Register("class EnlightenSystemData", typeof(EnlightenSystemData));
        Register("class Environment", typeof(Environment));
        Register("class EnvironmentLight", typeof(EnvironmentLight));
        Register("class EnvironmentLightGroup", typeof(EnvironmentLightGroup));
        Register("class EnvironmentTile", typeof(EnvironmentTile));
        Register("class EventLoggerEvent", typeof(EventLoggerEvent));
        Register("class EventStorage", typeof(EventStorage));
        Register("class EventStorage::PageEntry", typeof(EventStorage.PageEntry));
        Register("class EventStoragePage", typeof(EventStoragePage));
        Register("class FileName<class SoundEventBankDummy>", typeof(FileName<SoundEventBankDummy>));
        Register("class FileNameBase", typeof(FileNameBase));
        Register("class FilterArea", typeof(FilterArea));
        Register("class Flags", typeof(Flags), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class Font", typeof(Font));
        Register("class Font::FontCreateParam", typeof(Font.FontCreateParam));
        Register("class FontConfig", typeof(FontConfig));
        Register("class FontTool::EnumLanguageSet", typeof(FontTool.EnumLanguageSet));
        Register("class FootSteps", typeof(FootSteps));
        Register("class Handle", typeof(Handle<>));
        Register("class Handle<class ActorAgentMapper>", typeof(Handle<ActorAgentMapper>));
        Register("class Handle<class AgentMap>", typeof(Handle<AgentMap>));
        Register("class Handle<class AnimOrChore>", typeof(Handle<AnimOrChore>));
        Register("class Handle<class Animation>", typeof(Handle<Animation>));
        Register("class Handle<class AudioData>", typeof(Handle<AudioData>));
        Register("class Handle<class BlendGraph>", typeof(Handle<BlendGraph>));
        Register("class Handle<class BlendGraphManager>", typeof(Handle<BlendGraphManager>));
        Register("class Handle<class BlendMode>", typeof(Handle<BlendMode>));
        Register("class Handle<class Chore>", typeof(Handle<Chore>));
        Register("class Handle<class D3DMesh>", typeof(Handle<D3DMesh>));
        Register("class Handle<class D3DTexture>", typeof(Handle<T3Texture>));
        Register("class Handle<class DialogResource>", typeof(Handle<DialogResource>));
        Register("class Handle<class Dlg>", typeof(Handle<Dlg>));
        Register("class Handle<class EventStorage>", typeof(Handle<EventStorage>));
        Register("class Handle<class EventStoragePage>", typeof(Handle<EventStoragePage>));
        Register("class Handle<class Font>", typeof(Handle<Font>));
        Register("class Handle<class InputMapper>", typeof(Handle<InputMapper>));
        Register("class Handle<class LanguageDatabase>", typeof(Handle<LanguageDatabase>));
        Register("class Handle<class LanguageResource>", typeof(Handle<LanguageResource>));
        Register("class Handle<class LightProbeData>", typeof(Handle<LightProbeData>));
        Register("class Handle<class LocomotionDB>", typeof(Handle<LocomotionDb>));
        Register("class Handle<class ParticleProperties>", typeof(Handle<ParticleProperties>));
        Register("class Handle<class ParticleSprite>", typeof(Handle<ParticleSprite>));
        Register("class Handle<class PhonemeTable>", typeof(Handle<PhonemeTable>));
        Register("class Handle<class PhysicsData>", typeof(Handle<PhysicsData>));
        Register("class Handle<class PhysicsObject>", typeof(Handle<PhysicsObject>));
        Register("class Handle<class PreloadPackage::RuntimeDataDialog>", typeof(Handle<PreloadPackage.RuntimeDataDialog>));
        Register("class Handle<class PreloadPackage::RuntimeDataScene>", typeof(Handle<PreloadPackage.RuntimeDataScene>));
        Register("class Handle<class PropertySet>", typeof(Handle<PropertySet>));
        Register("class Handle<class ResourceBundle>", typeof(Handle<ResourceBundle>));
        Register("class Handle<class Rule>", typeof(Handle<Rule>));
        Register("class Handle<class Rules>", typeof(Handle<Rules>));
        Register("class Handle<class SaveGame>", typeof(Handle<SaveGame>));
        Register("class Handle<class Scene>", typeof(Handle<Scene>));
        Register("class Handle<class Skeleton>", typeof(Handle<Skeleton>));
        Register("class Handle<class SoundBusSnapshot::Snapshot>", typeof(Handle<SoundBusSnapshot.Snapshot>));
        Register("class Handle<class SoundBusSnapshot::SnapshotSuite>", typeof(Handle<SoundBusSnapshot.SnapshotSuite>));
        Register("class Handle<class SoundData>", typeof(Handle<SoundData>));
        Register("class Handle<class SoundEventBankDummy>", typeof(Handle<SoundEventBankDummy>));
        Register("class Handle<class SoundEventData>", typeof(Handle<SoundEventData>));
        Register("class Handle<class SoundEventSnapshotData>", typeof(Handle<SoundEventSnapshotData>));
        Register("class Handle<class SoundReverbDefinition>", typeof(Handle<SoundReverbDefinition>));
        Register("class Handle<class StyleGuide>", typeof(Handle<StyleGuide>));
        Register("class Handle<class StyleGuideRef>", typeof(Handle<StyleGuideRef>));
        Register("class Handle<class StyleIdleTransitionsRes>", typeof(Handle<StyleIdleTransitionsRes>));
        Register("class Handle<class T3OverlayData>", typeof(Handle<T3OverlayData>));
        Register("class Handle<class T3Texture>", typeof(Handle<T3Texture>));
        Register("class Handle<class TransitionMap>", typeof(Handle<TransitionMap>));
        Register("class Handle<class VoiceData>", typeof(Handle<VoiceData>));
        Register("class Handle<class WalkBoxes>", typeof(Handle<WalkBoxes>));
        Register("class Handle<struct ResourceGroupInfo>", typeof(Handle<ResourceGroupInfo>));
        Register("class Handle<struct SoundAmbience::AmbienceDefinition>", typeof(Handle<SoundAmbience.AmbienceDefinition>));
        Register("class HandleBase", typeof(HandleBase));
        Register("class HandleLock<class Animation>", typeof(Handle<Animation>));
        Register("class HandleLock<class LanguageRes>", typeof(Handle<LanguageRes>));
        Register("class HandleLock<class LanguageResource>", typeof(Handle<LanguageResource>));
        Register("class HandleLock<class PropertySet>", typeof(Handle<PropertySet>));
        Register("class HandleLock<class Scene>", typeof(Handle<Scene>));
        Register("class HandleLock<class Skeleton>", typeof(Handle<Skeleton>));
        Register("class HandleObjectInfo", typeof(HandleObjectInfo));
        Register("class HermiteCurvePathSegment", typeof(HermiteCurvePathSegment));
        Register("class HingeJointKey", typeof(HingeJointKey));
        Register("class IdleSlotDefaults", typeof(IdleSlotDefaults));
        Register("class IdleTransitionSettings", typeof(IdleTransitionSettings));
        Register("class InputMapper", typeof(InputMapper));
        Register("class InputMapper::EventMapping", typeof(InputMapper.EventMapping));
        Register("class InputMapper::RawEvent", typeof(InputMapper.RawEvent));
        Register("class IntrusiveSet<class Symbol,class PropertySet::KeyInfo,struct TagPropertyKeyInfoSet,struct Symbol::CompareCRC>", typeof(NotImplementedException));
        Register("class InverseKinematics", typeof(InverseKinematics));
        Register("class InverseKinematicsAttach", typeof(InverseKinematicsAttach));
        Register("class InverseKinematicsBase", typeof(InverseKinematicsBase));
        Register("class InverseKinematicsDerived", typeof(InverseKinematicsDerived));
        Register("class JiraRecordManager", typeof(JiraRecordManager));
        Register("class KeyframedValue<bool>", typeof(KeyframedValue<bool>));
        Register("class KeyframedValue<bool>::Sample", typeof(KeyframedValue<bool>.Sample));
        Register("class KeyframedValue<class AnimOrChore>", typeof(KeyframedValue<AnimOrChore>));
        Register("class KeyframedValue<class AnimOrChore>::Sample", typeof(KeyframedValue<AnimOrChore>.Sample));
        Register("class KeyframedValue<class Color>", typeof(KeyframedValue<Color>));
        Register("class KeyframedValue<class Color>::Sample", typeof(KeyframedValue<Color>.Sample));
        Register("class KeyframedValue<class Handle<class Chore> >", typeof(KeyframedValue<Handle<Chore>>));
        Register("class KeyframedValue<class Handle<class D3DMesh> >", typeof(KeyframedValue<Handle<D3DMesh>>));
        Register("class KeyframedValue<class Handle<class Dlg> >", typeof(KeyframedValue<Handle<Dlg>>));
        Register("class KeyframedValue<class Handle<class Font> >", typeof(KeyframedValue<Handle<Font>>));
        Register("class KeyframedValue<class Handle<class PhonemeTable> >", typeof(KeyframedValue<Handle<PhonemeTable>>));
        Register("class KeyframedValue<class Handle<class PropertySet> >", typeof(KeyframedValue<Handle<PropertySet>>));
        Register("class KeyframedValue<class Handle<class Scene> >", typeof(KeyframedValue<Handle<Scene>>));
        Register("class KeyframedValue<class Handle<class SoundBusSnapshot::Snapshot> >", typeof(KeyframedValue<Handle<SoundBusSnapshot.Snapshot>>));
        Register("class KeyframedValue<class Handle<class SoundBusSnapshot::SnapshotSuite> >", typeof(KeyframedValue<Handle<SoundBusSnapshot.SnapshotSuite>>));
        Register("class KeyframedValue<class Handle<class SoundData> >", typeof(KeyframedValue<Handle<SoundData>>));
        Register("class KeyframedValue<class Handle<class SoundEventData> >", typeof(KeyframedValue<Handle<SoundEventData>>));
        Register("class KeyframedValue<class Handle<class SoundEventSnapshotData> >", typeof(KeyframedValue<Handle<SoundEventSnapshotData>>));
        Register("class KeyframedValue<class Handle<class SoundReverbDefinition> >", typeof(KeyframedValue<Handle<SoundReverbDefinition>>));
        Register("class KeyframedValue<class Handle<class T3Texture> >", typeof(KeyframedValue<Handle<T3Texture>>));
        Register("class KeyframedValue<class Handle<class WalkBoxes> >", typeof(KeyframedValue<Handle<WalkBoxes>>));
        Register("class KeyframedValue<class Handle<struct SoundAmbience::AmbienceDefinition> >", typeof(KeyframedValue<Handle<SoundAmbience.AmbienceDefinition>>));
        Register("class KeyframedValue<class LocationInfo>", typeof(KeyframedValue<LocationInfo>));
        Register("class KeyframedValue<class PhonemeKey>::Sample", typeof(KeyframedValue<PhonemeKey>.Sample));
        Register("class KeyframedValue<class Polar>", typeof(KeyframedValue<Polar>));
        Register("class KeyframedValue<class Quaternion>", typeof(KeyframedValue<Quaternion>));
        Register("class KeyframedValue<class Quaternion>::Sample", typeof(KeyframedValue<Quaternion>.Sample));
        Register("class KeyframedValue<class SoundEventName<0> >", typeof(KeyframedValue<SoundEventName>));
        Register("class KeyframedValue<class SoundEventName<1> >", typeof(KeyframedValue<SoundEventName>));
        Register("class KeyframedValue<class SoundEventName<2> >", typeof(KeyframedValue<SoundEventName>));
        Register("class KeyframedValue<class String>", typeof(KeyframedValue<string>));
        Register("class KeyframedValue<class String>::Sample", typeof(KeyframedValue<string>.Sample));
        Register("class KeyframedValue<class Symbol>", typeof(KeyframedValue<Symbol>));
        Register("class KeyframedValue<class T3VertexBufferSample<class T3NormalSampleData,class T3HeapAllocator> >", typeof(KeyframedValue<T3VertexBufferSample<T3NormalSampleData, T3HeapAllocator>>));
        Register("class KeyframedValue<class T3VertexBufferSample<class T3PositionSampleData,class T3HeapAllocator> >", typeof(KeyframedValue<T3VertexBufferSample<T3PositionSampleData, T3HeapAllocator>>));
        Register("class KeyframedValue<class Transform>", typeof(KeyframedValue<Transform>));
        Register("class KeyframedValue<class Transform>::Sample", typeof(KeyframedValue<Transform>.Sample));
        Register("class KeyframedValue<class Vector2>", typeof(KeyframedValue<Vector2>));
        Register("class KeyframedValue<class Vector3>", typeof(KeyframedValue<Vector3>));
        Register("class KeyframedValue<class Vector3>::Sample", typeof(KeyframedValue<Vector3>.Sample));
        Register("class KeyframedValue<class Vector4>", typeof(KeyframedValue<Vector4>));
        Register("class KeyframedValue<float>", typeof(KeyframedValue<float>));
        Register("class KeyframedValue<float>::Sample", typeof(KeyframedValue<float>.Sample));
        Register("class KeyframedValue<int>", typeof(KeyframedValue<int>));
        Register("class KeyframedValue<int>::Sample", typeof(KeyframedValue<int>.Sample));
        Register("class KeyframedValue<struct CompressedPathBlockingValue::CompressedPathInfoKey>", typeof(KeyframedValue<CompressedPathBlockingValue.CompressedPathInfoKey>));
        Register("class KeyframedValue<struct PhonemeKey>", typeof(KeyframedValue<PhonemeKey>));
        Register("class KeyframedValue<struct ScriptEnum>", typeof(KeyframedValue<ScriptEnum>));
        Register("class KeyframedValue<unsigned __int64>", typeof(KeyframedValue<ulong>));
        Register("class KeyframedValue<unsigned __int64>::Sample", typeof(KeyframedValue<ulong>.Sample));
        Register("class KeyframedValueInterface", typeof(KeyframedValueInterface), MetaFlags.MetaSerializeDisable);
        Register("class KeyframedValueSteppedString", typeof(KeyframedValueSteppedString));
        Register("class LanguageDB", typeof(LanguageDb));
        Register("class LanguageDatabase", typeof(LanguageDatabase));
        Register("class LanguageLookupMap", typeof(LanguageLookupMap));
        Register("class LanguageRes", typeof(LanguageRes));
        Register("class LanguageResLocal", typeof(LanguageResLocal));
        Register("class LanguageResProxy", typeof(LanguageResProxy));
        Register("class LanguageResource", typeof(LanguageResource));
        Register("class LanguageResourceProxy", typeof(LanguageResourceProxy));
        Register("class LightInstance", typeof(LightInstance));
        Register("class LightProbe", typeof(LightProbe));
        Register("class LightProbeData", typeof(LightProbeData));
        Register("class LightType", typeof(LightType));
        Register("class LinkedBallTwistJointKey", typeof(LinkedBallTwistJointKey));
        Register("class LinkedList<class Scene::AgentInfo,0>", typeof(LinkedList<Scene.AgentInfo>));
        Register("class LipSync", typeof(LipSync));
        Register("class LipSync2", typeof(LipSync2));
        Register("class List<bool>", typeof(List<bool>));
        Register("class List<class Color>", typeof(List<Color>));
        Register("class List<class DCArray<class String> >", typeof(List<List<String>>));
        Register("class List<class Handle<class AnimOrChore> >", typeof(List<Handle<AnimOrChore>>));
        Register("class List<class Handle<class AudioData> >", typeof(List<Handle<AudioData>>));
        Register("class List<class Handle<class Chore> >", typeof(List<Handle<Chore>>));
        Register("class List<class Handle<class D3DMesh> >", typeof(List<Handle<D3DMesh>>));
        Register("class List<class Handle<class PropertySet> >", typeof(List<Handle<PropertySet>>));
        Register("class List<class Handle<class Rules> >", typeof(List<Handle<Rules>>));
        Register("class List<class Handle<class Scene> >", typeof(List<Handle<Scene>>));
        Register("class List<class Handle<class SoundData> >", typeof(List<Handle<SoundData>>));
        Register("class List<class Handle<class T3Texture> >", typeof(List<Handle<T3Texture>>));
        Register("class List<class HandleLock<class Scene> >", typeof(List<Handle<Scene>>));
        Register("class List<class List<class PropertySet> >", typeof(List<List<PropertySet>>));
        Register("class List<class List<class Symbol> >", typeof(List<List<Symbol>>));
        Register("class List<class Map<class String,class String,struct std::less<class String> > >", typeof(List<Dictionary<string, string>>));
        Register("class List<class PropertySet>", typeof(List<PropertySet>));
        Register("class List<class String>", typeof(List<String>));
        Register("class List<class T3ToonGradientRegion>", typeof(List<T3ToonGradientRegion>));
        Register("class List<class Vector3>", typeof(List<Vector3>));
        Register("class List<float>", typeof(List<float>));
        Register("class List<int>", typeof(List<int>));
        Register("class List<struct ActingPaletteGroup::ActingPaletteTransition>", typeof(List<ActingPaletteGroup.ActingPaletteTransition>));
        Register("class List<unsigned int>", typeof(List<uint>));
        Register("class Localization::Language", typeof(Localization.Language));
        Register("class LocalizationRegistry", typeof(LocalizationRegistry));
        Register("class LocalizeInfo", typeof(LocalizeInfo));
        Register("class LocationInfo", typeof(LocationInfo));
        Register("class LocomotionDB", typeof(LocomotionDb));
        Register("class LogicGroup", typeof(LogicGroup));
        Register("class LogicGroup::LogicItem", typeof(LogicGroup.LogicItem));
        Register("class Map<class DlgObjID,int,class DlgObjIDLess>", typeof(Dictionary<DlgObjId, int>));
        Register("class Map<class FontTool::EnumLanguageSet, class DCArray<FontConfig>, std::less<FontTool::EnumLanguageSet>>", typeof(Dictionary<FontTool.EnumLanguageSet, List<FontConfig>>));
        Register("class Map<class Handle<class StyleGuide>,class Handle<class StyleGuide>,struct std::less<class Handle<class StyleGuide> > >", typeof(Dictionary<Handle<StyleGuide>, Handle<StyleGuide>>));
        Register("class Map<class MetaClassDescription const * __ptr64,int,struct std::less<class MetaClassDescription const * __ptr64> >", typeof(NotImplementedException));
        Register("class Map<class String, class TransitionMap::TransitionMapInfo, std::less<class String>>", typeof(Dictionary<string, TransitionMap.TransitionMapInfo>));
        Register("class Map<class String,bool,struct std::less<class String> >", typeof(Dictionary<string, bool>));
        Register("class Map<class String,class AgentMap::AgentMapEntry,struct std::less<class String> >", typeof(Dictionary<String, AgentMap.AgentMapEntry>));
        Register("class Map<class String,class AnimOrChore,struct std::less<class String> >", typeof(Dictionary<string, AnimOrChore>));
        Register("class Map<class String,class DCArray<class String>,struct std::less<class String> >", typeof(Dictionary<string, List<String>>));
        Register("class Map<class String,class DCArray<unsigned char>,struct std::less<class String> >", typeof(Dictionary<string, List<byte>>));
        Register("class Map<class String,class Handle<class PropertySet>,struct std::less<class String> >", typeof(Dictionary<string, Handle<PropertySet>>));
        Register("class Map<class String,class LocomotionDB::AnimationInfo,struct std::less<class String> >", typeof(Dictionary<string, LocomotionDb.AnimationInfo>));
        Register("class Map<class String,class LogicGroup::LogicItem,struct std::less<class String> >", typeof(Dictionary<string, LogicGroup.LogicItem>));
        Register("class Map<class String,class Map<class String,class DCArray<class String>,struct std::less<class String> >,struct std::less<class String> >", typeof(Dictionary<string, Dictionary<string, List<String>>>));
        Register("class Map<class String,class Map<class String,class String,struct std::less<class String> >,struct std::less<class String> >", typeof(Dictionary<string, Dictionary<string, String>>));
        Register("class Map<class String,class PropertySet,struct std::less<class String> >", typeof(Dictionary<string, PropertySet>));
        Register("class Map<class String,class Ptr<class JiraRecord>,struct std::less<class String> >", typeof(Dictionary<string, JiraRecord>));
        Register("class Map<class String,class Rule * __ptr64,struct std::less<class String> >", typeof(Dictionary<string, Rule>));
        Register("class Map<class String,class Set<class String,struct std::less<class String> >,struct std::less<class String> >", typeof(Dictionary<string, HashSet<string>>));
        Register("class Map<class String,class Set<class Symbol,struct std::less<class Symbol> >,struct StringCompareCaseInsensitive>", typeof(Dictionary<string, HashSet<Symbol>>));
        Register("class Map<class String,class Set<class Symbol,struct std::less<class Symbol> >,struct std::less<class String> >", typeof(Dictionary<string, HashSet<Symbol>>));
        Register("class Map<class String,class SoundBusSystem::BusDescription,struct std::less<class String> >", typeof(Dictionary<String, SoundBusSystem.BusDescription>));
        Register("class Map<class String,class String,struct std::less<class String> >", typeof(Dictionary<string, string>));
        Register("class Map<class String,class StyleGuideRef,struct std::less<class String> >", typeof(Dictionary<String, StyleGuideRef>));
        Register("class Map<class String,class Vector3,struct std::less<class String> >", typeof(Dictionary<String, Vector3>));
        Register("class Map<class String,float,struct std::less<class String> >", typeof(Dictionary<string, float>));
        Register("class Map<class String,int,struct std::less<class String> >", typeof(Dictionary<String, int>));
        Register("class Map<class String,struct ChorecorderParameters,struct std::less<class String> >", typeof(Dictionary<String, ChorecorderParameters>));
        Register("class Map<class String,struct ClipResourceFilter,struct StringCompareCaseInsensitive>", typeof(Dictionary<String, ClipResourceFilter>));
        Register("class Map<class String,struct PhonemeTable::PhonemeEntry,struct std::less<class String> >", typeof(Dictionary<String, PhonemeTable.PhonemeEntry>));
        Register("class Map<class Symbol,bool,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, bool>));
        Register("class Map<class Symbol,class DCArray<class LanguageResLocal>,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, List<LanguageResLocal>>));
        Register("class Map<class Symbol,class Handle<class SoundBusSnapshot::Snapshot>,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, Handle<SoundBusSnapshot.Snapshot>>));
        Register("class Map<class Symbol,class Localization::Language,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, Localization.Language>));
        Register("class Map<class Symbol,class Map<class Symbol,class Set<class Symbol,struct std::less<class Symbol> >,struct std::less<class Symbol> >,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, Dictionary<Symbol, HashSet<Symbol>>>));
        Register("class Map<class Symbol,class Map<class Symbol,int,struct std::less<class Symbol> >,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, Dictionary<Symbol, int>>));
        Register("class Map<class Symbol,class PropertySet,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, PropertySet>));
        Register("class Map<class Symbol,class String,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, String>));
        Register("class Map<class Symbol,class Symbol,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, Symbol>));
        Register("class Map<class Symbol,class WalkPath,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, WalkPath>));
        Register("class Map<class Symbol,float,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, float>));
        Register("class Map<class Symbol,int,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, int>));
        Register("class Map<class Symbol,struct FootSteps::FootstepBank,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, FootSteps.FootstepBank>));
        Register("class Map<class Symbol,struct Footsteps2::FootstepBank,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, FootSteps2.FootstepBank>));
        Register("class Map<class Symbol,struct PhonemeTable::PhonemeEntry,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, PhonemeTable.PhonemeEntry>));
        Register("class Map<class Symbol,struct PreloadPackage::ResourceSeenTimes,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, PreloadPackage.ResourceSeenTimes>));
        Register("class Map<class Symbol,struct SoundBankWaveMapEntry,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, SoundBankWaveMapEntry>));
        Register("class Map<class Symbol,struct TransitionMap::TransitionMapInfo,struct std::less<class Symbol> >", typeof(Dictionary<Symbol, TransitionMap.TransitionMapInfo>));
        Register("class Map<int,class DlgLine,struct std::less<int> >", typeof(Dictionary<int, DlgLine>));
        Register("class Map<int,class LanguageResource,struct std::less<int> >", typeof(Dictionary<int, LanguageResource>));
        Register("class Map<int,class PropertySet,struct std::less<int> >", typeof(Dictionary<int, PropertySet>));
        Register("class Map<int,class Ptr<class DialogBranch>,struct std::less<int> >", typeof(Dictionary<int, DialogBranch>));
        Register("class Map<int,class Ptr<class DialogDialog>,struct std::less<int> >", typeof(Dictionary<int, DialogDialog>));
        Register("class Map<int,class Ptr<class DialogExchange>,struct std::less<int> >", typeof(Dictionary<int, DialogExchange>));
        Register("class Map<int,class Ptr<class DialogItem>,struct std::less<int> >", typeof(Dictionary<int, DialogItem>));
        Register("class Map<int,class Ptr<class DialogLine>,struct std::less<int> >", typeof(Dictionary<int, DialogLine>));
        Register("class Map<int,class Ptr<class DialogText>,struct std::less<int> >", typeof(Dictionary<int, DialogText>));
        Register("class Map<int,class String,struct std::less<int> >", typeof(Dictionary<int, String>));
        Register("class Map<int,class Symbol,struct std::less<int> >", typeof(Dictionary<int, Symbol>));
        Register("class Map<int,int,struct std::less<int> >", typeof(Dictionary<int, int>));
        Register("class Map<struct PreloadPackage::ResourceKey,struct PreloadPackage::ResourceSeenTimes,struct std::less<struct PreloadPackage::ResourceKey> >", typeof(Dictionary<PreloadPackage.ResourceKey, PreloadPackage.ResourceSeenTimes>));
        Register("class Map<struct SoundFootsteps::EnumMaterial,class DCArray<class Handle<class SoundData> >,struct std::less<struct SoundFootsteps::EnumMaterial> >", typeof(Dictionary<SoundFootsteps.EnumMaterial, List<Handle<SoundData>>>));
        Register("class Map<struct SoundFootsteps::EnumMaterial,class SoundEventName<0>,struct std::less<struct SoundFootsteps::EnumMaterial> >", typeof(Dictionary<SoundFootsteps.EnumMaterial, SoundEventName>));
        Register("class Map<unsigned int,class LanguageRes,struct std::less<unsigned int> >", typeof(Dictionary<uint, LanguageRes>));
        Register("class Map<unsigned int,class Set<class Symbol,struct std::less<class Symbol> >,struct std::less<unsigned int> >", typeof(Dictionary<uint, HashSet<Symbol>>));
        Register("class Map<unsigned int,struct Font::GlyphInfo,struct std::less<unsigned int> >", typeof(Dictionary<uint, Font.GlyphInfo>));
        Register("class Map<unsigned long,class LanguageRes,struct std::less<unsigned long> >", typeof(Dictionary<uint, LanguageRes>));
        Register("class Map<unsigned long,struct Font::GlyphInfo,struct std::less<unsigned long> >", typeof(Dictionary<uint, Font.GlyphInfo>));
        Register("class Matrix4", typeof(Matrix4));
        Register("class MeshSceneLightmapData::Entry", typeof(MeshSceneLightmapData.Entry));
        Register("class MetaVersionInfo", typeof(MetaVersionInfo));
        Register("class Mover", typeof(Mover));
        Register("class MovieCaptureInfo", typeof(MovieCaptureInfo));
        Register("class NavCam", typeof(NavCam));
        Register("class ParticleAffector", typeof(ParticleAffector));
        Register("class ParticleEmitter", typeof(ParticleEmitter));
        Register("class ParticleInverseKinematics", typeof(ParticleInverseKinematics));
        Register("class ParticlePropConnect", typeof(ParticlePropConnect));
        Register("class ParticleProperties", typeof(ParticleProperties));
        Register("class ParticleProperties::Animation", typeof(ParticleProperties.Animation));
        Register("class ParticleProperties::AnimationParams", typeof(ParticleProperties.AnimationParams));
        Register("class ParticlePropertySamples", typeof(ParticlePropertySamples));
        Register("class ParticleSprite", typeof(ParticleSprite));
        Register("class PathBase", typeof(PathBase));
        Register("class PathMover", typeof(PathMover));
        Register("class PathSegment", typeof(PathSegment));
        Register("class PathTo", typeof(PathTo));
        Register("class PhonemeTable", typeof(PhonemeTable));
        Register("class PhonemeTable::PhonemeEntry", typeof(PhonemeTable.PhonemeEntry));
        Register("class PhysicsData", typeof(PhysicsData));
        Register("class PhysicsObject", typeof(PhysicsObject));
        Register("class PivotJointKey", typeof(PivotJointKey));
        Register("class PlaceableBallTwistJointKey", typeof(PlaceableBallTwistJointKey));
        Register("class PlaybackController", typeof(PlaybackController));
        Register("class PointOfInterestBlocking", typeof(PointOfInterestBlocking));
        Register("class Polar", typeof(Polar));
        Register("class PreloadPackage::RuntimeDataDialog", typeof(PreloadPackage.RuntimeDataDialog));
        Register("class PreloadPackage::RuntimeDataDialog::DlgObjIdAndResourceVector", typeof(PreloadPackage.RuntimeDataDialog.DlgObjIdAndResourceVector));
        Register("class PreloadPackage::RuntimeDataDialog::DlgObjIdAndStartNodeOffset", typeof(PreloadPackage.RuntimeDataDialog.DlgObjIdAndStartNodeOffset));
        Register("class PreloadPackage::RuntimeDataScene", typeof(PreloadPackage.RuntimeDataScene));
        Register("class PreloadPackage::StartNodeOffset", typeof(PreloadPackage.StartNodeOffset));
        Register("class ProceduralEyes", typeof(ProceduralEyes));
        Register("class Procedural_LookAt", typeof(ProceduralLookAt));
        Register("class Procedural_LookAt::Constraint", typeof(ProceduralLookAt.Constraint));
        Register("class Procedural_LookAt_InstanceData", typeof(Procedural_LookAt_InstanceData));
        Register("class Procedural_LookAt_Value", typeof(ProceduralLookAtValue), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class PropertySet", typeof(PropertySet));
        Register("class Ptr<struct PtrBase>", typeof(NotImplementedException), MetaFlags.MetaSerializeDisable);
        Register("class Quaternion", typeof(Quaternion), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class Rect", typeof(Rect), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class RenderObjectInterface", typeof(RenderObjectInterface));
        Register("class RenderObject_Mesh", typeof(RenderObject_Mesh));
        Register("class RenderObject_PostMaterial", typeof(RenderObject_PostMaterial));
        Register("class RenderObject_Text", typeof(RenderObject_Text));
        Register("class RenderObject_Text2", typeof(RenderObject_Text2));
        Register("class ResourceBundle", typeof(ResourceBundle));
        Register("class ResourceGroups", typeof(ResourceGroups));
        Register("class Rollover", typeof(Rollover));
        Register("class RootKey", typeof(RootKey));
        Register("class Rule", typeof(Rule));
        Register("class Rules", typeof(Rules));
        Register("class SArray<class Handle<class T3Texture>,1>", typeof(Handle<T3Texture>[]));
        Register("class SArray<class TRange<float>,3>", typeof(Range<float>[]));
        Register("class SArray<class WalkBoxes::Edge,3>", typeof(WalkBoxes.Edge[]));
        Register("class SArray<float,3>", typeof(float[]));
        Register("class SArray<int,3>", typeof(int[]));
        Register("class SArray<int,4>", typeof(int[]));
        Register("class SArray<unsigned char,32>", typeof(byte[]));
        Register("class SArray<unsigned int,3>", typeof(uint[]));
        Register("class SaveGame", typeof(SaveGame));
        Register("class SaveGame::AgentInfo", typeof(SaveGame.AgentInfo));
        Register("class Scene", typeof(Scene));
        Register("class Scene::AgentInfo", typeof(Scene.AgentInfo));
        Register("class SceneInstData", typeof(SceneInstData));
        Register("class ScriptEnum:TextColorStyle", typeof(ScriptEnum));
        Register("class Selectable", typeof(Selectable));
        Register("class Set<class Color,struct std::less<class Color> >", typeof(HashSet<Color>));
        Register("class Set<class FileName<class SoundEventBankDummy>,struct std::less<class FileName<class SoundEventBankDummy> > >", typeof(HashSet<FileName<SoundEventBankDummy>>));
        Register("class Set<class Ptr<class PlaybackController>,struct std::less<class Ptr<class PlaybackController> > >", typeof(HashSet<PlaybackController>));
        Register("class Set<class String,struct StringCompareCaseInsensitive>", typeof(HashSet<string>));
        Register("class Set<class String,struct std::less<class String> >", typeof(HashSet<string>));
        Register("class Set<class Symbol,struct std::less<class Symbol> >", typeof(HashSet<Symbol>));
        Register("class Set<int,struct std::less<int> >", typeof(HashSet<int>));
        Register("class SingleQuaternionValue", typeof(SingleQuaternionValue));
        Register("class SingleValue<bool>", typeof(SingleValue<bool>));
        Register("class SingleValue<class AnimOrChore>", typeof(SingleValue<AnimOrChore>));
        Register("class SingleValue<class Color>", typeof(SingleValue<Color>));
        Register("class SingleValue<class Handle<class D3DMesh> >", typeof(SingleValue<Handle<D3DMesh>>));
        Register("class SingleValue<class Handle<class Dlg> >", typeof(SingleValue<Handle<Dlg>>));
        Register("class SingleValue<class Handle<class Font> >", typeof(SingleValue<Handle<Font>>));
        Register("class SingleValue<class Handle<class PhonemeTable> >", typeof(SingleValue<Handle<PhonemeTable>>));
        Register("class SingleValue<class Handle<class PropertySet> >", typeof(SingleValue<Handle<PropertySet>>));
        Register("class SingleValue<class Handle<class Scene> >", typeof(SingleValue<Handle<Scene>>));
        Register("class SingleValue<class Handle<class SoundBusSnapshot::Snapshot> >", typeof(SingleValue<Handle<SoundBusSnapshot.Snapshot>>));
        Register("class SingleValue<class Handle<class SoundBusSnapshot::SnapshotSuite> >", typeof(SingleValue<Handle<SoundBusSnapshot.SnapshotSuite>>));
        Register("class SingleValue<class Handle<class SoundData> >", typeof(SingleValue<Handle<SoundData>>));
        Register("class SingleValue<class Handle<class SoundEventData> >", typeof(SingleValue<Handle<SoundEventData>>));
        Register("class SingleValue<class Handle<class SoundEventSnapshotData> >", typeof(SingleValue<Handle<SoundEventSnapshotData>>));
        Register("class SingleValue<class Handle<class SoundReverbDefinition> >", typeof(SingleValue<Handle<SoundReverbDefinition>>));
        Register("class SingleValue<class Handle<class T3Texture> >", typeof(SingleValue<Handle<T3Texture>>));
        Register("class SingleValue<class Handle<class WalkBoxes> >", typeof(SingleValue<Handle<WalkBoxes>>));
        Register("class SingleValue<class Handle<struct SoundAmbience::AmbienceDefinition> >", typeof(SingleValue<Handle<SoundAmbience.AmbienceDefinition>>));
        Register("class SingleValue<class LocationInfo>", typeof(SingleValue<LocationInfo>));
        Register("class SingleValue<class Polar>", typeof(SingleValue<Polar>));
        Register("class SingleValue<class Quaternion>", typeof(SingleValue<Quaternion>));
        Register("class SingleValue<class SoundEventName<0> >", typeof(SingleValue<SoundEventName>));
        Register("class SingleValue<class SoundEventName<1> >", typeof(SingleValue<SoundEventName>));
        Register("class SingleValue<class SoundEventName<2> >", typeof(SingleValue<SoundEventName>));
        Register("class SingleValue<class String>", typeof(SingleValue<String>));
        Register("class SingleValue<class Symbol>", typeof(SingleValue<Symbol>));
        Register("class SingleValue<class T3VertexBufferSample<class T3NormalSampleData,class T3HeapAllocator> >", typeof(SingleValue<T3VertexBufferSample<T3NormalSampleData, T3HeapAllocator>>));
        Register("class SingleValue<class T3VertexBufferSample<class T3PositionSampleData,class T3HeapAllocator> >", typeof(SingleValue<T3VertexBufferSample<T3PositionSampleData, T3HeapAllocator>>));
        Register("class SingleValue<class Transform>", typeof(SingleValue<Transform>));
        Register("class SingleValue<class Vector2>", typeof(SingleValue<Vector2>));
        Register("class SingleValue<class Vector3>", typeof(SingleValue<Vector3>));
        Register("class SingleValue<class Vector4>", typeof(SingleValue<Vector4>));
        Register("class SingleValue<float>", typeof(SingleValue<float>));
        Register("class SingleValue<int>", typeof(SingleValue<int>));
        Register("class SingleValue<struct CompressedPathBlockingValue::CompressedPathInfoKey>", typeof(SingleValue<CompressedPathBlockingValue.CompressedPathInfoKey>));
        Register("class SingleValue<struct PhonemeKey>", typeof(SingleValue<PhonemeKey>));
        Register("class SingleValue<struct ScriptEnum>", typeof(SingleValue<ScriptEnum>));
        Register("class SingleValue<unsigned __int64>", typeof(SingleValue<ulong>));
        Register("class SingleVector3Value", typeof(SingleVector3Value));
        Register("class Skeleton", typeof(Skeleton));
        Register("class Skeleton::Entry", typeof(Skeleton.Entry));
        Register("class SkeletonInstance", typeof(SkeletonInstance));
        Register("class SkeletonPose", typeof(SkeletonPose));
        Register("class SkeletonPoseValue", typeof(SkeletonPoseValue));
        Register("class SkeletonPoseValueContext", typeof(SkeletonPoseValueContext));
        Register("class SoundAmbienceInterface", typeof(SoundAmbienceInterface));
        Register("class SoundBankWaveMap", typeof(SoundBankWaveMap));
        Register("class SoundBankWaveMapEntry", typeof(SoundBankWaveMapEntry));
        Register("class SoundBusSnapshot::Snapshot", typeof(SoundBusSnapshot.Snapshot));
        Register("class SoundBusSnapshot::SnapshotSuite", typeof(SoundBusSnapshot.SnapshotSuite));
        Register("class SoundBusSystem::BusDescription", typeof(SoundBusSystem.BusDescription));
        Register("class SoundBusSystem::BusHolder", typeof(SoundBusSystem.BusHolder));
        Register("class SoundData", typeof(SoundData));
        Register("class SoundEventBankDummy", typeof(SoundEventBankDummy));
        Register("class SoundEventBankMap", typeof(SoundEventBankMap));
        Register("class SoundEventData", typeof(SoundEventData));
        Register("class SoundEventEmitterInstance", typeof(SoundEventEmitterInstance));
        Register("class SoundEventName<0>", typeof(SoundEventName));
        Register("class SoundEventName<1>", typeof(SoundEventName));
        Register("class SoundEventName<2>", typeof(SoundEventName));
        Register("class SoundEventNameBase", typeof(SoundEventNameBase));
        Register("class SoundEventPreloadInterface", typeof(SoundEventPreloadInterface));
        Register("class SoundEventSnapshotData", typeof(SoundEventSnapshotData));
        Register("class SoundFootsteps::EnumMaterial", typeof(SoundFootsteps.EnumMaterial));
        Register("class SoundListenerInterface", typeof(SoundListenerInterface));
        Register("class SoundMusicInterface", typeof(SoundMusicInterface));
        Register("class SoundReverbDefinition", typeof(SoundReverbDefinition));
        Register("class SoundReverbInterface", typeof(SoundReverbInterface));
        Register("class SoundReverbPreset", typeof(SoundReverbPreset));
        Register("class SoundSfxInterface", typeof(SoundSfxInterface));
        Register("class SoundSnapshotInstance", typeof(SoundSnapshotInstance));
        Register("class Sphere", typeof(Sphere));
        Register("class String", typeof(string));
        Register("class StringFilter", typeof(StringFilter));
        Register("class Style", typeof(Style));
        Register("class StyleGuide", typeof(StyleGuide));
        Register("class StyleGuideMapper ", typeof(StyleGuideMapper));
        Register("class StyleGuideRef", typeof(StyleGuideRef));
        Register("class StyleIdleManager::FadeData", typeof(StyleIdleManager.FadeData));
        Register("class StyleIdleTransitionsRes", typeof(StyleIdleTransitionsRes));
        Register("class StyleIdleTransitionsResInst", typeof(StyleIdleTransitionsResInst));
        Register("class Subtitle", typeof(Subtitle));
        Register("class Symbol", typeof(Symbol), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class T3IndexBuffer", typeof(T3IndexBuffer));
        Register("class T3LightSceneInternalData::LightmapPage", typeof(T3LightSceneInternalData.LightmapPage));
        Register("class T3MeshMaterialOverride", typeof(T3MeshMaterialOverride));
        Register("class T3MeshTexCoordTransform", typeof(T3MeshTexCoordTransform));
        Register("class T3OverlayData", typeof(T3OverlayData));
        Register("class T3RenderStateBlock", typeof(T3RenderStateBlock));
        Register("class T3SamplerState", typeof(T3SamplerStateBlock));
        Register("class T3SamplerStateBlock", typeof(T3SamplerStateBlock));
        Register("class T3Texture", typeof(T3Texture));
        Register("class T3Texture::AuxiliaryData", typeof(T3Texture.AuxiliaryData));
        Register("class T3Texture::RegionStreamHeader", typeof(T3Texture.RegionStreamHeader));
        Register("class T3ToonGradientRegion", typeof(T3ToonGradientRegion));
        Register("class T3VertexBuffer", typeof(T3VertexBuffer));
        Register("class T3VertexBufferSample<class T3NormalSampleData,class T3HeapAllocator>", typeof(T3VertexBufferSample<T3NormalSampleData, T3HeapAllocator>));
        Register("class T3VertexBufferSample<class T3PositionSampleData,class T3HeapAllocator>", typeof(T3VertexBufferSample<T3PositionSampleData, T3HeapAllocator>));
        Register("class TRange<float>", typeof(Range<float>), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class TRect<float>", typeof(Rect<float>), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class TaskOwner", typeof(TaskOwner));
        Register("class TextAlignmentType", typeof(TextAlignmentType));
        Register("class ToolProps", typeof(ToolProps),MetaFlags.MetaSerializeNonBlockedVariableSize);
        Register("class Transform", typeof(Transform));
        Register("class TransitionMap", typeof(TransitionMap));
        Register("class TransitionMap::TransitionMapInfo", typeof(TransitionMap.TransitionMapInfo));
        Register("class TransitionRemapper", typeof(TransitionRemapper));
        Register("class Trigger", typeof(Trigger));
        Register("class UID::Generator", typeof(Generator));
        Register("class UID::Owner", typeof(Owner));
        Register("class Vector2", typeof(Vector2), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class Vector3", typeof(Vector3), MetaFlags.MetaSerializeBlockingDisabled);
        Register("class Vector4", typeof(Vector4),MetaFlags.MetaSerializeBlockingDisabled);
        Register("class Vers", typeof(Vers));
        Register("class VfxGroup", typeof(VfxGroup));
        Register("class VoiceData", typeof(VoiceData));
        Register("class VoiceSpeaker", typeof(VoiceSpeaker));
        Register("class WalkAnimator", typeof(WalkAnimator));
        Register("class WalkBoxes", typeof(WalkBoxes));
        Register("class WalkBoxes::Edge", typeof(WalkBoxes.Edge));
        Register("class WalkBoxes::Quad", typeof(WalkBoxes.Quad));
        Register("class WalkBoxes::Tri", typeof(WalkBoxes.Tri));
        Register("class WalkBoxes::Vert", typeof(WalkBoxes.Vert));
        Register("class WalkPath", typeof(WalkPath));
        Register("class WeakPtr<Agent>", typeof(Agent)); // HMM?
        Register("class WorkingMesh", typeof(WorkingMesh));
        Register("class WorkingMesh::BonePalette ", typeof(WorkingMesh.BonePalette));
        Register("class WorkingMesh::Mesh", typeof(WorkingMesh.Mesh));
        Register("class WorkingMesh::Shader", typeof(WorkingMesh.Shader));
        Register("class WorkingMesh::Triangle", typeof(WorkingMesh.Triangle));
        Register("class WorkingMesh::Vertex ", typeof(WorkingMesh.Vertex));
        Register("class ZTestFunction", typeof(ZTestFunction));
        Register("class class Footsteps2", typeof(FootSteps2));
        Register("double", typeof(double),MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum ActingPalette::ActiveDuring", typeof(ActingPalette.ActiveDuring), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum AudioSound::SoundMode", typeof(AudioSound.SoundMode), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum ChoreResource::AAStatus", typeof(ChoreResource.AutoActStatus), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum DialogItem::PlaybackMode", typeof(DialogItem.PlaybackModeEnum), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum DialogUtils::DialogElemT", typeof(DialogUtils.DialogElemT), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum InputCode", typeof(InputCode), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum InputMapper::EventType", typeof(InputMapper.EventType), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum MovieCaptureInfo::CompressorType", typeof(MovieCaptureInfo.CompressorType), MetaFlags.MetaSerializeBlockingDisabled);
        Register("enum NavCam::Mode", typeof(Mode), MetaFlags.MetaSerializeBlockingDisabled);
        Register("float", typeof(float), MetaFlags.MetaSerializeBlockingDisabled);
        Register("int", typeof(int), MetaFlags.MetaSerializeBlockingDisabled);
        Register("int16", typeof(short),MetaFlags.MetaSerializeBlockingDisabled);
        Register("int32", typeof(int), MetaFlags.MetaSerializeBlockingDisabled);
        Register("int64", typeof(long),MetaFlags.MetaSerializeBlockingDisabled);
        Register("int8", typeof(sbyte),MetaFlags.MetaSerializeBlockingDisabled);
        Register("long", typeof(int),MetaFlags.MetaSerializeBlockingDisabled);
        Register("short", typeof(short),MetaFlags.MetaSerializeBlockingDisabled);
        Register("struct ActingPalette::EnumActiveDuring", typeof(ActingPalette.EnumActiveDuring));
        Register("struct ActingPaletteGroup::EnumIdleTransition", typeof(ActingPaletteGroup.EnumIdleTransition));
        Register("struct AudioData::Streamed", typeof(AudioData.Streamed));
        Register("struct AudioSound::EnumSoundMode", typeof(AudioSound.EnumSoundMode));
        Register("struct BlendGraph::EnumBlendGraphType", typeof(BlendGraph.EnumBlendGraphType));
        Register("struct CSPK2Context", typeof(CSPK2Context));
        Register("struct Chore::EnumExtentsMode", typeof(Chore.EnumExtentsMode));
        Register("struct ChorecorderParameters", typeof(ChorecorderParameters));
        Register("struct ClipResourceFilter", typeof(ClipResourceFilter));
        Register("struct CompressedPathBlockingValue::CompressedPathInfoKey", typeof(CompressedPathBlockingValue.CompressedPathInfoKey));
        Register("struct D3DMesh::PaletteEntry", typeof(D3DMesh.PaletteEntry));
        Register("struct DialogItem::EnumPlaybackMode", typeof(DialogItem.EnumPlaybackMode));
        Register("struct DlgNodeCriteria::EnumDefaultResultT", typeof(DlgNodeCriteria.EnumDefaultResultT));
        Register("struct DlgNodeCriteria::EnumTestT", typeof(DlgNodeCriteria.EnumTestT));
        Register("struct DlgNodeCriteria::EnumThresholdT", typeof(DlgNodeCriteria.EnumThresholdT));
        Register("struct DlgNodeExchange::Entry", typeof(DlgNodeExchange.Entry));
        Register("struct DlgNodeInstanceParallel::ElemInstanceData", typeof(NotImplementedException));
        Register("struct DlgNodeInstanceSequence::ElemInstanceData", typeof(NotImplementedException));
        Register("struct EnlightenModule::EnlightenAdaptiveProbeVolumeSettings", typeof(EnlightenModule.EnlightenAdaptiveProbeVolumeSettings));
        Register("struct EnlightenModule::EnlightenAutoProbeVolumeSettings", typeof(EnlightenModule.EnlightenAutoProbeVolumeSettings));
        Register("struct EnlightenModule::EnlightenCubemapSettings", typeof(EnlightenModule.EnlightenCubemapSettings));
        Register("struct EnlightenModule::EnlightenLightSettings", typeof(EnlightenModule.EnlightenLightSettings));
        Register("struct EnlightenModule::EnlightenMeshSettings", typeof(EnlightenModule.EnlightenMeshSettings));
        Register("struct EnlightenModule::EnlightenMeshSettings::AutoUVSettings", typeof(EnlightenModule.EnlightenMeshSettings.AutoUVSettings));
        Register("struct EnlightenModule::EnlightenPrimitiveSettings", typeof(EnlightenModule.EnlightenPrimitiveSettings));
        Register("struct EnlightenModule::EnlightenProbeVolumeSettings", typeof(EnlightenModule.EnlightenProbeVolumeSettings));
        Register("struct EnlightenModule::EnlightenSystemSettings", typeof(EnlightenModule.EnlightenSystemSettings));
        Register("struct EnlightenModule::EnumeAgentUsage", typeof(EnlightenModule.EnumeAgentUsage));
        Register("struct EnlightenModule::EnumeAutoUVSimplificationMode", typeof(EnlightenModule.EnumeAutoUVSimplificationMode));
        Register("struct EnlightenModule::EnumeBackfaceType", typeof(EnlightenModule.EnumeBackfaceType));
        Register("struct EnlightenModule::EnumeDisplayQuality", typeof(EnlightenModule.EnumeDisplayQuality));
        Register("struct EnlightenModule::EnumeDistributedBuildSystem", typeof(EnlightenModule.EnumeDistributedBuildSystem));
        Register("struct EnlightenModule::EnumeInstanceType", typeof(EnlightenModule.EnumeInstanceType));
        Register("struct EnlightenModule::EnumeProbeResolution", typeof(EnlightenModule.EnumeProbeResolution));
        Register("struct EnlightenModule::EnumeProbeResolutionWithDefault", typeof(EnlightenModule.EnumeProbeResolutionWithDefault));
        Register("struct EnlightenModule::EnumeProbeSampleMethod", typeof(EnlightenModule.EnumeProbeSampleMethod));
        Register("struct EnlightenModule::EnumeQuality", typeof(EnlightenModule.EnumeQuality));
        Register("struct EnlightenModule::EnumeQualityWithDefault", typeof(EnlightenModule.EnumeQualityWithDefault));
        Register("struct EnlightenModule::EnumeRadiositySampleRate", typeof(EnlightenModule.EnumeRadiositySampleRate));
        Register("struct EnlightenModule::EnumeSceneOptimisationMode", typeof(EnlightenModule.EnumeSceneOptimisationMode));
        Register("struct EnlightenModule::EnumeSimplifyMode", typeof(EnlightenModule.EnumeSimplifyMode));
        Register("struct EnlightenModule::EnumeUpdateMethod", typeof(EnlightenModule.EnumeUpdateMethod));
        Register("struct EnlightenModule::EnumeUpdateMethodWithDefault", typeof(EnlightenModule.EnumeUpdateMethodWithDefault));
        Register("struct EnumBase", typeof(EnumBase), MetaFlags.MetaSerializeDisable);
        Register("struct EnumBokehOcclusionType", typeof(EnumBokehOcclusionType));
        Register("struct EnumBokehQualityLevel", typeof(EnumBokehQualityLevel));
        Register("struct EnumDOFQualityLevel", typeof(EnumDOFQualityLevel));
        Register("struct EnumDepthOfFieldType", typeof(EnumDepthOfFieldType));
        Register("struct EnumEmitterBoneSelection", typeof(EnumEmitterBoneSelection));
        Register("struct EnumEmitterColorType", typeof(EnumEmitterColorType));
        Register("struct EnumEmitterConstraintType", typeof(EnumEmitterConstraintType));
        Register("struct EnumEmitterParticleCountType", typeof(EnumEmitterParticleCountType));
        Register("struct EnumEmitterSpawnShape", typeof(EnumEmitterSpawnShape));
        Register("struct EnumEmitterSpriteAnimationSelection", typeof(EnumEmitterSpriteAnimationSelection));
        Register("struct EnumEmitterSpriteAnimationType", typeof(EnumEmitterSpriteAnimationType));
        Register("struct EnumEmitterTriggerEnable", typeof(EnumEmitterTriggerEnable));
        Register("struct EnumEmittersEnableType", typeof(EnumEmittersEnableType));
        Register("struct EnumGlowQualityLevel", typeof(EnumGlowQualityLevel));
        Register("struct EnumHBAOBlurQuality", typeof(EnumHBAOBlurQuality));
        Register("struct EnumHBAODeinterleaving", typeof(EnumHBAODeinterleaving));
        Register("struct EnumHBAOParticipationType", typeof(EnumHBAOParticipationType));
        Register("struct EnumHBAOPerPixelNormals", typeof(EnumHBAOPerPixelNormals));
        Register("struct EnumHBAOPreset", typeof(EnumHBAOPreset));
        Register("struct EnumHBAOQualityLevel", typeof(EnumHBAOQualityLevel));
        Register("struct EnumHBAOResolution", typeof(EnumHBAOResolution));
        Register("struct EnumHTextAlignmentType", typeof(EnumHTextAlignmentType));
        Register("struct EnumLightCellBlendMode", typeof(EnumLightCellBlendMode));
        Register("struct EnumMeshDebugRenderType", typeof(EnumMeshDebugRenderType));
        Register("struct EnumParticleAffectorType", typeof(EnumParticleAffectorType));
        Register("struct EnumParticleGeometryType", typeof(EnumParticleGeometryType));
        Register("struct EnumParticlePropDriver", typeof(EnumParticlePropDriver));
        Register("struct EnumParticlePropModifier", typeof(EnumParticlePropModifier));
        Register("struct EnumParticleSortMode", typeof(EnumParticleSortMode));
        Register("struct EnumPlatformType", typeof(EnumPlatformType));
        Register("struct EnumRenderAntialiasType", typeof(EnumRenderAntialiasType));
        Register("struct EnumRenderLightmapUVGenerationType", typeof(EnumRenderLightmapUVGenerationType));
        Register("struct EnumRenderMaskTest", typeof(EnumRenderMaskTest));
        Register("struct EnumRenderMaskWrite", typeof(EnumRenderMaskWrite));
        Register("struct EnumRenderTAAJitterType", typeof(EnumRenderTAAJitterType));
        Register("struct EnumRenderTextureResolution", typeof(EnumRenderTextureResolution));
        Register("struct EnumT3DetailShadingType", typeof(EnumT3DetailShadingType));
        Register("struct EnumT3LightEnvBakeOnStatic", typeof(EnumT3LightEnvBakeOnStatic));
        Register("struct EnumT3LightEnvEnlightenBakeBehavior", typeof(EnumT3LightEnvEnlightenBakeBehavior));
        Register("struct EnumT3LightEnvGroup", typeof(EnumT3LightEnvGroup));
        Register("struct EnumT3LightEnvLODBehavior", typeof(EnumT3LightEnvLodBehavior));
        Register("struct EnumT3LightEnvMobility", typeof(EnumT3LightEnvMobility));
        Register("struct EnumT3LightEnvShadowQuality", typeof(EnumT3LightEnvShadowQuality));
        Register("struct EnumT3LightEnvShadowType", typeof(EnumT3LightEnvShadowType));
        Register("struct EnumT3LightEnvType", typeof(EnumT3LightEnvType));
        Register("struct EnumT3MaterialLODFullyRough", typeof(EnumT3MaterialLODFullyRough));
        Register("struct EnumT3MaterialLightModelType", typeof(EnumT3MaterialLightModelType));
        Register("struct EnumT3MaterialNormalSpaceType", typeof(EnumT3MaterialNormalSpaceType));
        Register("struct EnumT3MaterialSwizzleType", typeof(EnumT3MaterialSwizzleType));
        Register("struct EnumT3NPRSpecularType", typeof(EnumT3NPRSpecularType));
        Register("struct EnumTextOrientationType", typeof(EnumTextOrientationType));
        Register("struct EnumTonemapType", typeof(EnumTonemapType));
        Register("struct EnumVTextAlignmentType", typeof(EnumVTextAlignmentType));
        Register("struct EnumeTangentModes", typeof(EnumeTangentModes));
        Register("struct FlagsT3LightEnvGroupSet", typeof(FlagsT3LightEnvGroupSet));
        Register("struct Font::GlyphInfo", typeof(Font.GlyphInfo));
        Register("struct FootSteps::FootstepBank", typeof(FootSteps.FootstepBank));
        Register("struct Footsteps2::FootstepBank", typeof(FootSteps2.FootstepBank));
        Register("struct GFXPlatformAttributeParams", typeof(GFXPlatformAttributeParams));
        Register("struct MeshSceneEnlightenData", typeof(MeshSceneEnlightenData));
        Register("struct MeshSceneLightmapData", typeof(MeshSceneLightmapData));
        Register("struct Meta::AgentResourceContext", typeof(NotImplementedException));
        Register("struct Meta::DependentResource", typeof(NotImplementedException));
        Register("struct MovieCaptureInfo::EnumCompressorType", typeof(MovieCaptureInfo.EnumCompressorType));
        Register("struct NavCam::EnumMode", typeof(EnumMode));
        Register("struct ParticleLODKey", typeof(ParticleLODKey));
        Register("struct ParticleSprite::Animation", typeof(ParticleSprite.Animation));
        Register("struct PerAgentClipResourceFilter", typeof(PerAgentClipResourceFilter));
        Register("struct PhonemeKey", typeof(PhonemeKey));
        Register("struct PhysicsObject::EnumePhysicsBoundingVolumeType", typeof(PhysicsObject.EnumePhysicsBoundingVolumeType));
        Register("struct PhysicsObject::EnumePhysicsCollisionType", typeof(PhysicsObject.EnumePhysicsCollisionType));
        Register("struct PreloadPackage::ResourceKey", typeof(PreloadPackage.ResourceKey));
        Register("struct PreloadPackage::ResourceSeenTimes", typeof(PreloadPackage.ResourceSeenTimes));
        Register("struct PreloadPackage::RuntimeDataDialog::DialogResourceInfo", typeof(PreloadPackage.RuntimeDataDialog.DialogResourceInfo));
        Register("struct Procedural_LookAt::EnumLookAtComputeStage", typeof(ProceduralLookAt.EnumLookAtComputeStage));
        Register("struct PtrBase", typeof(NotImplementedException));
        Register("struct RecordingUtils::EnumRecordingStatus", typeof(RecordingUtils.EnumRecordingStatus));
        Register("struct RenderSwizzleParams", typeof(RenderSwizzleParams));
        Register("struct ResourceGroupInfo", typeof(ResourceGroupInfo));
        Register("struct Rule::AgentInfo", typeof(Rule.AgentInfo));
        Register("struct Scene::AgentQualitySettings", typeof(Scene.AgentQualitySettings));
        Register("struct ScriptEnum", typeof(ScriptEnum));
        Register("struct SkeletonPoseValue::BoneEntry", typeof(SkeletonPoseValue.BoneEntry));
        Register("struct SkeletonPoseValue::Sample", typeof(SkeletonPoseValue.Sample));
        Register("struct SklNodeData", typeof(SklNodeData));
        Register("struct SoundAmbience::AmbienceDefinition", typeof(SoundAmbience.AmbienceDefinition));
        Register("struct SoundSystem::Implementation::ChannelHolder", typeof(SoundSystem.Implementation.ChannelHolder));
        Register("struct T3GFXBuffer", typeof(T3GFXBuffer));
        Register("struct T3GFXVertexState", typeof(T3GFXVertexState));
        Register("struct T3LightCinematicRigLOD", typeof(T3LightCinematicRigLOD));
        Register("struct T3LightEnvInternalData", typeof(T3LightEnvInternalData));
        Register("struct T3LightEnvInternalData::QualityEntry", typeof(T3LightEnvInternalData.QualityEntry));
        Register("struct T3LightEnvLOD", typeof(T3LightEnvLOD));
        Register("struct T3LightProbeInternalData", typeof(T3LightProbeInternalData));
        Register("struct T3LightProbeInternalData::QualityEntry", typeof(T3LightProbeInternalData.QualityEntry));
        Register("struct T3LightSceneInternalData", typeof(T3LightSceneInternalData));
        Register("struct T3LightSceneInternalData::QualityEntry", typeof(T3LightSceneInternalData.QualityEntry));
        Register("struct T3MaterialCompiledData", typeof(T3MaterialCompiledData));
        Register("struct T3MaterialData", typeof(T3MaterialData));
        Register("struct T3MaterialEnlightenPrecomputeParams", typeof(T3MaterialEnlightenPrecomputeParams));
        Register("struct T3MaterialParameter", typeof(T3MaterialParameter));
        Register("struct T3MaterialPassData", typeof(T3MaterialPassData));
        Register("struct T3MaterialPreShader", typeof(T3MaterialPreShader));
        Register("struct T3MaterialRequirements", typeof(T3MaterialRequirements));
        Register("struct T3MaterialRuntimeProperty", typeof(T3MaterialRuntimeProperty));
        Register("struct T3MaterialStaticParameter", typeof(T3MaterialStaticParameter));
        Register("struct T3MaterialSwizzleParams", typeof(T3MaterialSwizzleParams));
        Register("struct T3MaterialTexture", typeof(T3MaterialTexture));
        Register("struct T3MaterialTransform2D", typeof(T3MaterialTransform2D));
        Register("struct T3MeshBatch", typeof(T3MeshBatch));
        Register("struct T3MeshBoneEntry", typeof(T3MeshBoneEntry));
        Register("struct T3MeshData", typeof(T3MeshData));
        Register("struct T3MeshEffectPreload", typeof(T3MeshEffectPreload));
        Register("struct T3MeshEffectPreloadDynamicFeatures", typeof(T3MeshEffectPreloadDynamicFeatures));
        Register("struct T3MeshEffectPreloadEntry", typeof(T3MeshEffectPreloadEntry));
        Register("struct T3MeshLOD", typeof(T3MeshLOD));
        Register("struct T3MeshLocalTransformEntry", typeof(T3MeshLocalTransformEntry));
        Register("struct T3MeshMaterial", typeof(T3MeshMaterial));
        Register("struct T3MeshPropertyEntry", typeof(T3MeshPropertyEntry));
        Register("struct T3MeshTextureIndices", typeof(T3MeshTextureIndices));
        Register("struct T3OcclusionMeshData", typeof(T3OcclusionMeshData));
        Register("struct T3OverlayObjectData_Sprite", typeof(T3OverlayObjectDataSprite));
        Register("struct T3OverlayParams", typeof(T3OverlayParams));
        Register("struct T3OverlaySpriteParams", typeof(T3OverlaySpriteParams));
        Register("struct T3Texture::StreamHeader", typeof(T3Texture.StreamHeader));
        Register("struct TetrahedralMeshData", typeof(TetrahedralMeshData));
        Register("uint16", typeof(ushort),MetaFlags.MetaSerializeBlockingDisabled);
        Register("uint32", typeof(uint), MetaFlags.MetaSerializeBlockingDisabled);
        Register("uint64", typeof(ulong),MetaFlags.MetaSerializeBlockingDisabled);
        Register("uint8", typeof(byte),MetaFlags.MetaSerializeBlockingDisabled);
        Register("unsigned __int64", typeof(ulong),MetaFlags.MetaSerializeBlockingDisabled);
        Register("unsigned char", typeof(byte),MetaFlags.MetaSerializeBlockingDisabled);
        Register("unsigned int", typeof(uint), MetaFlags.MetaSerializeBlockingDisabled);
        Register("unsigned long", typeof(uint), MetaFlags.MetaSerializeBlockingDisabled);
        Register("unsigned short", typeof(ushort),MetaFlags.MetaSerializeBlockingDisabled);
        // @formatter:on
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Register(string typeName, Type linkingType, MetaFlags flags = MetaFlags.None)
    {
        Register(new MetaClassType(typeName, linkingType, flags));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Register(MetaClassType info)
    {
        ByName.Add(info.Symbol.SymbolName, info);
        ByHash.Add(info.Symbol.Crc64, info);
    }

    public static MetaClassType GetByName(string typeName)
    {
        ByName.TryGetValue(typeName, out MetaClassType? info);
        return info ?? throw new KeyNotFoundException($"Unregistered class {typeName}");
    }

    public static MetaClassType? GetByHash(ulong hash) =>
        ByHash.TryGetValue(hash, out MetaClassType? info) ? info : null;

    public static void PrintRegisteredTypes()
    {
        foreach (MetaClassType type in ByHash.Values)
        {
            Console.WriteLine(($"{type.Symbol.SymbolName} {type.Symbol.Crc64:X}"));
        }
    }
}

// TODO: I know this looks bad. Unfortunately, there are more than a thousand classes. I physically cannot add them all in one go.
// Over time, as soon as more classes and their serializers are implemented, this list will get shrunken.
// Some of these are completely useless and they are only related to the Telltale Tool.
// Requires further testing.

public class NavCam
{
}

public enum T3EffectParameterType
{
    // eEffectParameter_
    Unknown = -1,
    FirstUniformBuffer = 0x0,
    UniformBufferCamera = 0x0,
    UniformBufferScene = 0x1,
    UniformBufferSceneTool = 0x2,
    UniformBufferObject = 0x3,
    UniformBufferInstance = 0x4,
    UniformBufferLights = 0x5,
    UniformBufferLightEnv = 0x6,
    UniformBufferLightCin = 0x7,
    UniformBufferSkinning = 0x8,
    UniformBufferShadow = 0x9,
    UniformBufferParticle = 0xA,
    UniformBufferDecal = 0xB,
    UniformBufferSimple = 0xC,
    UniformBufferGaussian = 0xD,
    UniformBufferPost = 0xE,
    UniformBufferBrush = 0xF,
    UniformBufferLightEnvDataHigh = 0x10,
    UniformBufferLightEnvDataMedium = 0x11,
    UniformBufferLightEnvDataLow = 0x12,
    UniformBufferLightEnvView = 0x13,
    UniformBufferLightAmbient = 0x14,
    UniformBufferShadowVolume = 0x15,
    UniformBufferMesh = 0x16,
    UniformBufferMeshBatch = 0x17,
    UniformBufferMeshDynamicBatch = 0x18,
    UniformBufferMeshDebugBatch = 0x19,
    UniformBufferHBAO = 0x1A,
    UniformBufferMaterialTool = 0x1B,
    UniformBufferMaterialBase = 0x1C,
    UniformBufferMaterialMain = 0x1D,
    LastUniformBuffer = 0x1D,
    FirstGenericBuffer = 0x1E,
    GenericBufferSkinning = 0x1E,
    GenericBuffer0VertexIn = 0x1F,
    GenericBuffer1VertexIn = 0x20,
    GenericBufferVertexOut = 0x21,
    GenericBufferMeshBounds = 0x22,
    GenericBufferIndices = 0x23,
    GenericBufferIndirectArgs = 0x24,
    GenericBufferStartInstance = 0x25,
    GenericBufferDepthRange = 0x26,
    GenericBufferPrevDepthRange = 0x27,
    GenericBufferLightGrid = 0x28,
    GenericBufferLightZBin = 0x29,
    GenericBufferLightGroupMask = 0x2A,
    GenericBufferShadowCascades = 0x2B,
    GenericBufferCinShadowData = 0x2C,
    GenericBufferDusterData = 0x2D,
    GenericBufferDusterVisibility = 0x2E,
    GenericBufferWaveformMonitor = 0x2F,
    GenericBufferInput0 = 0x30,
    GenericBufferInput1 = 0x31,
    GenericBufferInput2 = 0x32,
    GenericBufferInput3 = 0x33,
    GenericBufferInput4 = 0x34,
    GenericBufferInput5 = 0x35,
    GenericBufferInput6 = 0x36,
    GenericBufferInput7 = 0x37,
    GenericBufferOutput0 = 0x38,
    GenericBufferOutput1 = 0x39,
    GenericBufferOutput2 = 0x3A,
    GenericBufferOutput3 = 0x3B,
    GenericBufferOutput4 = 0x3C,
    LastGenericBuffer = 0x3C,
    FirstSampler = 0x3D,
    SamplerDiffuse = 0x3D,
    SamplerStaticShadowmap = 0x3E,
    SamplerShadowmap = 0x3F,
    SamplerProjected = 0x40,
    SamplerBrushNear = 0x41,
    SamplerBrushFar = 0x42,
    SamplerEnvironment = 0x43,
    SamplerBokehPattern = 0x44,
    SamplerNoiseLUT = 0x45,
    SamplerHLSMovieY = 0x46,
    SamplerHLSMovieC = 0x47,
    SamplerHLSMovieRGB = 0x48,
    SamplerBackbuffer = 0x49,
    SamplerBackbufferHDR = 0x4A,
    SamplerBackbufferHDRPrev = 0x4B,
    SamplerBackbufferHDRResolved = 0x4C,
    SamplerDepthbuffer = 0x4D,
    SamplerLinearDepth = 0x4E,
    SamplerLinearDepthPrev = 0x4F,
    SamplerAlphaMeshLinearDepth = 0x50,
    SamplerStencil = 0x51,
    SamplerGBuffer0 = 0x52,
    SamplerGBuffer1 = 0x53,
    SamplerDBuffer0 = 0x54,
    SamplerDBuffer1 = 0x55,
    SamplerDeferredShadows = 0x56,
    SamplerDeferredModulatedShadows = 0x57,
    SamplerDeferredShadowsPrev = 0x58,
    SamplerDeferredLight0 = 0x59,
    SamplerDeferredLight1 = 0x5A,
    SamplerDofBlur1x = 0x5B,
    SamplerDofBlur2x = 0x5C,
    SamplerDofBlur3x = 0x5D,
    NewDepthOfFieldHalf = 0x5E,
    NewDofNearH = 0x5F,
    NewDofNearV = 0x60,
    NewDofFarH1x = 0x61,
    NewDofFarV1x = 0x62,
    Bokeh = 0x63,
    SSLines = 0x64,
    SamplerLightEnvShadowGobo = 0x65,
    SamplerStaticShadowVolume = 0x66,
    FirstEVSMShadowSampler = 0x67,
    SamplerEVSMShadow0 = 0x67,
    SamplerEVSMShadow1 = 0x68,
    LastEVSMShadowSampler = 0x68,
    SamplerSMAAAreaLookup = 0x69,
    SamplerSMAASearchLookup = 0x6A,
    SamplerRandom = 0x6B,
    SamplerNoise = 0x6C,
    SamplerTetrahedralLookup = 0x6D,
    SamplerLightmap = 0x6E,
    SamplerLightmapFlat = 0x6F,
    SamplerBoneMatrices = 0x70,
    SamplerDebugOverlay = 0x71,
    SamplerSoftwareOcclusion = 0x72,
    FirstMaterialInputSampler = 0x73,
    SamplerMaterialInput0 = 0x73,
    SamplerMaterialInput1 = 0x74,
    SamplerMaterialInput2 = 0x75,
    SamplerMaterialInput3 = 0x76,
    SamplerMaterialInput4 = 0x77,
    SamplerMaterialInput5 = 0x78,
    SamplerMaterialInput6 = 0x79,
    SamplerMaterialInput7 = 0x7A,
    SamplerMaterialInput8 = 0x7B,
    SamplerMaterialInput9 = 0x7C,
    SamplerMaterialInput10 = 0x7D,
    SamplerMaterialInput11 = 0x7E,
    SamplerMaterialInput12 = 0x7F,
    SamplerMaterialInput13 = 0x80,
    SamplerMaterialInput14 = 0x81,
    SamplerMaterialInput15 = 0x82,
    LastMaterialInputSampler = 0x82,
    SamplerFxaaConsole360TexExpBiasNegOne = 0x83,
    SamplerFxaaConsole360TexExpBiasNegTwo = 0x84,
    FirstPostOutputSampler = 0x85,
    SamplerPostOutput0 = 0x85,
    SamplerPostOutput1 = 0x86,
    SamplerPostOutput2 = 0x87,
    SamplerPostOutput3 = 0x88,
    LastPostOutputSampler = 0x88,
    FirstPostInputSampler = 0x89,
    SamplerPostInput0 = 0x89,
    SamplerPostInput1 = 0x8A,
    SamplerPostInput2 = 0x8B,
    SamplerPostInput3 = 0x8C,
    SamplerPostInput4 = 0x8D,
    SamplerPostInput5 = 0x8E,
    SamplerPostInput6 = 0x8F,
    SamplerPostInput7 = 0x90,
    LastPostInputSampler = 0x90,
    FirstParticleSampler = 0x91,
    SamplerParticlePosition = 0x91,
    SamplerParticleOrientation = 0x92,
    SamplerParticleColor = 0x93,
    SamplerParticleRotation3D = 0x94,
    LastParticleSampler = 0x94,
    SamplerEnlighten = 0x95,
    LastSampler = 0x95,
    Count = 0x96,
    UniformBufferCount = 0x1E,
    GenericBufferCount = 0x1F,
    SamplerCount = 0x59,
    MaterialInputSamplerCount = 0x10,
    PostInputSamplerCount = 0x8,
    PostOutputSamplerCount = 0x4,
    ParticleSamplerCount = 0x4,
    EVSMShadowSamplerCount = 0x2,
}

public class T3VertexComponent
{
}

public class T3EffectBinaryData
{
}

public class T3EffectBinaryDataCg
{
    public class SamplerState
    {
    }

    public class ParameterLocation
    {
    }

    public class Pass
    {
    }

    public class ParameterOffsets
    {
    }

    public class VertexStreamIndex
    {
    }

    public class Technique
    {
    }
}

public class T3EffectBinaryDataHlsl_D3D
{
}

public class Trigger
{
}

public class EnlightenSystem
{
}

public class DelaunayTriangleSet
{
}

public class EnlightenData
{
}

public class EnvironmentLightGroup
{
}

public class EnlightenSignature
{
}

public class EnlightenSystemData
{
}

public class EnlightenProbeData
{
}

public class Subtitle
{
}

public class StyleIdleTransitionsResInst
{
}

public class StyleIdleManager
{
    public class FadeData
    {
    }
}

public class Procedural_LookAt_InstanceData
{
}

public class VoiceSpeaker
{
}

public class SoundAmbienceInterface
{
}

public class RenderObject_Text
{
}

public class SkeletonPoseValueContext
{
}

public class SoundSfxInterface
{
}

public class SoundSnapshotInstance
{
}

public class MetaVersionInfo
{
}

public class SoundEventPreloadInterface
{
}

public class SoundEventEmitterInstance
{
}

public class SoundReverbInterface
{
}

public class SoundListenerInterface
{
}

public class SoundMusicInterface
{
}

public class BlendGraphManagerInst
{
}

public class DialogInstance
{
    public class InstanceID
    {
    }
}

public class FontConfig
{
}

public class EnvironmentTile
{
}

public class VfxGroup
{
}

public class ParticleAffector
{
}

public class ParticleEmitter
{
}

public class ChoreAgentInst
{
    public class SyncValue
    {
    }
}

public class AnimationMixer<T>
{
}

public class PathMover
{
}

public class FontTool
{
    public class EnumLanguageSet
    {
    }
}

public class LightProbe
{
}

public class ParticlePropertySamples
{
}

public class BGM_HeadTurn_Value
{
}

public class CorrespondencePoint
{
}

public class ContainerInterface
{
}

public class CompressedPhonemeKeys
{
}

public class FilterArea
{
}

public class PathBase
{
}

public class InverseKinematicsDerived
{
}

public class SoundBankWaveMap
{
}

public class TetrahedralMeshData
{
}

public class HermiteCurvePathSegment
{
}

public class InverseKinematics
{
}

public class ParticleInverseKinematics
{
}

public class PathSegment
{
}

public class ProceduralEyes
{
}

public class SingleQuaternionValue
{
}

public class PerAgentClipResourceFilter
{
}

public class IdleTransitionSettings
{
}

public class IdleSlotDefaults
{
}

public class EventStoragePage
{
}

public class EventLoggerEvent
{
}

public class AssetCollection
{
}

public class SoundBusSystem
{
    public class BusDescription
    {
    }

    public class BusHolder
    {
    }
}

public class ZTestFunction
{
}

public class SoundReverbPreset
{
}

public class Selectable
{
}

public class RenderObject_Text2
{
}

public class RenderObjectInterface
{
}

public class ChoreInst
{
}

public class AnimationManager
{
}

public class WalkAnimator
{
}

public class CinematicLight
{
}

public class AutoActStatus
{
}

public class BlendEntry
{
}

public class ClipResourceFilter
{
}

public class CompressedTransformKeys
{
}

public class CompressedVector3Keys2
{
}

public class CompressedVertexPositionKeys
{
}

public class CompressedVertexNormalKeys
{
}

public class CompressedQuaternionKeys2
{
}

public class T3LightSceneInternalData
{
    public class QualityEntry
    {
    }

    public class LightmapPage
    {
    }
}

public class StyleIdleTransitionsRes
{
}

public class MeshSceneEnlightenData
{
}

public class CinematicLightRig
{
}

public class PlaybackController
{
}

public class SoundSystem
{
    public class Implementation
    {
        public class ChannelHolder
        {
        }
    }
}

public class KeyframedValueInterface
{
}

public class DateStamp
{
}

public class SoundBankWaveMapEntry
{
}

public class SingleVector3Value
{
}

public class Guide
{
}

public class T3RenderStateBlock
{
}

public class RenderObject_PostMaterial
{
}

public class WalkPath
{
}

public class T3OcclusionMeshData
{
}

public class SoundEventBankMap
{
}

public class MeshSceneLightmapData
{
    public class Entry
    {
    }
}

public class PhysicsData
{
}

public class LipSync
{
}

public class LipSync2
{
}

public class CSPK2Context
{
}

public class InverseKinematicsAttach
{
}

public class LightInstance
{
}

public class Rollover
{
}

public class SkeletonPoseValue
{
    public class Sample
    {
    }

    public class BoneEntry
    {
    }
}

public class RootKey
{
}

public class HingeJointKey
{
}

public class PivotJointKey
{
}

public class BallTwistJointKey
{
}

public class LightProbeData
{
}

public class StringFilter
{
}

public class T3OcclusionMeshBatch
{
}

public class T3MaterialSwizzleParams
{
}

public class EventStorage
{
    public class PageEntry
    {
    }
}

public class JiraRecord
{
}

public class ChorecorderParameters
{
}

public class CameraFacingTypes
{
}

public class Agent
{
}

public class Camera
{
}

public class SceneInstData
{
}

public class SkeletonInstance
{
}

public class Mover
{
}

public class PhysicsObject
{
    public class EnumePhysicsBoundingVolumeType
    {
    }

    public class EnumePhysicsCollisionType
    {
    }
}

public class T3LightCinematicRigLOD
{
}

public class ParticleProperties
{
    public class Animation
    {
    }

    public class AnimationParams
    {
    }
}

public class EnvironmentLight
{
}

public class ActingAccentPalette
{
}

public class RecordingUtils
{
    public class EnumRecordingStatus
    {
    }
}

public class CameraSelect
{
}

public class PointOfInterestBlocking
{
}

public class KeyframedValueSteppedString
{
}

public class BlendCameraResource
{
}

public class EnumeTangentModes
{
}

public class LinkedBallTwistJointKey
{
}

public class PreloadPackage
{
    public class ResourceSeenTimes
    {
    }

    public class RuntimeDataDialog
    {
        public class DialogResourceInfo
        {
        }

        public class DlgObjIdAndResourceVector
        {
        }

        public class DlgObjIdAndStartNodeOffset
        {
        }
    }

    public class RuntimeDataScene
    {
    }

    public class ResourceKey
    {
    }

    public class StartNodeOffset
    {
    }
}

public class SoundFootsteps
{
    public class EnumMaterial
    {
    }
}

public class SklNodeData
{
}

public class ParticlePropConnect
{
}

public class Matrix4
{
}

public class InverseKinematicsBase
{
}

public class ParticleLODKey
{
}

public class T3HeapAllocator
{
}

public class T3NormalSampleData
{
}

public class T3VertexBufferSample<T, T1>
{
}

public class T3PositionSampleData
{
}

public class ResourceGroupInfo
{
}

public class SoundAmbience
{
    public class AmbienceDefinition
    {
    }

    public class EventContext
    {
    }
}

public class SoundEventSnapshotData
{
}

public class SoundEventData
{
}

public class SoundReverbDefinition
{
}

public class SoundBusSnapshot
{
    public class Snapshot
    {
    }

    public class SnapshotSuite
    {
    }
}

public class SingleValue<T>
{
}

public class ParticleSprite
{
    public class Animation
    {
    }
}
public class RenderObject_Mesh
{
    public class TextureInstance
    {
    }

    public class MeshInstance
    {
    }

    public class TriangleSetInstance
    {
    }

    public class VertexAnimationInstance
    {
    }
}

public class PlaceableBallTwistJointKey
{
}

public class CompressedKeys<T>
{
}

public class BallJointKey
{
}

public class PathTo
{
}

public class Style
{
}

public class AgentState
{
}

public class EnlightenModule
{
    public class EnumeQualityWithDefault
    {
    }

    public class EnlightenSystemSettings
    {
    }

    public class EnumeProbeResolution
    {
    }

    public class EnlightenCubemapSettings
    {
    }

    public class EnlightenAdaptiveProbeVolumeSettings
    {
    }

    public class EnumeQuality
    {
    }

    public class EnlightenProbeVolumeSettings
    {
    }

    public class EnlightenAutoProbeVolumeSettings
    {
    }

    public class EnlightenLightSettings
    {
    }

    public class EnlightenPrimitiveSettings
    {
    }

    public class EnumeInstanceType
    {
    }

    public class EnumeUpdateMethod
    {
    }

    public class EnumeDistributedBuildSystem
    {
    }

    public class EnumeSceneOptimisationMode
    {
    }

    public class EnumeBackfaceType
    {
    }

    public class EnumeAutoUVSimplificationMode
    {
    }

    public class EnumeProbeSampleMethod
    {
    }

    public class EnumeDisplayQuality
    {
    }

    public class EnumeRadiositySampleRate
    {
    }

    public class EnumeAgentUsage
    {
    }

    public class EnumeUpdateMethodWithDefault
    {
    }

    public class EnumeProbeResolutionWithDefault
    {
    }

    public class EnlightenMeshSettings
    {
        public class AutoUVSettings
        {
        }
    }

    public class EnumeSimplifyMode
    {
    }
}

public class LightGroupInstance
{
}

public class T3LightEnvInternalData
{
    public class QualityEntry
    {
    }
}

public class ColorHDR
{
}

public class ResourceBundle
{
    public class ResourceInfo
    {
    }
}

public class T3LightProbeInternalData
{
    public class QualityEntry
    {
    }
}

public class CompressedPathBlockingValue
{
    public class CompressedPathInfoKey
    {
    }
}

public class CompressedSkeletonPoseKeys
{
}

public class BlendGraph
{
    public class EnumBlendGraphType
    {
    }
}

public class AnimationConstraintParameters
{
}

public class BlendGraphManager
{
}

public class DebugString
{
}

public class SkeletonPose
{
}

public class SoundData
{
}
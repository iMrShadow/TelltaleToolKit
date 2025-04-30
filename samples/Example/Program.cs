using TelltaleToolKit;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Textures;
using TelltaleToolKit.T3Types.Textures.T3Types;
using TelltaleToolKit.Utility;

// Set up the context from a folder.
TTKContext.Instance().Load("../../../../../data");

// (Recommended) Set the active game for default configuration.
TTKContext.Instance().SetActiveGame("the-walking-dead-definitive-series-2019");

// Replace the path with a valid one.
// Load a Telltale archive. 
using var archive = TTK.Load("WDC_pc_WalkingDead404_txmesh.ttarch2", T3BlowfishKey.Twdc);

// Extract a file from the archive in a stream.
var blob = archive.ExtractFile("obj_backpackClementine400.d3dtx");

// Load the d3dtx from a memory stream.
var d3dtxObj = TTK.Load<T3Texture>(blob, out MetaStreamConfiguration config);

// Alternatively, load the texture directly from the filesystem.
// Replace the path with a valid one.
// TTK.Load<T3Texture>("obj_backpackClementine400.d3dtx", out config);

// Modify the texture.
d3dtxObj.Name = "My new modified texture!";
d3dtxObj.SurfaceFormat = T3SurfaceFormat.ARGB8;
d3dtxObj.Width = 1024;
d3dtxObj.Height = 1024;

// Save the modified texture on the filesystem.
TTK.Save(d3dtxObj, "new_modified.d3dtx", config);
using TelltaleToolKit;
using TelltaleToolKit.T3Types.Textures;
using TelltaleToolKit.T3Types.Textures.T3Types;

// 1. Initialize once — loads game profiles, hash databases, and MetaClass registry.
Toolkit.Initialize();

// 2. Create a workspace for the target game.
Workspace workspace = Toolkit.Instance.CreateWorkspace(
    "The Walking Dead Workspace",
    gameProfile: "The Walking Dead: Definitive Series");

// 3. Mount the game data.
//
//    Version 2 game — load via a resource description (.resdesc) Lua script.
//    The script supplies archive paths, a context name, and a priority automatically.
//
//    await workspace.LoadResourceDescriptionAsync("C:/GameData/WalkingDead.resdesc");
//
//    Version 1 game — mount folders directly.
//    Add a patch folder at a higher priority so it overrides the base data.
//
//    workspace.MountGameFolder("C:/GameData",       priority: 0);
//    workspace.MountGameFolder("C:/GameData/Patch", priority: 10);
//
//    Or mount a single archive by itself:
workspace.LoadArchive("WDC_pc_WalkingDead404_txmesh.ttarch2", contextName: "WalkingDead404 Textures");

// 4. Load an asset.
T3Texture? texture = workspace.LoadAsset<T3Texture>("obj_backpackClementine400.d3dtx");

if (texture != null)
{
    // 5. Modify it.
    texture.Name = "obj_backpackClementine400_modified";
    texture.SurfaceFormat = T3SurfaceFormat.ARGB8;
    texture.Width = 1024;
    texture.Height = 1024;

    // 6. Export it back to disk.
    workspace.ExportAsset(texture, "obj_backpackClementine400_modified.d3dtx");
}

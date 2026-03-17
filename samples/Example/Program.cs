using TelltaleToolKit;
using TelltaleToolKit.Resource;
using TelltaleToolKit.T3Types.Textures;
using TelltaleToolKit.T3Types.Textures.T3Types;

// Set up the context from a folder.
var toolkitConfiguration = new Toolkit.Configuration
{
    DataFolder = "../../../../../data",
};

// Initialize the library.
Toolkit.Initialize(toolkitConfiguration);

// (Recommended) Create a workspace if you want to work with a specific game.
Workspace workspace = Toolkit.Instance.CreateWorkspace("The Walking Dead", "The Walking Dead: Definitive Series");

// Create a resource context.
ResourceContext resourceContext = workspace.CreateResourceContext("Texture Archives", 100);

// Add an archive to the resource context.
resourceContext.AddProvider(new ArchiveProvider("WDC_pc_WalkingDead404_txmesh.ttarch2", workspace));

// Replace the path with a valid one.
var texture = workspace.LoadAsset<T3Texture>("obj_backpackClementine400.d3dtx");

// Alternatively, load the texture directly from the filesystem without a workspace and a resource context.
// var textureDisk = Toolkit.Instance.LoadObject<T3Texture>("obj_backpackClementine400.d3dtx", out MetaStreamConfiguration  _);

if (texture != null)
{
    // Modify the texture.
    texture.Name = "My new modified texture!";
    texture.SurfaceFormat = T3SurfaceFormat.ARGB8;
    texture.Width = 1024;
    texture.Height = 1024;
    
    // Save the modified texture on the filesystem.
    workspace.SaveObject(texture, "new_modified.d3dtx");
}




[![Nuget](https://img.shields.io/nuget/v/TelltaleToolKit)](https://www.nuget.org/packages/TelltaleToolKit/)
![License](https://img.shields.io/github/license/iMrShadow/TelltaleToolKit)

<!-- omit in toc -->
# TelltaleToolKit

`TelltaleToolKit` is a .NET library created to allow modding games which run on the [Telltale Tool](https://www.pcgamingwiki.com/wiki/Engine:Telltale_Tool) game engine.

<!-- omit in toc -->
## Table of contents:
- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Supported Games](#supported-games)
- [License](#license)
- [Credits](#credits)

## Introduction

[Telltale Tool](https://www.pcgamingwiki.com/wiki/Engine:Telltale_Tool) is a proprietary game engine, originally created by Telltale Games. The engine was never publicly released which made official modding from very limited to impossible. This library aims to help modders with developing modding tools, plugins and converters, scripting, and more.

## Features

- Extract and create archive files (`.ttarch` and `.ttarch2`).
- Open, edit, and save assets (textures, meshes, sounds, and more).
- Create environments for working with assets of a specific Telltale game with resource contexts.
- Modular and flexible registration system (types, metaclasses, serializers, per-game configs).
- Create and manage simple hash databases.
- Cross-platform: Windows, Linux, Mac (requires .NET 8.0 or later, or .NET Standard 2.1).
- For more details, check the [documentation folder](docs/README.md).

## Installation

You can install `TelltaleToolKit` via NuGet Package Manager:

```sh
Install-Package TelltaleToolKit
```
Or add it to your .csproj file:
```xml
<PackageReference Include="TelltaleToolKit" Version="0.2.1" />
```

The Nuget package ships a default database, but if you want the latest one - you can download it from [this link](https://downgit.github.io/#/home?url=https://github.com/iMrShadow/TelltaleToolKit/tree/main/data).

## Usage

```csharp
using TelltaleToolKit;
using TelltaleToolKit.T3Types.Textures;
using TelltaleToolKit.T3Types.Textures.T3Types;

// 1. Initialize the library.
Toolkit.Initialize();

// 2. Create a workspace for the target game.
Workspace workspace = Toolkit.Instance.CreateWorkspace(
    "The Walking Dead Workspace",
    gameProfile: "The Walking Dead: Definitive Series");

// 3. Mount the game data.
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
```

## API Documentation

The API is currently **unstable** and may change. For now, refer to the source code, inline XML docs.

## Supported Games

See the [data folder](data/README.md) for more information regarding supported games.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/iMrShadow/TelltaleToolKit/blob/main/LICENSE) file for more information.

## Credits

Thanks to [Gamma_02](https://github.com/gamma-02) for contributing a lot to this project such as adding resdesc parser and many improvements to the API. The Lua decompiler is based on their [UnLuaCSharp](https://github.com/gamma-02/UnLuaCSharp) library.

Thanks to [Knollad Knolladious](https://github.com/LBPHaxMods) for adding `D3DMesh` serialization support.

Thanks to [Plague](https://x.com/QueenPlagueCure) for providing version databases for "Borderlands" (2021, PC and Nintendo Switch).

Thanks to Pumba for providing version databases for "The Wolf Among Us".

Thanks to [Lucas Saragosa](https://github.com/LucasSaragosa) for his outstanding work on [`TelltaleToolLib`](https://github.com/LucasSaragosa/TelltaleToolLib/tree/main), [`Telltale Inspector`](https://github.com/LucasSaragosa/TelltaleInspector) and [`Telltale Editor`](https://github.com/Telltale-Modding-Group/Telltale-Editor), which made me understand Telltale's meta system.

Thanks to [Luigi Auriemma](https://aluigi.altervista.org/index.htm) for their [`ttarchext`](https://aluigi.altervista.org/papers.htm#others-file), which laid much of the groundwork for `.ttarch` and `.ttarch2` extraction.

Thanks to [Pavel Sudakov](https://github.com/zenderovpaulo95) and [Heitor Spectre](https://github.com/HeitorSpectre) for their [`TTG Tools`](https://github.com/zenderovpaulo95/TTG-Tools), which provided additional references for `.ttarch` and `.ttarch2` extraction.

Thanks to [Bennyboy](https://github.com/bgbennyboy) for their work on [`Telltale Music Extractor`](https://github.com/bgbennyboy/Telltale-Music-Extractor), which contained some additional blowfish keys for demo games.

Thanks to [Azil Zogby](https://github.com/asilz) for his work on [`TelltaleHydra`](https://github.com/asilz/TelltaleHydra) and [`TelltaleDevTool`](https://github.com/asilz/TelltaleDevTool).

Thanks to all [contributors](https://github.com/stride3d/stride/graphs/contributors) which worked on the popular C# game engine [`Stride`](https://github.com/stride3d/stride). The serialization system is inspired from there.

Thanks to [David Matos](https://github.com/frostbone25) for introducing me to the `Telltale Modding Community`.

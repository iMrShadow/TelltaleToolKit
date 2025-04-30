[![Nuget](https://img.shields.io/nuget/v/TelltaleToolKit)](https://www.nuget.org/packages/TelltaleToolKit/)
[![License](https://img.shields.io/github/license/iMrShadow/TelltaleToolKit)](LICENSE)

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

- Open and extract files from `.ttarch` and `.ttarch2` archives.
- Open, edit, and save file formats (textures, meshes, sounds, and more).
- Modular and flexible registration system (types, metaclasses, serializers, per-game configs).
- Create and manage a simple SQLite hash database.
- Cross-platform: Windows, Linux, Mac (requires .NET 8.0 or later).
- For more details, check the [documentation folder](docs/README.md).

## Installation

You can install `TelltaleToolKit` via NuGet Package Manager:

```sh
Install-Package TelltaleToolKit
```
Or add it to your .csproj file:
```xml
<PackageReference Include="TelltaleToolKit" Version="0.1.0" />
```

You must also download the latest database from the data folder. You can use [this link](https://downgit.github.io/#/home?url=https://github.com/iMrShadow/TelltaleToolKit/tree/main/data).

## Usage

```csharp
using TelltaleToolKit;
using TelltaleToolKit.Serialization.Binary;
using TelltaleToolKit.T3Types.Textures;
using TelltaleToolKit.T3Types.Textures.T3Types;
using TelltaleToolKit.Utility;

// Set up the context from a folder.
TTKContext.Instance().Load("../../../../../data");

// (Recommended) Set the active game for default configuration.
// This is not required, if you only read files.
TTKContext.Instance().SetActiveGame("the-walking-dead-definitive-series-2019");

// Load a Telltale archive. 
using var archive = TTK.Load("WDC_pc_WalkingDead404_txmesh.ttarch2", T3BlowfishKey.Twdc);

// Extract a file from the archive in a stream.
var blob = archive.ExtractFile("obj_backpackClementine400.d3dtx");

// Load the d3dtx from a stream.
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
```

## API Documentation

The API is currently **unstable** and may change. For now, refer to the source code, inline XML docs.

## Supported Games

See the [data folder](data/README.md) for more information regarding supported games. 

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/iMrShadow/TelltaleToolKit/blob/main/LICENSE) file for more information.

## Credits

Thanks to [Lucas Saragosa](https://github.com/LucasSaragosa) for his outstanding work on [`TelltaleToolLib`](https://github.com/LucasSaragosa/TelltaleToolLib/tree/main), [`Telltale Inspector`](https://github.com/LucasSaragosa/TelltaleInspector) and [`Telltale Editor`](https://github.com/Telltale-Modding-Group/Telltale-Editor), which made me understand Telltale's meta system.

Thanks to [Luigi Auriemma](https://aluigi.altervista.org/index.htm) for their [`ttarchext`](https://aluigi.altervista.org/papers.htm#others-file), which laid much of the groundwork for `.ttarch` and `.ttarch2` extraction.

Thanks to [Pavel Sudakov](https://github.com/zenderovpaulo95) and [Heitor Spectre](https://github.com/HeitorSpectre) for their [`TTG Tools`](https://github.com/zenderovpaulo95/TTG-Tools), which provided additional references for `.ttarch` and `.ttarch2` extraction.

Thanks to [Bennyboy](https://github.com/bgbennyboy) for their work on [`Telltale Music Extractor`](https://github.com/bgbennyboy/Telltale-Music-Extractor), which contained some additional blowfish keys for demo games.

Thanks to [Azil Zogby](https://github.com/asilz) for his work on [`TelltaleHydra`](https://github.com/asilz/TelltaleHydra) and [`TelltaleDevTool`](https://github.com/asilz/TelltaleDevTool).

Thanks to all [contributors](https://github.com/stride3d/stride/graphs/contributors) which worked on the popular C# game engine [`Stride`](https://github.com/stride3d/stride). The serialization system is inspired from there.

Thanks to [David Matos](https://github.com/frostbone25) for introducing me to the `Telltale Modding Community`.

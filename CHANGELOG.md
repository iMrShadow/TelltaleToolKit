# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## Unreleased

### Added
- `OpenWrite` and `OpenRead` static functions in `MetaStream`.
- Experimental `JsonMetaStreamReader` and `JsonMetaStreamWriter` classes. You can export serialize objects to JSON files in `MetaStream`.
- Support for encrypted meta stream files with `LegacyEncryptedStream` which matches with Telltale Tool in `MetaStream`. This requires a workspace. The user also has the option to encrypt/decrypt using the `LegacyEncryption` class provided with a Blowfish key and an archive version.
- An abstract `Close` method.
- Experimental support for compressed meta stream resources.
- `ContainerStreamParams` class.
- `StreamVersion` to `GameProfile`.
- A constructor with a boolean argument in `Blowfish`.
- `Workspace` parameter in deserialize methods in `Toolkit`.
- Helper methods for `VersionInfo`.
- Helper version crc calculator method for `MetaClass`.
- Better `TTArchive` compression detection.
- A dedicated `Compression` class.
- Support for many new types, including SArrays.
- Missing enums.
- Added missing members to support some older games.
- Support for .ambience files.
- SectionDepth in MetaStream.
- API related to MetaStream to improve serialization support and parity.
- Logging in MetaClassSerializer<T>.
- Many serializers to handle support for old games and improve existing ones.
- Logging when there's no profilesPath found.

### Changed
- Rename the following MetaStream implementations: `MetaStreamReader`/`MetaStreamWriter` to  `BinaryMetaStreamReader`/`BinaryMetaStreamWriter`.
- Rename `MetaStreamConfig` to `MetaStreamParams`.
- Update database files with the new changes.
- Exposed `GetClass(ulong, uint)` method from `MetaClassRegistry`.
- Refactor `MetaStream` internally.
- Refactor `MetaStreamParams` with new fields and methods to handle compression and encryption.
- Reorganize some namespaces.
- Make the default serializer not throw errors and use logger instead.
- `Serialize` from `MetaStream` now automatically preserializes.
- Namespaces organization. (#24)
- Various class names. (#24)
- Symbols now use their own serializers instead of the ones.
- Use LRU caching by default for Containers.
- Removed constraint for GetSerializer<T> - it should support all types.
- Make Blowfish key optional in Containers.
- Animation serializer to return rather than throw when there's an unregistered type.

### Fixed
- A regression that did not allow files being copied to the output folder.
- Blowfish encryption not working correctly.
- Many serializers.
- Zlib detection for TTArchive.
- MBIN and MTRE stream support.
- TTArchive2 creation.
  
### Removed
- A csproj folder.
- Redundant `MetaStreamVersion` from `GameProfile`.
- `AreSymbolsHashed` from `GameProfile` as symbols are always hashed.
- `PreSerialize` from `MetaStream`.
- Redundant `Version` from `MetaStreamParams`.
- Unused `ISymbolResolver` interface.
- Symbol serializing from MetaStream. This does not affect anything except if you use `stream.Serialize(obj)` of type Symbol.

## [0.2.1] - 2026-05-28
### Fixed
- Data files not being copied to output directory.

## [0.2.0] - 2026-05-28

### Added
- `Toolkit` singleton class that acts as an entry point to the library. 
  - Global registry for game profiles, metaclasses, and symbol resolution.
  - Centralized archive loading with automatic format detection.
  - Object serialization/deserialization for MetaStream files.
  - Must be initialized with `Toolkit.Initialize()` before use.
  - Logging support.
- `HashDatabase` class - Thread-safe, in-memory symbol resolution database.
  - Stores CRC64 -> Symbol name mappings with concurrent dictionary.
  - Supports batch imports from text files and directories.
  - Event-driven architecture with `SymbolsCleared`, `SymbolAdded`, `SymbolRemoved`.
  - Export functionality to tab-separated value files.
  - Read-only mode support for immutable databases.
- `Workspace` class - Game-specific working environment.
  - Created from a `GameProfile` via `Toolkit.CreateWorkspace()`.
  - Manages prioritized resource contexts, similar to Telltale Tool.
  - Handles file extraction with proper override order (higher priority wins).
  - Provides game-specific metaclass lookup and symbol resolution.
  - Default `MetaStream` configuration derived from the game profile.
  - Can create resource contexts from Lua resdesc files.
- `IFileProvider` interface - Common abstraction for file sources.
  - Implemented by `ArchiveProvider`, `FolderProvider`, and `LooseFileProvider`.
  - Supports lookup by both CRC64 and filename.
  - Enables composable, priority-based file systems.
- `ResourceContext` class - Named collection of file providers with explicit priority.
  - Can be enabled/disabled at runtime.
  - Automatically disposed when removed from workspace.
  - Can contain archives, subfolders and regular files.
- Symbol resolution improvements.
  - Workspace-level `LocalHashDatabase` for game-specific symbols.
  - Automatic fallback: global DB -> workspace DB -> mounted file names.
  - Batch resolution with `ResolveSymbols()`.
- Various new T3Types for compatibility with MTRE, MSV5 and MSV6 games.
- Support for `resdesc` (#1).
- Support for container streams (#18).
- Experimental support creating archives. (#18)
- Experimental `PropertySet` serializer.
- Helper methods for `T3Texture` and `PropertySet`.
- Serializers for `D3DMesh` related classes.
- Some animation/skeleton related types in the type registry.
- New contributors in README.md.

### Changed
- `GameDescriptor` renamed to `GameProfile` to better reflect its purpose.
- `GameContext` renamed to `Workspace` for clarity.
- Some class members now have default values.
- Renamed `TelltaleFileEntry` to `ResourceEntry` to match Telltale's naming.
- Renamed, reorganized and refactored various classes in `TelltaleArchives`. (#18)
- Refactored `Symbol`. (#14)
- NuGet related metadata in `TelltaleToolKit.csproj`.
- Package the data folder as `ttk-data`.

### Removed
- `TTKContext` singleton class. It has been replaced by `Toolkit`.
- `TTK` static class. Functions are replaced by the ones in `Toolkit`.
- Default static class member in `MetaStreamConfiguration`.

### Fixed
- Due to the heavy refactoring, most old bugs do not apply to this version.
- Various bugs regarding loading and saving have been fixed.

## [0.1.0] - 2025-09-30

### Added
- `.ttarch` and `.ttarch2` extractors for all games which **do not** use `Oodle` compression. The extractors **do not** automatically decrypt Lua files.
- Initial support for the following file formats: `.aam`, `.amap`, `.anm`, `.aud`, `.chore`, `.d3dmesh`, `.d3dtx`, `.dlg`, `.dlog`, `.dss`, `.font`, `.imap`, `.landb`, `.lanreg`, `.lang`, `.langdb`, `.langres`, `.ldb`, `.locreg`, `.look`, `.note`, `.overlay`, `.ptable`, `.prop`, `.rules`, `.save`, `.scene`, `.skl`, `.style`, `.tmap`, `.vox`, `.wbox`. Currently most serializers are **unfinished and unreliable, especially in writing mode**. Over time, they will get polished and refined.
- Registration system for types, metaclasses, type serializers and game descriptors (game configurations).

[unreleased]: https://github.com/iMrShadow/TelltaleToolKit/compare/0.2.1...HEAD
[0.2.1]: https://github.com/iMrShadow/TelltaleToolKit/releases/tag/0.2.1
[0.2.0]: https://github.com/iMrShadow/TelltaleToolKit/releases/tag/0.2.0
[0.1.0]: https://github.com/iMrShadow/TelltaleToolKit/releases/tag/0.1.0
  
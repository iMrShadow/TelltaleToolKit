# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

### Added
- `Toolkit` singleton class that acts as an entry point to the library. 
  - Global registry for game profiles, metaclasses, and symbol resolution.
  - Centralized archive loading with automatic format detection.
  - Object serialization/deserialization for MetaStream files.
  - Must be initialized with `Toolkit.Initialize()` before use.
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
  - Default MetaStream configuration derived from the game profile.
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
- Various new T3Types for compatibility with MSV5 and MSV6 games.
- Experimental `PropertySet` serializer.
- Helper methods for `T3Texture` and `PropertySet`.
- Some animation/skeleton related types in the type registry.
- New contributor in README.md.

### Changed
- `GameDescriptor` renamed to `GameProfile` to better reflect its purpose.
- `GameContext` renamed to `Workspace` for clarity.

- Moved `Lua` relate
- Some `T3Texture` members now have default values.
- NuGet related metadata in `TelltaleToolKit.csproj`.

### Removed
- `TTKContext` singleton class. It has been replaced by `Toolkit`.
- `TTK` static class. Functions are replaced by the ones in `Toolkit`.
- Default static class member in `MetaStreamConfiguration`.

### Fixed
- Due to the heavy refactoring, most old bugs do not apply to this version.
- Various bugs regarding loading and saving have been fixed.
- `Save<T>` without a configuration now works properly.

## [0.1.0] - 2025-09-30

### Added
- `.ttarch` and `.ttarch2` extractors for all games which **do not** use `Oodle` compression. The extractors **do not** automatically decrypt Lua files.
- Initial support for the following file formats: `.aam`, `.amap`, `.anm`, `.aud`, `.chore`, `.d3dmesh`, `.d3dtx`, `.dlg`, `.dlog`, `.dss`, `.font`, `.imap`, `.landb`, `.lanreg`, `.lang`, `.langdb`, `.langres`, `.ldb`, `.locreg`, `.look`, `.note`, `.overlay`, `.ptable`, `.prop`, `.rules`, `.save`, `.scene`, `.skl`, `.style`, `.tmap`, `.vox`, `.wbox`. Currently most serializers are **unfinished and unreliable, especially in writing mode**. Over time, they will get polished and refined.
- Registration system for types, metaclasses, type serializers and game descriptors (game configurations).

[unreleased]: https://github.com/iMrShadow/TelltaleToolKit/compare/0.1.0...HEAD
[0.1.0]: https://github.com/iMrShadow/TelltaleToolKit/releases/tag/0.1.0
  
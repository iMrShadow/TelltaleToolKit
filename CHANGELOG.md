# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

## [Unreleased]

### Added
- Helper methods for `T3Texture`.
- Method to retrieve the currently active game descriptor.

### Changed
- IsMetaFile method has more excluded file types.
- Some `T3Texture` members now have default values.

### Fixed

## [0.1.0] - 2025-09-30

### Added
- `.ttarch` and `.ttarch2` extractors for all games which **do not** use `Oodle` compression. The extractors **do not** automatically decrypt Lua files.
- Initial support for the following file formats: `.aam`, `.amap`, `.anm`, `.aud`, `.chore`, `.d3dmesh`, `.d3dtx`, `.dlg`, `.dlog`, `.dss`, `.font`, `.imap`, `.landb`, `.lanreg`, `.lang`, `.langdb`, `.langres`, `.ldb`, `.locreg`, `.look`, `.note`, `.overlay`, `.ptable`, `.prop`, `.rules`, `.save`, `.scene`, `.skl`, `.style`, `.tmap`, `.vox`, `.wbox`. Currently most serializers are **unfinished and unreliable, especially in writing mode**. Over time, they will get polished and refined.
- Registration system for types, metaclasses, type serializers and game descriptors (game configurations).

[unreleased]: https://github.com/iMrShadow/TelltaleToolKit/compare/0.1.0...HEAD
[0.1.0]: https://github.com/iMrShadow/TelltaleToolKit/releases/tag/0.1.0
  
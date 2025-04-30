## Telltale Archives (`.ttarch` and `.ttarch2`)

Telltale Archives are zip-like containers used for storing game data, with optional encryption and compression.

---

### TTArch (`.ttarch`) Support Overview

| TTArch Version | Extracting | Rebuilding | Notes                |
| -------------- | :--------: | :--------: | -------------------- |
| 1 (normal)     |     ✔️      |     ❌      | No version indicator |
| 1 (encrypted)  |     ✔️      |     ❌      | No version indicator |
| 2              |     ✔️      |     ❌      |
| 3              |     ✔️      |     ❌      |
| 4              |     ✔️      |     ❌      |
| 5              |     ✔️      |     ❌      |
| 6              |     ✔️      |     ❌      |
| 7              |     ✔️      |     ❌      |
| 8              |     ✔️      |     ❌      |
| 9              |     ✔️      |     ❌      |

#### Storage Type Support
| Storage Type                       | Extracting | Rebuilding |
| ---------------------------------- | :--------: | :--------: |
| Normal                             |     ✔️      |     ❌      |
| Encrypted                          |     ✔️      |     ❌      |
| Raw Deflate Compressed             |     ✔️      |     ❌      |
| Zlib Compressed                    |     ✔️      |     ❌      |
| Encrypted + Raw Deflate Compressed |     ✔️      |     ❌      |
| Encrypted + Zlib Compressed        |     ✔️      |     ❌      |

---

### TTArch2 (`.ttarch2`) Support Overview

| TTArch2 Version | Extracting | Rebuilding |
| --------------- | :--------: | :--------: |
| TTA2            |     ✔️      |     ❌      |
| TTA3            |     ✔️      |     ❌      |
| TTA4            |     ✔️      |     ❌      |

| TTArch2 Storage Type                           | Extracting | Rebuilding |
| ---------------------------------------------- | :--------: | :--------: |
| TTCN (Normal)                                  |     ✔️      |     ❌      |
| TTCZ/TTCz (Raw Deflate Compressed)             |     ✔️      |     ❌      |
| TTCz (Oodle Compressed)                        |     ❌      |     ❌      |
| TTCE/TTCe (Encrypted + Raw Deflate Compressed) |     ✔️      |     ❌      |
| TTCe (Encrypted + Oodle Compressed)            |     ❌      |     ❌      |

> [!NOTE]
> `TTCz` and `TTCe` can use either Raw Deflate or Oodle compression. The actual method is specified by a separate field in the archive.

> [!WARNING]
> Rebuilding archives with Oodle compression is unlikely to be supported. Telltale used a legacy Oodle (LZNA) algorithm that is no longer present in modern versions. If anyone is more knowledgeable about this topic, please contact me.
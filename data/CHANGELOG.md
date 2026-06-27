## Database Changelog

### 26th of June, 2026

#### Game Profiles
- Added initial game profiles for the following games:
  - Tales of Monkey Island (2010) (PC) (All 5 Episodes)
  - Back To The Future: The Game (2011) (All 5 Episodes)
  - Jurassic Park: The Game (2011)
  - Puzzle Agent 2 (2011)
  - Lad & Order - Legacies (2012)
  - Poker Night 2 (2013)
- Fixed an encryption key for The Walking Dead: Season 2.

#### Version Database
- Added initial version databases for aforementoned games.
- Updated the global database with the new meta class descriptions.
- Implemented meta class descriptions for animations. 
- Added a missing member in DlgChild for The Wolf Among Us and The Walking Dead: Season 2.
- Removed some files that weren't used.

### 1st of June, 2026

#### Game Profiles
- Added `StreamVersion` field that matches Telltale Tool. Please refer to the [docs](README.md#game-profile-fields).
- Removed redundant `MetaStreamVersion` and `AreSymbolsHashed` fields. 
- Updated database files with the new changes.

### 28th of May, 2026

#### Game Profiles
- Added initial game profile for The Wolf Among Us (2013)(Thank you Pumba!).

#### Version Database
- Added initial version database for The Wolf Among Us (2013).

### 28th of April, 2026

#### Game Profiles
- Added a game profile and its corresponding version database for "The Walking Dead (2012)".

#### Version Database
- Added new metaclass descriptions to the global database.
  - Added initial MTRE types which mainly support "The Walking Dead (2012)". This indirectly adds support for other MTRE games which were released relatively early to the game such as "Jurassic Park: The Game (2011)" and "Back to the Future: The Game (2010)", but this requires further testing.
  - Added more MSV5 types which affect "The Walking Dead: Season 2 (2013)" and "The Wolf Among Us (2013)". These types used non-fixed-width primitive types (like ```int``` or ```unsigned long```) which affected almost all CRC32 checksums.
- Added more class description mappings for "The Walking Dead: Season 2 (2012)"
- Registered new types.

### 24th of April, 2026

#### Game Profiles
- Added the year to TWDS2 game profile json file name to match other entries (Thank you @gamma-02!).

### 17th of March, 2026

#### Game Profiles
- Added initial game profile for Poker Night at the Inventory Remastered (2026) (Thank you @gamma-02!).

#### Version Database
- Added initial version database for aforementoned Poker Night at the Inventory Remastered (2026).

### 18th of February, 2026

#### Game Profiles
- Added initial game profiles for the following games:
  - Sam and Max: Season Two (2007)
  - The Walking Dead: Season Two (2013)
  - Tales of the Borderlands (2021) (Thank you [Plague](https://x.com/QueenPlagueCure))

#### Version Database
- Added initial version databases (.vdb) for all mentioned games in the game profiles section.
- Modified a lot of database CRC32 values.

#### Hash Database
- Added initial hash databases as .txt files.
  - Added over 3700 property keys, 2/3rds imported from Telltale Inspector.
  - Added bone names from RTB's 3ds Max Script repository.
  - Added bone groups from Telltale Inspector.
---

### 30th of September, 2025

#### Game Profiles
- Added initial game profiles for the following games:
  - Telltale Texas Hold'em (2005)
  - Bone: Out from Boneville (2005)
  - Bone: The Great Cow Race (2006)
  - CSI: Crime Scene Investigation - 3 Dimensions of Murder (2006) (Demo)
  - Sam & Max: Episode 1 - Culture Shock (2006) (Demo)
  - Sam & Max: Episode 2 - Situation: Comedy (2006) (Demo)
  - Sam & Max: Episode 3 - The Mole, the Mob, and the Meatball (2006) (Demo)
  - Sam & Max: Episode 4 - Abe Lincoln Must Die! (2006) (Demo)
  - Sam & Max: Episode 5 - Reality 2.0 (2006) (Demo)
  - Sam & Max: Episode 6 - Bright Side of the Moon (2006) (Demo)
  - Bone: Out from Boneville (2007) (Demo)
  - Bone: Act 1 & 2 Combo Pack (2007)
  - CSI: Crime Scene Investigation - Hard Evidence (2007) (Demo)
  - Poker Night 2 (2013)
  - Borderlands (2015)
  - Game of Thrones (2015)
  - Tales from the Borderlands (2015)
  - Minecraft: Story Mode (2015)
  - The Walking Dead: Michonne (2016)
  - Batman the Telltale Series (2016-12)
  - The Walking Dead: A New Frontier (2017)
  - Marvel Guardians of the Galaxy: The Telltale Series (2017)
  - Minecraft: Story Mode - Season Two (2017)
  - Batman: The Telltale Series - The Enemy Within (2017)
  - The Walking Dead Collection (2017)
  - The Walking Dead: The Final Season (2018)
  - The Walking Dead: The Telltale Definitive Series (2019)

#### Version Database
- Added initial version databases (.vdb) for all mentioned games in the game profiles section.
- Poker Night 2 only has D3DTX related classes registered.
- The following databases are only for developer purposes or unfinished.
  - ignore-unsupported.vdb.json
  - ignore.vdb.json
  - jurassic-park-the-game-NEW.vdb.json
  - jurassic-park-the-game-UNSUPPORTED.vdb.json

#### Hash Database
- Not implemented yet.

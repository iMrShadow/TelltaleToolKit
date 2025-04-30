using System.ComponentModel.DataAnnotations;

namespace TelltaleToolKit.Utility;

/// <summary>
/// Enum representing the various Telltale games which use different blowfish keys.
/// This list can grow in the future, but these are all the currently known keys.
/// </summary>
public enum T3BlowfishKey
{
    [Display(Name = "Telltale Texas Hold'em")]
    TelltaleTexasHoldEm = 0,

    [Display(Name = "Bone: Out from Boneville")]
    BoneOutFromBoneville = 1,

    [Display(Name = "CSI: 3 Dimensions of Murder / Bone (Demo)")]
    Csi3DimensionsOfMurder = 2,
    
    [Display(Name = "Bone: The Great Cow Race")]
    BoneCowRace,

    [Display(Name = "Bone: Act 1 Combo Pack")]
    BoneAct1ComboPack,

    [Display(Name = "Bone: Act 2 Combo Pack")]
    BoneAct2ComboPack,

    [Display(Name = "Sam & Max 101: Culture Shock (Demo)")]
    SamMax101Demo,

    [Display(Name = "Sam & Max 101: Culture Shock")]
    SamMax101,

    [Display(Name = "Sam & Max 102: Situation Comedy (Demo)")]
    SamMax102Demo,

    [Display(Name = "Sam & Max 102: Situation Comedy")]
    SamMax102,

    [Display(Name = "Sam & Max 103: The Mole, The Mob, and the Meatball (Demo)")]
    SamMax103Demo,

    [Display(Name = "Sam & Max 103: The Mole, The Mob, and the Meatball")]
    SamMax103,

    [Display(Name = "Sam & Max 104: Abe Lincoln Must Die! (Demo)")]
    SamMax104Demo,

    [Display(Name = "Sam & Max 104: Abe Lincoln Must Die!")]
    SamMax104,

    [Display(Name = "Sam & Max 105: Reality 2.0 (Demo)")]
    SamMax105Demo,

    [Display(Name = "Sam & Max 105: Reality 2.0")]
    SamMax105,

    [Display(Name = "Sam & Max 106: Bright Side of the Moon (Demo)")]
    SamMax106Demo,

    [Display(Name = "Sam & Max 106: Bright Side of the Moon")]
    SamMax106,

    [Display(Name = "CSI: Hard Evidence (Demo)")]
    CsiHardEvidenceDemo,

    [Display(Name = "CSI: Hard Evidence")]
    CsiHardEvidence,

    [Display(Name = "Sam & Max 201: Ice Station Santa")]
    SamMax201,

    [Display(Name = "Sam & Max 202: Moai Better Blues")]
    SamMax202,

    [Display(Name = "Sam & Max 203: Night of the Raving Dead")]
    SamMax203,

    [Display(Name = "Sam & Max 204: Chariots of the Dogs")]
    SamMax204,

    [Display(Name = "Sam & Max 205: What's New, Beelzebub?")]
    SamMax205,

    [Display(Name = "Strong Bad's Cool Game for Attractive People - Episode 1: Homestar Ruiner")]
    Sbcg4Ap101,

    [Display(Name = "Strong Bad's Cool Game for Attractive People - Episode 2: Strong Badia the Free")]
    Sbcg4Ap102,

    [Display(Name = "Strong Bad's Cool Game for Attractive People - Episode 3: Baddest of the Bands")]
    Sbcg4Ap103,

    [Display(Name = "Strong Bad's Cool Game for Attractive People - Episode 4: Dangeresque 3: The Criminal Projective")]
    Sbcg4Ap104,

    [Display(Name = "Strong Bad's Cool Game for Attractive People - Episode 5: 8-Bit Is Enough")]
    Sbcg4Ap105,

    [Display(Name = "Wallace & Gromit (Demo)")]
    WagDemo,

    [Display(Name = "Wallace & Gromit's Grand Adventures: Episode 1 - Fright of the Bumblebees")]
    Wag101,

    [Display(Name = "Wallace & Gromit's Grand Adventures: Episode 2 - The Last Resort")]
    Wag102,

    [Display(Name = "Wallace & Gromit's Grand Adventures: Episode 3 - Muzzled")]
    Wag103,

    [Display(Name = "Wallace & Gromit's Grand Adventures: Episode 4 - The Bogey Man")]
    Wag104,

    [Display(Name = "Tales of Monkey Island: Episode 1 - Launch of the Screaming Narwhal")]
    MonkeyIsland101,

    [Display(Name = "Tales of Monkey Island: Episode 2 - The Siege of Spinner Cay")]
    MonkeyIsland102,

    [Display(Name = "Tales of Monkey Island: Episode 3 - Lair of the Leviathan")]
    MonkeyIsland103,

    [Display(Name = "Tales of Monkey Island: Episode 4 - The Trial and Execution of Guybrush Threepwood")]
    MonkeyIsland104,

    [Display(Name = "Tales of Monkey Island: Episode 5 - Rise of the Pirate God")]
    MonkeyIsland105,

    [Display(Name = "CSI: Deadly Intent (Demo)")]
    CsiDeadlyDemo,

    [Display(Name = "CSI: Deadly Intent")]
    CsiDeadly,

    [Display(Name = "Sam & Max 301: The Penal Zone")]
    SamMax301,

    [Display(Name = "Sam & Max 302: The Tomb of Sammun-Mak")]
    SamMax302,

    [Display(Name = "Sam & Max 303: They Stole Max's Brain!")]
    SamMax303,

    [Display(Name = "Sam & Max 304: Beyond the Alley of the Dolls")]
    SamMax304,

    [Display(Name = "Sam & Max 305: The City That Dares Not Sleep")]
    SamMax305,

    [Display(Name = "Hector: Badge of Carnage - Episode 1: We Negotiate with Terrorists")]
    Hector101,

    [Display(Name = "Hector: Badge of Carnage - Episode 2: Senseless Act of Justice")]
    Hector102,

    [Display(Name = "Hector: Badge of Carnage - Episode 3: Beyond Reasonable Doom")]
    Hector103,

    [Display(Name = "Puzzle Agent")]
    Grickle101,

    [Display(Name = "CSI: Fatal Conspiracy")]
    CsiFatal,

    [Display(Name = "Poker Night at the Inventory")]
    CelebrityPoker,

    [Display(Name = "Back To The Future: Episode 1 - It's About Time")]
    Bttf101,

    [Display(Name = "Back To The Future: Episode 2 - Get Tannen!")]
    Bttf102,

    [Display(Name = "Back To The Future: Episode 3 - Citizen Brown")]
    Bttf103,

    [Display(Name = "Back To The Future: Episode 4 - Double Visions")]
    Bttf104,

    [Display(Name = "Back To The Future: Episode 5 - OUTATIME")]
    Bttf105,

    [Display(Name = "Puzzle Agent 2")]
    Grickle201,

    [Display(Name = "Jurassic Park: The Game")]
    JurassicPark,

    [Display(Name = "Law & Order: Legacies")]
    LawAndOrder,

    [Display(Name = "The Walking Dead")]
    Twds1,

    [Display(Name = "Poker Night 2")]
    CelebrityPoker2,

    [Display(Name = "The Wolf Among Us")]
    Fables,

    [Display(Name = "The Walking Dead: Season Two")]
    Twds2,

    [Display(Name = "Tales from the Borderlands")]
    Borderlands,

    [Display(Name = "Game of Thrones")]
    GoT,

    [Display(Name = "Minecraft: Story Mode")]
    Mcsm,

    [Display(Name = "The Walking Dead: Michonne")]
    Twdm,

    [Display(Name = "Batman: The Telltale Series")]
    Batman1,

    [Display(Name = "The Walking Dead: A New Frontier")]
    Twds3,

    [Display(Name = "Minecraft: Story Mode - Season Two")]
    Mcsm2,

    [Display(Name = "Batman: The Enemy Within")]
    Batman2,

    [Display(Name = "Guardians of the Galaxy: The Telltale Series")]
    GotG,

    [Display(Name = "The Walking Dead: The Final Season")]
    Twds4,

    [Display(Name = "The Walking Dead: The Telltale Definitive Series")]
    Twdc,

    [Display(Name = "Sam & Max Save the World Remastered")]
    SamMax100Remastered,

    [Display(Name = "Sam & Max Beyond Time and Space Remastered")]
    SamMax200Remastered,

    [Display(Name = "Sam & Max: The Devil's Playhouse Remastered")]
    SamMax300Remastered
}



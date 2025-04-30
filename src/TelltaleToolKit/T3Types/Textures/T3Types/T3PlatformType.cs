using System.ComponentModel.DataAnnotations;
using TelltaleToolKit.Reflection;
using TelltaleToolKit.Serialization;
using TelltaleToolKit.Serialization.Serializers;

namespace TelltaleToolKit.T3Types.Textures.T3Types;

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumBase>))]
public struct EnumBase
{
}

[MetaClassSerializerGlobal(typeof(DefaultClassSerializer<EnumPlatformType>))]
public struct EnumPlatformType
{
    [MetaMember("mVal")]
    public PlatformType Value { get; set; }
    
    [MetaMember("Baseclass_EnumBase")]
    public EnumBase EnumBase { get; set; }
}

[MetaClassSerializerGlobal(typeof(EnumSerializer<PlatformType>))]
public enum PlatformType
{
    [Display(Name = "Default Mode")]
    None = 0,

    [Display(Name = "Unknown Platform")]
    All = 1,

    [Display(Name = "PC")]
    PC = 2,

    [Display(Name = "Wii")]
    Wii = 3,

    [Display(Name = "Xbox 360")]
    Xbox = 4,

    [Display(Name = "PS3")]
    PS3 = 5,

    [Display(Name = "Mac")]
    Mac = 6,

    [Display(Name = "iPhone")]
    iPhone = 7,

    [Display(Name = "Android")]
    Android = 8,

    [Display(Name = "PS Vita")]
    Vita = 9,

    [Display(Name = "Linux")]
    Linux = 10,

    [Display(Name = "PS4")]
    PS4 = 11,

    [Display(Name = "Xbox One")]
    XBOne = 12,

    [Display(Name = "Wii U")]
    WiiU = 13,

    [Display(Name = "Windows 10")]
    Win10 = 14,

    [Display(Name = "Nintendo Switch")]
    NX = 15,
    Count = 16,
}

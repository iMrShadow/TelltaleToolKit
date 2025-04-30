namespace TelltaleToolKit.T3Types.Animations;

// This is internal for this project.
// C# doesn't support multiple inheritance.
// So, the only place where I use inheritance for Telltale types are the Handles (since they don't have meta members)

public interface IAnimatedValueInterface
{
    public AnimationValueInterfaceBase AnimationValueInterfaceBase { get; set; }
}
namespace TelltaleToolKit.Reflection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MetaMemberAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
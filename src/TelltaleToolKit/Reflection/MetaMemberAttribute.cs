namespace TelltaleToolKit.Reflection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class MetaMemberAttribute : Attribute
{
    public MetaMemberAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}
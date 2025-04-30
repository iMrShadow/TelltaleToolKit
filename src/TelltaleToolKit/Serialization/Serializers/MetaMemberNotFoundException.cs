namespace TelltaleToolKit.Serialization.Serializers;

public class MetaMemberNotFoundException : Exception
{
    public MetaMemberNotFoundException ()
    {}

    public MetaMemberNotFoundException (string message) 
        : base(message)
    {}

    public MetaMemberNotFoundException (string message, Exception innerException)
        : base (message, innerException)
    {} 
}
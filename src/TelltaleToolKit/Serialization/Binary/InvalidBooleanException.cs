namespace TelltaleToolKit.Serialization.Binary;

public class InvalidBooleanException : Exception
{
    public InvalidBooleanException()
    {
    }

    public InvalidBooleanException(string message)
        : base(message)
    {
    }

    public InvalidBooleanException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
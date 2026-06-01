namespace TelltaleToolKit.Serialization;

/// <summary>
/// Enumerates the different mode of serialization (either serialization or deserialization).
/// </summary>
public enum MetaStreamMode
{
    /// <summary>
    /// The serializer is in serialize mode.
    /// </summary>
    Read,

    /// <summary>
    /// The serializer is in deserialize mode.
    /// </summary>
    Write,
}

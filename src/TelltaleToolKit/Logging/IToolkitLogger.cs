using System;

namespace TelltaleToolKit.Logging;

/// <summary>
/// Defines the logging contract used internally by the library.
/// </summary>
/// <remarks>
/// Assign an implementation to <see cref="Toolkit.Configuration.Logger"/>.
/// When the property is <see langword="null"/>, all messages are silently discarded.
/// </remarks>
public interface IToolkitLogger
{
    /// <summary>
    /// Logs an informational message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void LogInfo(string message);

    /// <summary>
    /// Logs a warning that indicates a recoverable problem.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void LogWarning(string message);

    /// <summary>
    /// Logs a non-recoverable error.
    /// </summary>
    /// <param name="message">The message to log.</param>
    void LogError(string message);

    /// <summary>
    /// Logs a critical error that prevents further operation.
    /// </summary>
    void LogException(Exception? exception = null);
}

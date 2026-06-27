namespace TelltaleToolKit.Logging;

/// <summary>
/// Provides ready-made <see cref="IToolkitLogger"/> implementations.
/// </summary>
public static class ToolkitLogger
{
    /// <summary>
    /// Creates a console logger with optional timestamp and log level filtering.
    /// </summary>
    /// <param name="minLevel">Minimum log level to output (default: Info).</param>
    /// <param name="includeTimestamp">Whether to include timestamps in output (default: true).</param>
    /// <param name="includeCategory">Whether to include the [TTK] category prefix (default: true).</param>
    public static IToolkitLogger CreateConsoleLogger(
        LogLevel minLevel = LogLevel.Info,
        bool includeTimestamp = true,
        bool includeCategory = true)
        => new ConsoleLogger(minLevel, includeTimestamp, includeCategory);

    /// <summary>
    /// Gets a logger that writes all messages to <see cref="Console.Out"/> /
    /// <see cref="Console.Error"/> with a simple <c>[INFO]</c> / <c>[WARN]</c> /
    /// <c>[ERROR]</c> prefix. Uses Info level filtering with timestamps enabled.
    /// </summary>
    public static IToolkitLogger ConsoleLoggerInstance { get; } = new ConsoleLogger(LogLevel.Info, true, true);

    /// <summary>
    /// Creates a file logger that writes to the specified path with automatic rotation.
    /// </summary>
    /// <param name="filePath">Path to the log file.</param>
    /// <param name="minLevel">Minimum log level to output (default: Info).</param>
    /// <param name="maxFileSizeMb">Maximum file size in MB before rotation (0 = no rotation, default: 10).</param>
    public static IToolkitLogger CreateFileLogger(
        string filePath,
        LogLevel minLevel = LogLevel.Info,
        int maxFileSizeMb = 10)
        => new FileLogger(filePath, minLevel, maxFileSizeMb);

    /// <summary>
    /// Creates a logger that delegates to multiple loggers.
    /// </summary>
    /// <param name="loggers">The loggers to delegate to.</param>
    public static IToolkitLogger CreateCompositeLogger(params IToolkitLogger[] loggers)
        => new CompositeLogger(loggers);

    /// <summary>
    /// Gets a logger that discards every message.
    /// Equivalent to leaving <see cref="Toolkit.Configuration.Logger"/> <see langword="null"/>.
    /// </summary>
    public static IToolkitLogger Null { get; } = new NullLogger();

    // -------------------------------------------------------------------------
    // Built-in implementations
    // -------------------------------------------------------------------------

    private sealed class ConsoleLogger : IToolkitLogger
    {
        private readonly LogLevel _minLevel;
        private readonly bool _includeTimestamp;
        private readonly bool _includeCategory;

        public ConsoleLogger(LogLevel minLevel, bool includeTimestamp, bool includeCategory)
        {
            _minLevel = minLevel;
            _includeTimestamp = includeTimestamp;
            _includeCategory = includeCategory;
        }

        public void LogInfo(string message) => Log(LogLevel.Info, message);
        public void LogWarning(string message) => Log(LogLevel.Warning, message);
        public void LogError(string message) => Log(LogLevel.Error, message);
        public void LogException(Exception? exception = null) => Log(LogLevel.Exception, null, exception);

        private void Log(LogLevel level, string? message, Exception? exception = null)
        {
            if (level < _minLevel) return;

            var originalColor = Console.ForegroundColor;
            var output = new System.Text.StringBuilder();

            // Timestamp
            if (_includeTimestamp)
                output.Append($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ");

            // Level indicator with color
            switch (level)
            {
                case LogLevel.Info:
                    output.Append("[INF] ");
                    break;
                case LogLevel.Warning:
                    output.Append("[WRN] ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    output.Append("[ERR] ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogLevel.Exception:
                    output.Append("[CRT] ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }

            // Category
            if (_includeCategory)
                output.Append("[TTK] ");

            // Message
            if (!string.IsNullOrEmpty(message))
                output.Append(message);

            // Exception
            if (exception != null)
            {
                output.AppendLine();
                output.Append($"  Exception: {exception.Message}");
                output.AppendLine();
                output.Append($"  StackTrace: {exception.StackTrace}");
            }

            var consoleOut = level >= LogLevel.Error ? Console.Error : Console.Out;
            consoleOut.WriteLine(output.ToString());
            Console.ForegroundColor = originalColor;
        }
    }

    private sealed class FileLogger : IToolkitLogger
    {
        private readonly string _filePath;
        private readonly LogLevel _minLevel;
        private readonly int _maxFileSizeMb;
        private readonly object _lock = new();

        public FileLogger(string filePath, LogLevel minLevel, int maxFileSizeMb)
        {
            _filePath = filePath;
            _minLevel = minLevel;
            _maxFileSizeMb = maxFileSizeMb;

            // Ensure directory exists
            string? directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);
        }

        public void LogInfo(string message) => WriteLog(LogLevel.Info, message, null);
        public void LogWarning(string message) => WriteLog(LogLevel.Warning, message, null);
        public void LogError(string message) => WriteLog(LogLevel.Error, message, null);
        public void LogException(Exception? exception = null) => WriteLog(LogLevel.Exception, null, exception);

        private void WriteLog(LogLevel level, string? message, Exception? exception)
        {
            if (level < _minLevel) return;

            lock (_lock)
            {
                RotateIfNeeded();

                var output = new System.Text.StringBuilder();
                output.Append($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} ");

                switch (level)
                {
                    case LogLevel.Info:
                        output.Append("[INF] ");
                        break;
                    case LogLevel.Warning:
                        output.Append("[WRN] ");
                        break;
                    case LogLevel.Error:
                        output.Append("[ERR] ");
                        break;
                    case LogLevel.Exception:
                        output.Append("[CRT] ");
                        break;
                }

                output.Append("[TTK] ");

                if (!string.IsNullOrEmpty(message))
                    output.Append(message);

                if (exception != null)
                {
                    output.AppendLine();
                    output.Append($"  Exception: {exception.Message}");
                    output.AppendLine();
                    output.Append($"  StackTrace: {exception.StackTrace}");
                }

                File.AppendAllText(_filePath, output + Environment.NewLine);
            }
        }

        private void RotateIfNeeded()
        {
            if (_maxFileSizeMb <= 0) return;
            if (!File.Exists(_filePath)) return;

            var fileInfo = new FileInfo(_filePath);
            if (fileInfo.Length < _maxFileSizeMb * 1024 * 1024) return;

            // Rotate: file.log -> file_1.log, file_1.log -> file_2.log, etc.
            for (int i = 9; i >= 1; i--)
            {
                string oldFile = $"{_filePath}_{i}.log";
                string newFile = $"{_filePath}_{i + 1}.log";
                if (File.Exists(oldFile))
                    File.Move(oldFile, newFile);
            }

            if (File.Exists(_filePath))
                File.Move(_filePath, $"{_filePath}_1.log");
        }
    }

    private sealed class CompositeLogger : IToolkitLogger
    {
        private readonly IToolkitLogger[] _loggers;

        public CompositeLogger(params IToolkitLogger[] loggers)
        {
            _loggers = loggers;
        }

        public void LogInfo(string message)
        {
            foreach (var logger in _loggers)
                logger.LogInfo(message);
        }

        public void LogWarning(string message)
        {
            foreach (var logger in _loggers)
                logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            foreach (var logger in _loggers)
                logger.LogError(message);
        }

        public void LogException(Exception? exception = null)
        {
            foreach (var logger in _loggers)
                logger.LogException(exception);
        }
    }

    private sealed class NullLogger : IToolkitLogger
    {
        public void LogInfo(string message)
        {
        }

        public void LogWarning(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogException(Exception? exception = null)
        {
        }
    }
}

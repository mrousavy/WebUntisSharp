namespace WebUntisSharp {
    public class Logger {
        public enum LogLevel { Debug, Info, Error, Critical };

        public LogList Messages;
        public LogLevel MinimumLogLevel;

        public delegate void LogAppendHandler(LogLevel level, string message);
        public event LogAppendHandler NewMessage;

        public Logger() {
            Messages = new LogList();
            Append(LogLevel.Debug, "Logger initialized.");
        }

        public Logger(LogLevel minLevel) {
            Messages = new LogList();
        }

        public void Append(LogLevel level, string message) {
            if (level >= MinimumLogLevel) {
                NewMessage?.Invoke(level, message);
                Messages.Add(level, message);
            }
        }
    }
}
